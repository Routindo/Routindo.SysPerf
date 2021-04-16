using Routindo.Contract.Attributes;
using Routindo.Plugins.SysPerf.Components.Watchers;
using Routindo.Plugins.SysPerf.UI.Views;

[assembly: ComponentConfigurator(typeof(ProcessorUsageWatcherView), ProcessorUsageWatcher.ComponentUniqueId, "Configurator for CPU Usage Watcher")]
[assembly: ComponentConfigurator(typeof(MemoryUsageWatcherView), MemoryUsageWatcher.ComponentUniqueId, "Configurator for RAM Usage Watcher")]
[assembly: ComponentConfigurator(typeof(DriveUsageWatcherView), DriveUsageWatcher.ComponentUniqueId, "Configurator for Drive Usage Watcher")]