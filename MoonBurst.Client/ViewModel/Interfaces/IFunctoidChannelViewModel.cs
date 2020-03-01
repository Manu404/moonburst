using MoonBurst.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MoonBurst.ViewModel
{
    public interface IFunctoidChannelViewModel : IViewModel
    {
        ObservableCollection<IFunctoidActionViewModel> Actions { get; set; }
        int ArduinoBitMask { get; set; }
        int Index { get; set; }
        bool IsEnabled { get; set; }
        bool IsExpanded { get; set; }
        bool IsTriggered { get; set; }
        string Name { get; set; }
        DeviceInputViewModel SelectedInput { get; set; }

        void RefreshInputs();
        void TryBindInput(string bindedInput);
    }
}