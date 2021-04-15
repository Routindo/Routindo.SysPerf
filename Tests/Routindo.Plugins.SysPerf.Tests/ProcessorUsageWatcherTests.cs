using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;
using Routindo.Plugins.SysPerf.Components.Watchers;

namespace Routindo.Plugins.SysPerf.Tests
{
    [TestClass]
    public class ProcessorUsageWatcherTests
    {
        [TestMethod]
        [TestCategory("Unit Test")]
        public void WatchProcessorUsageSuccessfulTest()
        {
            IWatcher watcher = new ProcessorUsageWatcher()
            {
                TargetMaximumUsage = 1, 
                LoggingService = ServicesContainer.ServicesProvider.GetLoggingService(nameof(ProcessorUsageWatcher))
            };

            var watcherResult = watcher.Watch();
            Assert.IsNotNull(watcherResult);
            Assert.IsFalse(watcherResult.Result);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                watcherResult = watcher.Watch();
                Assert.IsNotNull(watcherResult);
                Assert.IsTrue(watcherResult.Result);
                Assert.IsTrue(watcherResult.WatchingArguments.HasArgument(UsageWatcherResultArgs.Usage));
                var usage = watcherResult.WatchingArguments.GetValue<string>(UsageWatcherResultArgs.Usage);
                Assert.IsNotNull(usage);
                Console.WriteLine($"Usage: {usage}");
            }
            
        }
    }
}
