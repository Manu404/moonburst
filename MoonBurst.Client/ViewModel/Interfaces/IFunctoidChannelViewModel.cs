using System.Collections.ObjectModel;
using MoonBurst.Core;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface IFunctoidChannelViewModel : IViewModel
    {
        ObservableCollection<IFunctoidActionViewModel> Actions { get; set; }
        int ArduinoBitMask { get; set; }
        int Index { get; set; }
        bool IsEnabled { get; set; }
        bool IsExpanded { get; set; }
        bool IsTriggered { get; set; }
        bool IsLocked { get; set; }
        string Name { get; set; }
        IDeviceInputViewModel SelectedInput { get; set; }

        void RefreshInputs();
        void TryBindInput(string bindedInput);
    }
}