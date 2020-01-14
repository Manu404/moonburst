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
    public class LayoutViewModel : ViewModelBase
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;

        public ObservableCollection<FunctoidChannelViewModel> FunctoidChannels { get; set; }

        public ICommand OnAddChannelCommand { get; set; }
        public ICommand OnSaveLayoutCommand { get; set; }
        public ICommand OnSaveAsLayoutCommand { get; set; }
        public ICommand OnLoadLayoutCommand { get; set; }
        public ICommand OnCollaspeAllCommand { get; set; }
        public ICommand OnExpandAllCommand { get; set; }

        public LayoutViewModel(IArduinoGateway arduinoGateway, IMessenger messenger)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;

            FunctoidChannels = new ObservableCollection<FunctoidChannelViewModel>();
            Path = string.Empty;

            OnAddChannelCommand = new RelayCommand(() => AddChannel());
            OnSaveLayoutCommand = new RelayCommand(OnSaveLayout);
            OnLoadLayoutCommand = new RelayCommand(OnLoadLayout);
            OnSaveAsLayoutCommand = new RelayCommand(OnSaveAsLayout);
            OnCollaspeAllCommand = new RelayCommand(() => ToggleChannelAll(false));
            OnExpandAllCommand = new RelayCommand(() => ToggleChannelAll(true));
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
            this.FunctoidChannels.Add(new FunctoidChannelViewModel(_messenger));
            UpdateChannelIndexes();
        }

        public void DeleteChannel(FunctoidChannelViewModel channel)
        {
            this.FunctoidChannels.Remove(channel);
            UpdateChannelIndexes();
        }

        public void UpdateChannelIndexes()
        {
            int i = 1;
            foreach (var functoidChannel in this.FunctoidChannels)
            {
                functoidChannel.Index = i;
                i++;
            }
        }

        private void OnSaveAsLayout()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Moonburst layout|*.mblayout";
            saveFileDialog1.Title = "Save layout";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Path = saveFileDialog1.FileName;
                DataSerializer<LayoutData>.SaveToFile(GetData());
            }
        }

        private void OnLoadLayout()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst layout|*.mblayout";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                LoadData(DataSerializer<LayoutData>.LoadFromFile(openFileDialog1.FileName));
            }
        }

        private void OnSaveLayout()
        {
            if (String.IsNullOrEmpty(this.Path))
                OnSaveAsLayout();
            else
                DataSerializer<LayoutData>.SaveToFile(GetData());
        }

        public string Path { get; set; }

        public LayoutData GetData()
        {
            return new LayoutData()
            {
                Path = this.Path,
                Channels = this.FunctoidChannels.ToList().ConvertAll(f => f.GetData())
            };
        }

        public void LoadData(LayoutData data)
        {
            this.FunctoidChannels.Clear();
            data.Channels.ConvertAll(d => new FunctoidChannelViewModel(d, this._arduinoGateway, _messenger)).ForEach(this.FunctoidChannels.Add);
            FunctoidChannels.ToList().ForEach(d => d.RefreshInputs());
            this.Path = data.Path;
        }

        public class LayoutData : IFileSerializableType
        {
            [XmlIgnore]
            public string Path { get; set; }
            [XmlIgnore]
            public string Default => "default_layout.xml";

            public List<FunctoidChannelViewModel.FunctoidChannelData> Channels { get; set; }

            public LayoutData()
            {
                Channels = new List<FunctoidChannelViewModel.FunctoidChannelData>();
            }
        }
    }
}