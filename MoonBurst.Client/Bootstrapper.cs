using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    public static class Bootstrapper
    {
        public static IWindsorContainer CreateDefaultContainer()
        {
            var container = new WindsorContainer();
            var kernel = container.Kernel;
            container.AddFacility<TypedFactoryFacility>(); // generate factory
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel)); // recursive resolution
            return container;
        }

        public static IWindsorContainer LoadStatic(this IWindsorContainer container)
        {
            container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());
            return container;
        }

        public static IWindsorContainer LoadPlugins(this IWindsorContainer container, FromAssemblyDescriptor sourceAssembly)
        {
            // Load plugins
            AssemblyFilter filter = new AssemblyFilter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MoonBurst.*");
            var typesToDiscoverFromFilter = new List<Type>()
            {
                typeof(IDeviceDefinition),
                typeof(IDeviceParser)
            };

            foreach (var type in typesToDiscoverFromFilter)
                container.Register(Classes.FromAssemblyInDirectory(filter).BasedOn(type).WithServiceAllInterfaces());

            return container;
        }

        public static IWindsorContainer LoadDefaultFrom(this IWindsorContainer container, FromAssemblyDescriptor sourceAssembly)
        {
            // Load default types
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

            foreach (var type in typesToDiscoverFromLoadedAssembly)
                container.Register(sourceAssembly.BasedOn(type).WithServiceAllInterfaces());

            return container;
        }

        public static IWindsorContainer GetDefaultContainer()
        {
            return CreateDefaultContainer()
                .LoadDefaultFrom(Classes.FromAssemblyInThisApplication())
                .LoadStatic()
                .LoadPlugins(Classes.FromAssemblyInThisApplication());
        }
    }
}