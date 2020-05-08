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

        public static IEnumerable<Type> GetDefaultTypes()
        {
            return new List<Type>
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
        }

        public static IEnumerable<Type> GetPluginTypes()
        {
            return new List<Type>
            {
                typeof(IDeviceDefinition),
                typeof(IDeviceParser)
            };
        }

        public static IWindsorContainer RegisterStatic(this IWindsorContainer container)
        {
            //container.Register(Component.For<IMessenger>().ImplementedBy<Messenger>());
            return container;
        }

        public static IWindsorContainer RegisterTypesFrom(this IWindsorContainer container, FromAssemblyDescriptor sourceAssembly, IEnumerable<Type> typesToRegister)
        {
            foreach (var type in typesToRegister)
                container.Register(sourceAssembly.BasedOn(type).WithServiceAllInterfaces());
            return container;
        }

        public static IWindsorContainer RegisterPlugins(this IWindsorContainer container, string mask = "MoonBurst.*")
        {
            AssemblyFilter filter = new AssemblyFilter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), mask);
            return container.RegisterTypesFrom(Classes.FromAssemblyInDirectory(filter), GetPluginTypes());
        }

        public static IWindsorContainer RegisterDefaultTypesFrom(this IWindsorContainer container, FromAssemblyDescriptor sourceAssembly)
        {
            return container.RegisterTypesFrom(sourceAssembly, GetDefaultTypes());
        }

        public static IWindsorContainer GetDefaultContainer()
        {
            return CreateDefaultContainer()
                .RegisterDefaultTypesFrom(Classes.FromAssemblyInThisApplication())
                .RegisterStatic()
                .RegisterPlugins();
        }
    }
}