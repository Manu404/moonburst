using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Factory
{    
    public interface IArduinoConfigPortViewModelFactory : IFactory<IArduinoConfigPortViewModel, IArduinoPort>
    {

    }
    
    public class ArduinoConfigPortViewModelFactory : IArduinoConfigPortViewModelFactory
    {
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;

        public ArduinoConfigPortViewModelFactory(IFactory<IDeviceInputViewModel> deviceInputViewModelFactory)
        {
            _deviceInputViewModelFactory = deviceInputViewModelFactory;
        }

        public IArduinoConfigPortViewModel Build(IArduinoPort data)
        {
            return new ArduinoConfigPortViewModel(data, _deviceInputViewModelFactory);
        }
    }
}