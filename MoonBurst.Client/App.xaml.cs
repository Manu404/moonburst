using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Parser;
using MoonBurst.Model;
using MoonBurst.Model.Parser;
using MoonBurst.ViewModel;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FileAssociationsHelper.EnsureFileAssociation();

            var container = new WindsorContainer();

            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());

            container.Register(Classes.FromThisAssembly().BasedOn(typeof(ISerializer<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(ISerializer<>)).WithServiceAllInterfaces());

            container.Register(Component.For<IFootswitchParser>().ImplementedBy<MomentaryFootswitchParser>());
            container.Register(Component.For<IControllerParser>().ImplementedBy<Fs3XParser>());
            container.Register(Component.For<IMusicalNoteHelper>().ImplementedBy<MusicalNoteHelper>());

            container.Register(Component.For<IMidiGateway>().ImplementedBy<MidiGateway>());
            container.Register(Component.For<ISerialGateway>().ImplementedBy<SerialGateway>());
            container.Register(Component.For<IArduinoGateway>().ImplementedBy<ArduinoGateway>());

            container.Register(Classes.FromThisAssembly().BasedOn(typeof(ViewModelBase)).WithServiceAllInterfaces());
                        
            base.OnStartup(e);

            MainWindow mw = new MainWindow(container.Resolve<IMainViewModel>());
            mw.Show();
        }

    }
}
