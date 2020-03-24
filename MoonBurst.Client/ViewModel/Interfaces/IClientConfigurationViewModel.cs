﻿using MoonBurst.Core;
using MoonBurst.Core.Serializer;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface IClientConfigurationViewModel : IViewModel, IFileSerializableType
    {
        string LastHardwareConfigurationPath { get; set; }
        string LastLayoutPath { get; set; }

        void LoadDefault();
        void SaveDefault();
        void Close();
    }
}