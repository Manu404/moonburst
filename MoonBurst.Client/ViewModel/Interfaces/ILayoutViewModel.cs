using System.Collections.ObjectModel;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface ILayoutViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<ILayoutChannelViewModel> FunctoidChannels { get; set; }
        
        void Close();
        void LoadLast();
        void DeleteChannel(ILayoutChannelViewModel channel);
    }
}