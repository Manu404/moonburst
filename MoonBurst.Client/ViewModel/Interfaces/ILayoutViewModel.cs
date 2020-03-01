using MoonBurst.Core;
using MoonBurst.Model;
using System.Collections.ObjectModel;

namespace MoonBurst.ViewModel
{
    public interface ILayoutViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<IFunctoidChannelViewModel> FunctoidChannels { get; set; }
        
        string CurrentPath { get; set; }
        void Close();
        void LoadLastConfig();
        void DeleteChannel(IFunctoidChannelViewModel channel);
    }
}