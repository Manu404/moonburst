using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using MoonBurst.Core.Serializer;
using MoonBurst.Helper;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class LayoutViewModel : ViewModelBase, ILayoutViewModel
    {
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

        public LayoutViewModel(IClientConfigurationViewModel clientConfiguration,
            ISerializer<ILayoutViewModel> serializer,
            IFunctoidChannelViewModelFactory channelFactory)
        {
            _config = clientConfiguration;
            _serializer = serializer;
            _channelFactory = channelFactory;

            FunctoidChannels = new ObservableCollection<IFunctoidChannelViewModel>();
            FunctoidChannels.CollectionChanged += (o,e) => UpdateIndexes();
            CurrentPath = string.Empty;

            OnAddChannelCommand = new RelayCommand(AddChannel);
            OnSaveLayoutCommand = new RelayCommand(OnSaveLayout);
            OnLoadLayoutCommand = new RelayCommand(OnLoadLayout);
            OnSaveAsLayoutCommand = new RelayCommand(OnSaveAsLayout);
            OnCollaspeAllCommand = new RelayCommand(() => ToggleChannelAll(false));
            OnExpandAllCommand = new RelayCommand(() => ToggleChannelAll(true));
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < FunctoidChannels.Count; FunctoidChannels[i].Index = (i++) + 1)
            {
            }
        }

        private void ToggleChannelAll(bool newState)
        {
            foreach (var functoidChannel in FunctoidChannels)
            {
                functoidChannel.IsExpanded = newState;
            }
        }

        public void AddChannel()
        {
            FunctoidChannels.Add(_channelFactory.Build());
        }

        public void DeleteChannel(IFunctoidChannelViewModel channel)
        {
            FunctoidChannels.Remove(channel);
        }

        private void OnSaveAsLayout()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = FileAssociationsHelper.LayoutFilter;
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
            openFileDialog1.Filter = FileAssociationsHelper.LayoutFilter;
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