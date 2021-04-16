using System.Diagnostics;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    [PluginItemInfo(ComponentUniqueId, "RAM Usage Watcher",
         "Watch Memory Usage and notify when usage reaches the target maximum"),
     ResultArgumentsClass(typeof(UsageWatcherResultArgs))]
    public class MemoryUsageWatcher : UsageWatcher
    {
        public const string ComponentUniqueId = "8B008F94-9621-44D1-A187-D402F3BBD581";

        public MemoryUsageWatcher() : base(new PerformanceCounter("Memory", "% Committed Bytes In Use", true))
        {
        }
    }
}
