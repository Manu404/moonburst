using System.Collections.ObjectModel;
using MoonBurst.Api.Client;
using MoonBurst.Api.Serializer;

namespace MoonBurst.ViewModel.Interface
{
    public interface ILayoutViewModel : IFileSerializableType, IViewModel
    {
        ObservableCollection<ILayoutChannelViewModel> FunctoidChannels { get; set; }
        
        void Close();
        void LoadLast();
        void DeleteChannel(ILayoutChannelViewModel channel);
    }
}