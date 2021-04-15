using System.Diagnostics;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    public class MemoryUsageWatcher : UsageWatcher
    {
        public MemoryUsageWatcher() : base(new PerformanceCounter("Memory", "% Committed Bytes In Use", true))
        {
        }
    }
}
