using System.Diagnostics;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    public class ProcessorUsageWatcher : UsageWatcher
    {
        public ProcessorUsageWatcher() : base(new PerformanceCounter("Processor", "% Processor Time", "_Total", true))
        {
        }
    }
}
