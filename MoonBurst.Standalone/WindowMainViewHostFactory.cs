using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    public class WindowMainViewHostFactory : IMainViewHostFactory
    {
        private readonly IMainViewModel mainViewModel;
        private readonly IFactory<IMainView> viewFactory;

        public WindowMainViewHostFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new WindowMainViewHost(mainViewModel, viewFactory);
        }
    }
}