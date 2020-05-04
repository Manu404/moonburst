using System.Windows.Controls;
using MoonBurst.Api.Client;

namespace MoonBurst.View
{
    public class MainViewFactory : IFactory<IMainView>
    {
        public IMainView Build()
        {
            return new MainView();
        }
    } 

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}
