using MoonBurst.Core;
using MoonBurst.View;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    public class MainViewHostFactory : IMainWindowFactory
    {
        private IMainViewModel mainViewModel;
        private IFactory<IMainView> viewFactory;

        public MainViewHostFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new MainWindow(mainViewModel, viewFactory);
        }
    }
}