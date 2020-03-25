using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Parser;
using MoonBurst.Api.Gateways;
using MoonBurst.Api.Hardware;
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

            var kernel = container.Kernel;
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel));
            
            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<,>)).WithServiceAllInterfaces());
            
            //container.Register(Component.For<IFootswitchParser>().ImplementedBy<MomentaryFootswitchParser>());
            container.Register(Component.For<IMusicalNoteHelper>().ImplementedBy<MusicalNoteHelper>());
            container.Register(Component.For<IDynamicsHelper>().ImplementedBy<DynamicsHelper>());
            
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IGateway)).WithServiceAllInterfaces());
            
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDeviceDefinition)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDeviceParser)).WithServiceAllInterfaces());
            
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IViewModel)).WithServiceAllInterfaces());

            base.OnStartup(e);

            MainWindow mw = new MainWindow(container.Resolve<IMainViewModel>());
            mw.Show();
        }

    }
}
