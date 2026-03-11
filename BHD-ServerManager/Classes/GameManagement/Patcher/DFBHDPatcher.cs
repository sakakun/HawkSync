using System;
using System.IO;

/// <summary>
/// Patches dfbhd.exe in two independent areas:
///
///   1. Max-players cap: raises the server player limit from 50/51 to 80/81.
///
///   2. 4-team PSP announcements: in the original binary sub_4AFD70 and sub_4AFE00
///      both contain a hard-coded early-return for teams other than 1 (Red) and
///      2 (Blue), which silently skips the network message that triggers the
///      "enemy is taking over a spawn point" audio cue and status-text overlay.
///      NOPing those two conditional jumps lets teams 3 (Yellow) and 4 (Violet)
///      reach the same broadcast path as Red and Blue.
///
/// Call Patch() before starting the server process, Unpatch() after stopping it.
/// Safe to call multiple times — IsPatched() guards against double-patching.
/// </summary>
/// 
namespace BHD_ServerManager.Classes.GameManagement.Patcher;

public static class DFBHDPatcher
{
    private struct PatchSite
    {
        public readonly long   Offset;
        public readonly byte   Original;   // 0x32 = 50, 0x33 = 51
        public readonly byte   Patched;    // 0x50 = 80, 0x51 = 81
        public readonly string Description;

        public PatchSite(long offset, byte original, byte patched, string description)
        {
            Offset      = offset;
            Original    = original;
            Patched     = patched;
            Description = description;
        }
    }

    private static readonly PatchSite[] Sites =
    {
        // ── Max-players cap (50/51 → 80/81) ─────────────────────────────────────
        new PatchSite(0x0D979E, 0x32, 0x50, "Config parser cmp 50->80"),
        new PatchSite(0x0D97B1, 0x32, 0x50, "Config parser mov 50->80"),
        new PatchSite(0x0A4A7A, 0x33, 0x51, "Map change cmp 51->81"),
        new PatchSite(0x0A4A7E, 0x33, 0x51, "Map change mov 51->81"),
        new PatchSite(0x0DB563, 0x33, 0x51, "Session start cmp 51->81"),
        new PatchSite(0x0DB56C, 0x33, 0x51, "Session start mov 51->81"),
        new PatchSite(0x0DD0DF, 0x32, 0x50, "UI browser cmp 50->80"),
        new PatchSite(0x0DD0E3, 0x32, 0x50, "UI browser push 50->80"),
        new PatchSite(0x0B13EA, 0x33, 0x51, "Allocator guard cmp 51->81"),
    };

	public enum PatchState
    {
        Unpatched,      // All sites have original bytes
        Patched,        // All sites have patched bytes
        Partial,        // Mix of original and patched — file may be corrupt
        FileMissing,    // exe not found
        Unknown,        // Byte values don't match either expected state
    }

    /// <summary>
    /// Inspects the exe and returns its current patch state without modifying it.
    /// </summary>
    public static PatchState CheckState(string exePath)
    {
        if (!File.Exists(exePath))
            return PatchState.FileMissing;

        using var fs = new FileStream(exePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        int patchedCount   = 0;
        int originalCount  = 0;
        int unknownCount   = 0;

        foreach (var site in Sites)
        {
            fs.Position = site.Offset;
            int b = fs.ReadByte();
            if (b == site.Patched)        patchedCount++;
            else if (b == site.Original)  originalCount++;
            else                          unknownCount++;
        }

        if (unknownCount > 0)        return PatchState.Unknown;
        if (patchedCount == Sites.Length)  return PatchState.Patched;
        if (originalCount == Sites.Length) return PatchState.Unpatched;
        return PatchState.Partial;
    }

    /// <summary>
    /// Patches the exe. Returns true if patching was performed, false if already patched.
    /// Throws InvalidOperationException if the file is in an unexpected state.
    /// </summary>
    public static bool Patch(string exePath)
    {
        var state = CheckState(exePath);

        switch (state)
        {
            case PatchState.Patched:
                return false;   // already done, nothing to do

            case PatchState.FileMissing:
                throw new FileNotFoundException("dfbhd.exe not found.", exePath);

            case PatchState.Partial:
            case PatchState.Unknown:
                throw new InvalidOperationException(
                    $"Cannot patch: file is in an unexpected state ({state}). " +
                    "Restore from backup before proceeding.");
        }

        ApplyBytes(exePath, usePatchedValues: true);
        return true;
    }

    /// <summary>
    /// Restores the exe to original bytes. Returns true if unpatch was performed,
    /// false if already unpatched.
    /// Throws InvalidOperationException if the file is in an unexpected state.
    /// </summary>
    public static bool Unpatch(string exePath)
    {
        var state = CheckState(exePath);

        switch (state)
        {
            case PatchState.Unpatched:
                return false;   // already original, nothing to do

            case PatchState.FileMissing:
                throw new FileNotFoundException("dfbhd.exe not found.", exePath);

            case PatchState.Partial:
            case PatchState.Unknown:
                throw new InvalidOperationException(
                    $"Cannot unpatch: file is in an unexpected state ({state}). " +
                    "Restore from backup before proceeding.");
        }

        ApplyBytes(exePath, usePatchedValues: false);
        return true;
    }

    private static void ApplyBytes(string exePath, bool usePatchedValues)
    {
        using var fs = new FileStream(exePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        foreach (var site in Sites)
        {
            fs.Position = site.Offset;
            fs.WriteByte(usePatchedValues ? site.Patched : site.Original);
        }
        fs.Flush();
    }
}
