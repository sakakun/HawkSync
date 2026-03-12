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

        // Read entire file to detect 4-byte immediate patterns for team-3/4
        fs.Position = 0;
        var all = new byte[fs.Length];
        fs.Read(all, 0, all.Length);

        byte[] t3Orig = { 0x07, 0x10, 0x00, 0x00 };
        byte[] t4Orig = { 0x06, 0x10, 0x00, 0x00 };
        // updated to use type_id 1563 (0x061B) and 1564 (0x061C)
        byte[] t3Pat  = { 0x1B, 0x06, 0x00, 0x00 };
        byte[] t4Pat  = { 0x1C, 0x06, 0x00, 0x00 };

        bool t3HasOrig = ContainsSequence(all, t3Orig);
        bool t4HasOrig = ContainsSequence(all, t4Orig);
        bool t3HasPat  = ContainsSequence(all, t3Pat);
        bool t4HasPat  = ContainsSequence(all, t4Pat);

        if (patchedCount == Sites.Length && !t3HasOrig && !t4HasOrig && (t3HasPat || t4HasPat))
            return PatchState.Patched;

        if (originalCount == Sites.Length && !t3HasPat && !t4HasPat && (t3HasOrig || t4HasOrig))
            return PatchState.Unpatched;

        return PatchState.Partial;
    }

    private static bool ContainsSequence(byte[] buffer, byte[] seq)
    {
        for (int i = 0; i + seq.Length <= buffer.Length; i++)
        {
            bool ok = true;
            for (int j = 0; j < seq.Length; j++) if (buffer[i + j] != seq[j]) { ok = false; break; }
            if (ok) return true;
        }
        return false;
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
                // Instead of failing hard, create a timestamped backup and proceed with patching.
                var backup = exePath + ".bak." + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                File.Copy(exePath, backup, overwrite: false);
                Console.Error.WriteLine($"Warning: exe in unexpected state ({state}). Backup created: {backup}. Proceeding to apply patches.");
                break;
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
                // Try to locate and restore a timestamped backup created earlier by the patcher or manual scripts.
                try
                {
                    var dir = Path.GetDirectoryName(exePath) ?? ".";
                    var pattern = Path.GetFileName(exePath) + ".bak.*";
                    var matches = Directory.GetFiles(dir, pattern).OrderByDescending(f => f).ToArray();
                    if (matches.Length > 0)
                    {
                        // restore the newest backup
                        File.Copy(matches[0], exePath, overwrite: true);
                        Console.Error.WriteLine($"Restored backup {matches[0]} over {exePath}.");
                        return true;
                    }
                }
                catch { /* ignore and fall back to ApplyBytes */ }
                // If no backup found, fall back to attempting to unpatch by replacing patched bytes with originals.
                break;
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

        // Also perform 4-byte immediate replacements for bay type ids
        // Team3 original: 0x00001007 -> bytes {0x07,0x10,0x00,0x00}
        // Team4 original: 0x00001006 -> bytes {0x06,0x10,0x00,0x00}
        // Team3 patched : 0x0000061B -> bytes {0x1B,0x06,0x00,0x00} (type_id 1563)
        // Team4 patched : 0x0000061C -> bytes {0x1C,0x06,0x00,0x00} (type_id 1564)

        var whole = new byte[fs.Length];
        fs.Position = 0;
        fs.Read(whole, 0, whole.Length);

        byte[] t3Orig = { 0x07, 0x10, 0x00, 0x00 };
        byte[] t4Orig = { 0x06, 0x10, 0x00, 0x00 };

        byte[] t3Pat  = { 0x43, 0x08, 0x00, 0x00 };
        byte[] t4Pat  = { 0xC9, 0x05, 0x00, 0x00 };

        bool changed = false;
        if (usePatchedValues)
        {
            changed |= ReplaceAllInBuffer(whole, t3Orig, t3Pat);
            changed |= ReplaceAllInBuffer(whole, t4Orig, t4Pat);
        }
        else
        {
            changed |= ReplaceAllInBuffer(whole, t3Pat, t3Orig);
            changed |= ReplaceAllInBuffer(whole, t4Pat, t4Orig);
        }

        if (changed)
        {
            fs.Position = 0;
            fs.Write(whole, 0, whole.Length);
        }

        fs.Flush();
    }

    private static bool ReplaceAllInBuffer(byte[] buffer, byte[] needle, byte[] replacement)
    {
        bool any = false;
        for (int i = 0; i + needle.Length <= buffer.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < needle.Length; j++)
            {
                if (buffer[i + j] != needle[j]) { match = false; break; }
            }
            if (!match) continue;
            for (int j = 0; j < needle.Length; j++) buffer[i + j] = replacement[j];
            any = true;
            i += needle.Length - 1;
        }
        return any;
    }
}
