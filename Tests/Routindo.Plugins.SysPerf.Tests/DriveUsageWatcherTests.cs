using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;
using Routindo.Plugins.SysPerf.Components.Watchers;

namespace Routindo.Plugins.SysPerf.Tests
{
    [TestClass]
    public class DriveUsageWatcherTests
    {
        [TestMethod]
        [TestCategory("Unit Test")]
        public void WatchDriveUsageSuccessfulTest() 
        {
            var drives = DriveInfo.GetDrives().ToList();

            IWatcher watcher = new DriveUsageWatcher()
            {
                TargetMaximumUsage = 1,
                DriveName = drives.First().Name, 
                NotificationTimeIntervalSeconds = 0,
                LoggingService = ServicesContainer.ServicesProvider.GetLoggingService(nameof(DriveUsageWatcher))
            };

            var watcherResult = watcher.Watch();
            Assert.IsNotNull(watcherResult);
            Assert.IsTrue(watcherResult.Result);
            Assert.IsTrue(watcherResult.WatchingArguments.HasArgument(UsageWatcherResultArgs.Usage));
            var usage = watcherResult.WatchingArguments.GetValue<string>(UsageWatcherResultArgs.Usage);
            Assert.IsNotNull(usage);
            Console.WriteLine($"Usage: {usage}");
            //Thread.Sleep(1000);
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(500);
            //    watcherResult = watcher.Watch();
                
            //}

        }
    }
}
