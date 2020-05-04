using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Vst
{
    public class MainViewHostFactory : IMainViewHostFactory
    {
        private readonly IMainViewModel mainViewModel;
        private readonly IFactory<IMainView> viewFactory;

        public MainViewHostFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new VstMainViewHost(mainViewModel, viewFactory);
        }
    }
}