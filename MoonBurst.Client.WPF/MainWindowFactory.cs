using MoonBurst.Core;
using MoonBurst.View;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    public class MainWindowFactory : IMainWindowFactory
    {
        private IMainViewModel mainViewModel;
        private IFactory<IMainView> viewFactory;

        public MainWindowFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
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