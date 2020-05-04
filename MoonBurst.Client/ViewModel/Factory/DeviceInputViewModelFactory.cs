using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Factory
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