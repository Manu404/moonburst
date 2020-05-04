using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api;
using MoonBurst.Api.Gateway;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Hardware.Parser;
using MoonBurst.Api.Serializer;
using MoonBurst.Core;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst
{
    public class EntryPoint
    {
        [STAThread()]
        public static void Main()
        {
            var container = new WindsorContainer();

            var kernel = container.Kernel;
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel));

            AssemblyFilter filter = new AssemblyFilter(".", "MoonBurst.*");

            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ISerializer<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IFactory<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDataExtractor<,>)).WithServiceAllInterfaces());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(INoteNameFormatter)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IHelper)).WithServiceAllInterfaces());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IGateway)).WithServiceAllInterfaces());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDeviceDefinition)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IDeviceParser)).WithServiceAllInterfaces());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(IViewModel)).WithServiceAllInterfaces());

            container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(typeof(ILauncher)).WithServiceAllInterfaces());

            container.Resolve<ILauncher>().Launch();
        }
    }
}