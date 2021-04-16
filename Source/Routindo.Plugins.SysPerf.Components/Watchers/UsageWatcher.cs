using System;
using System.Diagnostics;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{ 
    public abstract class PerformanceCounterWatcher : IWatcher
    {
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        protected readonly PerformanceCounter Counter;

        [Argument(UsageWatcherArgs.MaximumUsage, true)] public int TargetMaximumUsage { get; set; } = 99;

        [Argument(UsageWatcherArgs.NotificationTimeIntervalSeconds, true)] public int NotificationTimeIntervalSeconds { get; set; } = 0;

        private float _lastUsage = 0;
        private float _lastNotificationUsage = 0;

        private DateTime _lastNotificationTime;

        protected PerformanceCounterWatcher(PerformanceCounter performanceCounter)
        {
            Counter = performanceCounter;
        }

        public WatcherResult Watch()
        {
            try
            {
                var usage = Counter.NextValue();

                // LoggingService.Debug($"Last Usage: {_lastUsage}, Current Usage: {usage}, Target {TargetMaximumUsage}");
                if (usage > TargetMaximumUsage && CanNotify())
                {
                    // Notify only for the first occurrence when exceeding the target maximum
                    if (_lastUsage < TargetMaximumUsage 
                        // Current usage is greater than previous notification usage
                        || _lastNotificationUsage < usage)
                    {
                        _lastUsage = usage;
                        _lastNotificationUsage = usage;
                        _lastNotificationTime = DateTime.Now;
                        return WatcherResult.Succeed(ArgumentCollection.New()
                            .WithArgument(UsageWatcherResultArgs.Usage, usage.ToString("F"))
                        );
                    }
                }

                _lastUsage = usage;

                return WatcherResult.NotFound;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return WatcherResult.NotFound;
            }
        }

        private bool CanNotify()
        {
            var totalSecondsSinceLastNotification = DateTime.Now.Subtract(_lastNotificationTime).TotalSeconds;
            // LoggingService.Debug($"Total Seconds since last notification: {totalSecondsSinceLastNotification}");
            return totalSecondsSinceLastNotification >= NotificationTimeIntervalSeconds;
        }
    }
}