using System.Collections.ObjectModel;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface ILayoutViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<IFunctoidChannelViewModel> FunctoidChannels { get; set; }
        
        void Close();
        void LoadLastConfig();
        void DeleteChannel(IFunctoidChannelViewModel channel);
    }
}