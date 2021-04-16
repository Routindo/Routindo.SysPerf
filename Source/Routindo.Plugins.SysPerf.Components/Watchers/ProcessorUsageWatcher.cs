using System.Diagnostics;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.SysPerf.Components.Watchers
{
    [PluginItemInfo(ComponentUniqueId, "CPU Usage Watcher",
         "Watch Processor Usage and notify when usage reaches the target maximum"),
     ResultArgumentsClass(typeof(UsageWatcherResultArgs))]
    public class ProcessorUsageWatcher : UsageWatcher
    {
        public const string ComponentUniqueId = "9DFD07C0-6968-49FC-8555-53670C624361";

        public ProcessorUsageWatcher() : base(new PerformanceCounter("Processor", "% Processor Time", "_Total", true))
        {
        }
    }
}
