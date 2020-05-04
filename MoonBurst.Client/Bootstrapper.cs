using System;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;
using MoonBurst.Api.Helper;
using MoonBurst.Api.Serializer;

namespace MoonBurst
{
    public class Bootstrapper
    {
        public IWindsorContainer GetDefault()
        {
            var container = new WindsorContainer();
            var kernel = container.Kernel;
            container.AddFacility<TypedFactoryFacility>();
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel));

            // Load "core"
            var typesToDiscoverFromLoadedAssembly = new List<Type>()
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
                typeof(IMainView),
                typeof(IViewModel)
            };

            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());
            foreach (var type in typesToDiscoverFromLoadedAssembly)
                container.Register(Classes.FromAssemblyInThisApplication().BasedOn(type).WithServiceAllInterfaces());

            // Load plugins
            AssemblyFilter filter = new AssemblyFilter(@"C:\git\moonburst-dev\Output\AnyCPU\Debug\Vst\", "MoonBurst.*");
            var typesToDiscoverFromFilter = new List<Type>()
            {
                typeof(IDeviceDefinition),
                typeof(IDeviceParser)
            };

            foreach (var type in typesToDiscoverFromFilter)
                container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(type).WithServiceAllInterfaces());

            return container;
        }
    }
}