using System;
using System.Collections.Generic;
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

            var typesToDiscoverFromFilter = new List<Type>()
            {
                typeof(ISerializer<,>),
                typeof(ISerializer<>),
                typeof(IFactory<>),
                typeof(IFactory<,>),
                typeof(IDataExtractor<>),
                typeof(IDataExtractor<,>),
                typeof(INoteNameFormatter),
                typeof(IHelper),
                typeof(IGateway),
                typeof(IDeviceDefinition),
                typeof(IDeviceParser),
                typeof(IViewModel),
                typeof(ILauncher),
            };

            foreach(var type in typesToDiscoverFromFilter)
                container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(type).WithServiceAllInterfaces());

            container.Resolve<ILauncher>().Launch();
        }
    }
}