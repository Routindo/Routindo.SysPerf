using Routindo.Contract.UI;
using System.Linq;
using Routindo.Contract.Arguments;
using Routindo.Plugins.SysPerf.Components.Watchers;

namespace Routindo.Plugins.SysPerf.UI.ViewModels
{
    public abstract class UsageWatcherViewModel : PluginConfiguratorViewModelBase
    { 
        private int _targetMaximumUsage = 1;

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

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(UsageWatcherArgs.MaximumUsage, TargetMaximumUsage);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(UsageWatcherArgs.MaximumUsage))
                TargetMaximumUsage = arguments.GetValue<int>(UsageWatcherArgs.MaximumUsage);
        }
    }
}
