using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MoonBurst.Api.Serializer;
using MoonBurst.Core.Helper;
using MoonBurst.ViewModel.Factory;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class LayoutViewModel : ViewModelBase, ILayoutViewModel
    {
        private readonly IApplicationConfigurationViewModel _config;
        private readonly ISerializer<ILayoutViewModel> _serializer;
        private readonly IFunctoidChannelViewModelFactory _channelFactory;
        private readonly ILoadSaveDialogProvider _dialogProvider;

        public ObservableCollection<ILayoutChannelViewModel> FunctoidChannels { get; set; }

        public string CurrentPath { get; set; }

        public ICommand OnAddChannelCommand { get; set; }
        public ICommand OnSaveLayoutCommand { get; set; }
        public ICommand OnSaveAsLayoutCommand { get; set; }
        public ICommand OnLoadLayoutCommand { get; set; }
        public ICommand OnCollaspeAllCommand { get; set; }
        public ICommand OnExpandAllCommand { get; set; }

        public LayoutViewModel(IApplicationConfigurationViewModel applicationConfiguration,
            ISerializer<ILayoutViewModel> serializer,
            IFunctoidChannelViewModelFactory channelFactory)
        {
            _config = applicationConfiguration;
            _serializer = serializer;
            _channelFactory = channelFactory;

            _dialogProvider = new LoadSaveDialogProvider();

            FunctoidChannels = new ObservableCollection<ILayoutChannelViewModel>();
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

        public void DeleteChannel(ILayoutChannelViewModel channel)
        {
            FunctoidChannels.Remove(channel);
        }

        public void Close()
        {
            OnSaveLayoutCommand.Execute(null);
        }

        private void OnSaveAsLayout()
        {
            Save(_dialogProvider.ShowSaveDialog("Save layout", FileAssociationsHelper.LayoutFilter));
        }

        private void OnLoadLayout()
        {
            Load(_dialogProvider.ShowSaveDialog("Load layout", FileAssociationsHelper.LayoutFilter));
        }

        private void OnSaveLayout()
        {
            if (String.IsNullOrEmpty(this.CurrentPath))
                OnSaveAsLayout();
            else
                Save();
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

        public void Load(string path)
        {
            _serializer.Load(path, this);
            _config.LastLayoutPath = path;
        }

        public void SaveLast()
        {
            Save(_config.LastLayoutPath);
        }

        public void LoadLast()
        {
            Load(_config.LastLayoutPath);
        }
    }
}