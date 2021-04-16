using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.SysPerf.Components.Watchers;
using Routindo.Plugins.SysPerf.UI.Enums;
using Routindo.Plugins.SysPerf.UI.Models;

namespace Routindo.Plugins.SysPerf.UI.ViewModels
{
    public class DriveUsageWatcherViewModel : PluginConfiguratorViewModelBase
    {
        private int _targetMaximumUsage = 1;
        private int _timeInterval = 5;
        private TimeIntervalUnit _timeIntervalUnit = TimeIntervalUnit.Minute;
        private DriveModel _selectedDrive;

        public DriveUsageWatcherViewModel()
        {
            this.TimeIntervalUnits = new ObservableCollection<TimeIntervalUnit>(Enum.GetValues<TimeIntervalUnit>());
            this.Drives =
                new ObservableCollection<DriveModel>(DriveInfo.GetDrives()
                    .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
                    .Select(d => new DriveModel(d.Name, d.VolumeLabel)));
        }

        public int TargetMaximumUsage
        {
            get => _targetMaximumUsage;
            set
            {
                _targetMaximumUsage = value;
                ClearPropertyErrors();
                ValidateNumber(_targetMaximumUsage, i => i > 0 && i <= 100);
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

        public DriveModel SelectedDrive
        {
            get => _selectedDrive;
            set
            {
                ClearPropertyErrors();
                _selectedDrive = value;
                ValidateNoNullProperty(_selectedDrive);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeIntervalUnit> TimeIntervalUnits { get; set; }

        public ObservableCollection<DriveModel> Drives { get; set; }

        public override void Configure()
        {
            int timeIntervalSeconds = TimeInterval * (this.TimeIntervalUnit switch
            {
                TimeIntervalUnit.Hour => 60 * 60,
                TimeIntervalUnit.Minute => 60,
                _ => 1
            });

            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(DriveUsageWatcherArgs.MaximumUsage, TargetMaximumUsage)
                .WithArgument(DriveUsageWatcherArgs.DriveName, SelectedDrive?.Name)
                .WithArgument(DriveUsageWatcherArgs.NotificationTimeIntervalSeconds, timeIntervalSeconds);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(DriveUsageWatcherArgs.MaximumUsage))
                TargetMaximumUsage = arguments.GetValue<int>(DriveUsageWatcherArgs.MaximumUsage);

            if (arguments.HasArgument(DriveUsageWatcherArgs.DriveName))
            {
                var driveName = arguments.GetValue<string>(DriveUsageWatcherArgs.DriveName);
                this.SelectedDrive = this.Drives.SingleOrDefault(d => String.Equals(d.Name, driveName, StringComparison.CurrentCultureIgnoreCase));
            }

            if (arguments.HasArgument(DriveUsageWatcherArgs.NotificationTimeIntervalSeconds))
            {
                var timeInterval = arguments.GetValue<int>(DriveUsageWatcherArgs.NotificationTimeIntervalSeconds);
                if (timeInterval % (60 * 60) == 0)
                {
                    TimeInterval = timeInterval / (60 * 60);
                    this.TimeIntervalUnit = TimeIntervalUnit.Hour;
                }
                else if (timeInterval % (60) == 0)
                {
                    TimeInterval = timeInterval / 60;
                    this.TimeIntervalUnit = TimeIntervalUnit.Minute;
                }
                else
                {
                    TimeInterval = timeInterval;
                    this.TimeIntervalUnit = TimeIntervalUnit.Seconds;
                }
            }
        }

        private void ValidateNoNullProperty(object obj, [CallerMemberName] string propertyName = null)
        {
            if (obj != null)
                return;
            this.AddPropertyError(propertyName, "This field cannot be null");
        }
    }
}
