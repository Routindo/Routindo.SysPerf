using System;
using System.Diagnostics;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    public abstract class UsageWatcher: IWatcher
    {
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        protected readonly PerformanceCounter Counter;

        [Argument(UsageWatcherArgs.MaximumUsage, true)] public int TargetMaximumUsage { get; set; } = 99;

        private float _lastUsage = 0;

        protected UsageWatcher(PerformanceCounter performanceCounter)
        {
            Counter = performanceCounter;
        }

        public WatcherResult Watch()
        {
            try
            {
                var usage = Counter.NextValue();

                if (usage > TargetMaximumUsage)
                {
                    // Notify only for the first occurrence when exceeding the target maximum
                    if (_lastUsage < TargetMaximumUsage)
                    {
                        return WatcherResult.Succeed(ArgumentCollection.New()
                            .WithArgument(UsageWatcherResultArgs.Usage, usage.ToString("F"))
                        );
                    }

                    _lastUsage = usage;
                }

                return WatcherResult.NotFound;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return WatcherResult.NotFound;
            }
        }
    }

    public static class UsageWatcherArgs
    {
        public const string MaximumUsage = nameof(MaximumUsage);
    }

    public static class UsageWatcherResultArgs
    {
        public const string Usage = nameof(Usage);
    }
}
