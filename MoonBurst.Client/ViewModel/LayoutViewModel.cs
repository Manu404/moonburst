using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MoonBurst.Core;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public partial class LayoutViewModel : ViewModelBase, ILayoutViewModel
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;
        private IClientConfigurationViewModel _config;
        private ISerializer<ILayoutViewModel> _serializer;
        private IFunctoidChannelViewModelFactory _channelFactory;

        public ObservableCollection<IFunctoidChannelViewModel> FunctoidChannels { get; set; }

        public string CurrentPath { get; set; }

        public ICommand OnAddChannelCommand { get; set; }
        public ICommand OnSaveLayoutCommand { get; set; }
        public ICommand OnSaveAsLayoutCommand { get; set; }
        public ICommand OnLoadLayoutCommand { get; set; }
        public ICommand OnCollaspeAllCommand { get; set; }
        public ICommand OnExpandAllCommand { get; set; }

        public LayoutViewModel(IArduinoGateway arduinoGateway, 
            IMessenger messenger, 
            IClientConfigurationViewModel clientConfiguration,
            ISerializer<ILayoutViewModel> serializer,
            IFunctoidChannelViewModelFactory channelFactory)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _config = clientConfiguration;
            _serializer = serializer;
            _channelFactory = channelFactory;

            FunctoidChannels = new ObservableCollection<IFunctoidChannelViewModel>();
            FunctoidChannels.CollectionChanged += (o,e) => UpdateIndexes();
            CurrentPath = string.Empty;

            OnAddChannelCommand = new RelayCommand(() => AddChannel());
            OnSaveLayoutCommand = new RelayCommand(OnSaveLayout);
            OnLoadLayoutCommand = new RelayCommand(OnLoadLayout);
            OnSaveAsLayoutCommand = new RelayCommand(OnSaveAsLayout);
            OnCollaspeAllCommand = new RelayCommand(() => ToggleChannelAll(false));
            OnExpandAllCommand = new RelayCommand(() => ToggleChannelAll(true));
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < this.FunctoidChannels.Count; this.FunctoidChannels[i].Index = (i++) + 1) ;
        }

        private void ToggleChannelAll(bool newState)
        {
            foreach (var functoidChannel in this.FunctoidChannels)
            {
                functoidChannel.IsExpanded = newState;
            }
        }

        public void AddChannel()
        {
            this.FunctoidChannels.Add(_channelFactory.Build());
        }

        public void DeleteChannel(IFunctoidChannelViewModel channel)
        {
            this.FunctoidChannels.Remove(channel);
        }

        private void OnSaveAsLayout()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Moonburst layout|*.mblayout";
            saveFileDialog1.Title = "Save layout";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Save(saveFileDialog1.FileName);
            }
        }

        private void OnLoadLayout()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst layout|*.mblayout";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                Load(openFileDialog1.FileName);
            }
        }

        private void OnSaveLayout()
        {
            if (String.IsNullOrEmpty(this.CurrentPath))
                OnSaveAsLayout();
            else
                Save();
        }

        public void Close()
        {
            OnSaveLayoutCommand.Execute(null);
        }

        public void Save()
        {
            _serializer.Save(this, CurrentPath);
            _config.LastLayoutPath = CurrentPath;
        }

        public void Save(string path)
        {
            _serializer.Save(this, path);
            _config.LastLayoutPath = path;
        }

        public void SaveLastConfig()
        {
            Save(_config.LastLayoutPath);
        }

        public void Load(string path)
        {
            _serializer.Load(path, this);
            _config.LastLayoutPath = path;
        }

        public void LoadLastConfig()
        {
            Load(_config.LastLayoutPath);
        }


    }
}