using MoonBurst.Api.Hardware;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IDeviceInputViewModelFactory : IFactory<IDeviceInputViewModel>
    {

    }

    public class DeviceInputViewModelFactory : IDeviceInputViewModelFactory
    {
        private readonly ISerialGateway _gateway;

        public DeviceInputViewModelFactory(ISerialGateway gateway)
        {
            _gateway = gateway;
        }

        public IDeviceInputViewModel Build()
        {
            return new DeviceInputViewModel(_gateway);
        }
    }
}