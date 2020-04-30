using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Gateways;
using MoonBurst.Core;

namespace MoonBurst.ViewModel.Factories
{    
    public interface IArduinoConfigPortViewModelFactory : IFactory<IArduinoConfigPortViewModel, IArduinoPort>
    {

    }
    
    public class ArduinoConfigPortViewModelFactory : IArduinoConfigPortViewModelFactory
    {
        private readonly ISerialGateway _gateway;
        private readonly IMessenger _messenger;
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;
        private readonly IEnumerable<IDeviceDefinition> _deviceDeviceDefinitions;

        public ArduinoConfigPortViewModelFactory(
            ISerialGateway gateway, 
            IMessenger messenger, 
            IFactory<IDeviceInputViewModel> deviceInputViewModelFactory,
            IEnumerable<IDeviceDefinition> deviceDefinitions)
        {
            _gateway = gateway;
            _messenger = messenger;
            _deviceInputViewModelFactory = deviceInputViewModelFactory;
            _deviceDeviceDefinitions = deviceDefinitions;
        }

        public IArduinoConfigPortViewModel Build(IArduinoPort data)
        {
            return new ArduinoConfigPortViewModel(data, _messenger, _deviceInputViewModelFactory);
        }
    }
}