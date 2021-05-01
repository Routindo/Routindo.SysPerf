using System.Diagnostics;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    [PluginItemInfo(ComponentUniqueId, nameof(MemoryUsageWatcher),
         "Watch Memory Usage and notify when usage reaches the target maximum", Category = "System Resources", FriendlyName = "Monitor RAM Usage"),
     ResultArgumentsClass(typeof(UsageWatcherResultArgs))]
    public class MemoryUsageWatcher : PerformanceCounterWatcher
    {
        public const string ComponentUniqueId = "8B008F94-9621-44D1-A187-D402F3BBD581";

        public MemoryUsageWatcher() : base(new PerformanceCounter("Memory", "% Committed Bytes In Use", true))
        {
        }
    }
}
