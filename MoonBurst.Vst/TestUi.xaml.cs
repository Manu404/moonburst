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
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Vst
{
    /// <summary>
    /// Interaction logic for TestUi.xaml
    /// </summary>
    public partial class TestUi : IMainViewHost
    {
        private IMainView View;
        public TestUi()
        {
            InitializeComponent();
        }

        public TestUi(IMainViewModel vm, IFactory<IMainView> view)
        {
            InitializeComponent();
            View = view.Build();
            this.AddChild(View);
            this.DataContext = vm;
        }

        public void Start()
        {
            return;
        }
    }
}
