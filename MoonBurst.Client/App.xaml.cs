using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Parser;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Hardware.Parser;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Helper;
using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interfaces;
using MoonBurst.Views;

namespace MoonBurst
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FileAssociationsHelper.EnsureFileAssociation();

            AssemblyFilter filter = new AssemblyFilter(".", "MoonBurst.*");

            var container = new WindsorContainer();

            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IViewModel)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IHardwareService)).WithServiceAllInterfaces());

            container.Register(Component.For<IFootswitchParser>().ImplementedBy<MomentaryFootswitchParser>());
            container.Register(Component.For<IControllerParser>().ImplementedBy<Fs3XParser>());
            container.Register(Component.For<IMusicalNoteHelper>().ImplementedBy<MusicalNoteHelper>());
            container.Register(Component.For<IDynamicsHelper>().ImplementedBy<DynamicsHelper>());

            base.OnStartup(e);

            MainWindow mw = new MainWindow(container.Resolve<IMainViewModel>());
            mw.Show();
        }

    }
}
