using System;
using System.Collections.Generic;
using Timer = System.Timers.Timer;

namespace BHD_ServerManager.Classes.SupportClasses
{
    public class Ticker
    {
        private readonly object _tickerLock = new object();

        private class TickerItem
        {
            public Timer? Timer { get; set; }
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public Action? TickAction { get; set; }
        }

        // Dictionary to store multiple tickers
        private readonly Dictionary<string, TickerItem> _tickers;

        public Ticker()
        {
            _tickers = new Dictionary<string, TickerItem>();
        }

        // Start a ticker with a given name, interval, and action to execute
        public void Start(string name, double intervalInMilliseconds, Action action)
        {
            lock (_tickerLock)
            {
                if (_tickers.ContainsKey(name))
                {
                    // If ticker already exists, update interval and action, then restart
                    var tickerItem = _tickers[name];
                    tickerItem.Timer!.Interval = intervalInMilliseconds;
                    tickerItem.TickAction = action;
                    tickerItem.Timer.Start();
                }
                else
                {
                    // Create a new ticker if it doesn't exist
                    var timer = new Timer(intervalInMilliseconds);
                    timer.Elapsed += (_, _) => action(); // Assign the action to call on each tick
                    timer.AutoReset = true;

                    var tickerItem = new TickerItem
                    {
                        Timer = timer,
                        TickAction = action
                    };

                    _tickers[name] = tickerItem;
                    timer.Start();
                }
            }
        }

        // Stop a ticker by name
        public void Stop(string name)
        {
            if (_tickers.TryGetValue(name, out var tickerItem))
            {
                tickerItem.Timer!.Stop();
            }
        }

        // Stop all tickers
        public void StopAll()
        {
            foreach (var ticker in _tickers.Values)
            {
                ticker.Timer!.Stop();
            }
        }

        // Set or update the interval of a specific ticker
        public void SetInterval(string name, double intervalInMilliseconds)
        {
            if (_tickers.TryGetValue(name, out var tickerItem))
            {
                tickerItem.Timer!.Interval = intervalInMilliseconds;
            }
        }

        // Remove a ticker entirely
        public void Remove(string name)
        {
            if (_tickers.TryGetValue(name, out var tickerItem))
            {
                tickerItem.Timer!.Stop();
                tickerItem.Timer.Dispose();
                _tickers.Remove(name);
            }
        }

        // Check if a specific ticker is running
        public bool IsRunning(string name)
        {
            return _tickers.TryGetValue(name, out var tickerItem) && tickerItem.Timer!.Enabled;
        }
    }
}