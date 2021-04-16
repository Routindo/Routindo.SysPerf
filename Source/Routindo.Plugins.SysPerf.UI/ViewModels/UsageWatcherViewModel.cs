using System;
using System.Collections.ObjectModel;
using Routindo.Contract.UI;
using System.Linq;
using Routindo.Contract.Arguments;
using Routindo.Plugins.SysPerf.Components.Watchers;
using Routindo.Plugins.SysPerf.UI.Enums;

namespace Routindo.Plugins.SysPerf.UI.ViewModels
{
    public abstract class UsageWatcherViewModel : PluginConfiguratorViewModelBase
    { 
        private int _targetMaximumUsage = 1;
        private int _timeInterval = 5;
        private TimeIntervalUnit _timeIntervalUnit = TimeIntervalUnit.Minute;

        public UsageWatcherViewModel()
        {
            this.TimeIntervalUnits = new ObservableCollection<TimeIntervalUnit>(Enum.GetValues<TimeIntervalUnit>());
        }

        public int TargetMaximumUsage
        {
            get => _targetMaximumUsage;
            set
            {
                _targetMaximumUsage = value;
                ClearPropertyErrors();
                ValidateNumber(_targetMaximumUsage, i => i>0 && i<=100);
                OnPropertyChanged();
            }
        }

        public int TimeInterval
        {
            get => _timeInterval;
            set
            {
                _timeInterval = value;
                ClearPropertyErrors();
                ValidateNumber(_targetMaximumUsage, i => i > 0);
                OnPropertyChanged();
            }
        }

        public TimeIntervalUnit TimeIntervalUnit
        {
            get => _timeIntervalUnit;
            set
            {
                _timeIntervalUnit = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeIntervalUnit> TimeIntervalUnits { get; set; }

        public override void Configure()
        {
            int timeIntervalSeconds = TimeInterval * (this.TimeIntervalUnit switch
            {
                TimeIntervalUnit.Hour => 60 * 60,
                TimeIntervalUnit.Minute => 60,
                _ => 1
            });

            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(UsageWatcherArgs.MaximumUsage, TargetMaximumUsage)
                .WithArgument(UsageWatcherArgs.NotificationTimeIntervalSeconds, timeIntervalSeconds);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(UsageWatcherArgs.MaximumUsage))
                TargetMaximumUsage = arguments.GetValue<int>(UsageWatcherArgs.MaximumUsage);

            if (arguments.HasArgument(UsageWatcherArgs.NotificationTimeIntervalSeconds))
            {
                var timeInterval = arguments.GetValue<int>(UsageWatcherArgs.NotificationTimeIntervalSeconds);
                if (timeInterval % (60 * 60) == 0)
                {
                    TimeInterval = timeInterval / (60* 60);
                    this.TimeIntervalUnit = TimeIntervalUnit.Hour;
                }
                else if (timeInterval % (60) == 0)
                {
                    TimeInterval = timeInterval  / 60;
                    this.TimeIntervalUnit = TimeIntervalUnit.Minute;
                }
                else
                {
                    TimeInterval = timeInterval;
                    this.TimeIntervalUnit = TimeIntervalUnit.Seconds;
                }
            }
        }
    }
}
