using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MoonBurst.Core;

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
