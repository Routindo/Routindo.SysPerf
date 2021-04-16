using System;
using System.IO;
using System.Linq;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    [PluginItemInfo(ComponentUniqueId, "Drive Usage Watcher",
         "Watch Drive Usage and notify when usage reaches the target maximum"),
     ResultArgumentsClass(typeof(UsageWatcherResultArgs))]
    public class DriveUsageWatcher: IWatcher
    {
        public const string ComponentUniqueId = "9E443A59-446A-49B5-BE9A-0AE0B8D223FB";

        public string Id { get; set; }

        public ILoggingService LoggingService { get; set; }

        [Argument(DriveUsageWatcherArgs.DriveName, true)] public string DriveName { get; set; }

        [Argument(DriveUsageWatcherArgs.MaximumUsage, true)] public int TargetMaximumUsage { get; set; } = 99;

        [Argument(DriveUsageWatcherArgs.NotificationTimeIntervalSeconds, true)] public int NotificationTimeIntervalSeconds { get; set; } = 0;

        private double _lastUsage = 0;
        private double _lastNotificationUsage = 0;
        private DateTime _lastNotificationTime;

        public WatcherResult Watch()
        {
            try
            {
                var usage = GetDriveTotalUsage(DriveName);

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
                            .WithArgument(DriveUsageWatcherResultArgs.Usage, usage.ToString("F"))
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

        private double GetDriveTotalUsage(string driveName)
        {
            var drive = DriveInfo.GetDrives().SingleOrDefault(d => d.IsReady && d.Name == driveName);
            if (drive == null)
            {
                LoggingService.Error($"Drive ({driveName}) not found or not ready.");
                return -1;
            }

            if (drive.TotalSize <= 0)
            {
                LoggingService.Error($"Drive ({driveName}) has not a total size: ({drive.TotalSize})");
                return -1;
            }

            return ((drive.TotalSize - drive.TotalFreeSpace)*1.0)/(drive.TotalSize * 1.0) * 100.0;
        }

        private bool CanNotify()
        {
            var totalSecondsSinceLastNotification = DateTime.Now.Subtract(_lastNotificationTime).TotalSeconds;
            // LoggingService.Debug($"Total Seconds since last notification: {totalSecondsSinceLastNotification}");
            return totalSecondsSinceLastNotification >= NotificationTimeIntervalSeconds;
        }
    }

    public static class DriveUsageWatcherArgs
    {
        public const string DriveName = nameof(DriveName);
        public const string MaximumUsage = nameof(MaximumUsage);
        public const string NotificationTimeIntervalSeconds = nameof(NotificationTimeIntervalSeconds);
    }

    public static class DriveUsageWatcherResultArgs
    {
        public const string Usage = nameof(Usage);
    }
}
