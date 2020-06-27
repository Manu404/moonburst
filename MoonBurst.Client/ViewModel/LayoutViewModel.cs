using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MoonBurst.Api.Serializer;
using MoonBurst.Core.Helper;
using MoonBurst.Helper;
using MoonBurst.ViewModel.Factory;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class LayoutViewModel : ViewModelBase, ILayoutViewModel
    {
        private readonly IApplicationConfigurationViewModel _config;
        private readonly ISerializer<ILayoutViewModel> _serializer;
        private readonly IFunctoidChannelViewModelFactory _channelFactory;
        private readonly IFileDialogProvider _dialogProvider;

        public ObservableCollection<ILayoutChannelViewModel> FunctoidChannels { get; set; }

        public string CurrentPath { get; set; }

        public ICommand OnAddChannelCommand { get; set; }
        public ICommand OnNewLayoutCommand { get; set; }
        public ICommand OnSaveLayoutCommand { get; set; }
        public ICommand OnSaveAsLayoutCommand { get; set; }
        public ICommand OnLoadLayoutCommand { get; set; }
        public ICommand OnCollapseAllCommand { get; set; }
        public ICommand OnExpandAllCommand { get; set; }

        public LayoutViewModel(IApplicationConfigurationViewModel applicationConfiguration,
            ISerializer<ILayoutViewModel> serializer,
            IFunctoidChannelViewModelFactory channelFactory)
        {
            _config = applicationConfiguration;
            _serializer = serializer;
            _channelFactory = channelFactory;

            _dialogProvider = new FileDialogProvider();

            FunctoidChannels = new ObservableCollection<ILayoutChannelViewModel>();
            FunctoidChannels.CollectionChanged += (o,e) => UpdateIndexes();
            CurrentPath = string.Empty;

            OnNewLayoutCommand = new RelayCommand(CreateNewLayoutAsync);
            OnAddChannelCommand = new RelayCommand(AddChannel);
            OnSaveLayoutCommand = new RelayCommand(OnSaveLayout);
            OnLoadLayoutCommand = new RelayCommand(OnLoadLayout);
            OnSaveAsLayoutCommand = new RelayCommand(OnSaveAsLayout);
            OnCollapseAllCommand = new RelayCommand(() => ToggleChannelAll(false));
            OnExpandAllCommand = new RelayCommand(() => ToggleChannelAll(true));
        }

        private async void CreateNewLayoutAsync()
        {
            var result = await ConfirmationHelper.RequestConfirmationBeforeCreateNew();
            if (result is bool boolResult && boolResult)
            {
                CurrentPath = string.Empty;
                FunctoidChannels.Clear();
            }
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < FunctoidChannels.Count; FunctoidChannels[i].Index = (i++) + 1);
        }

        private void RegisterEvents()
        {
            foreach (var channel in FunctoidChannels)
            {
                channel.DeleteRequested += OnDeleteRequested;
            }
        }

        private void OnDeleteRequested(object sender, EventArgs e)
        {
            if (sender is ILayoutChannelViewModel channel)
            {
                channel.DeleteRequested -= OnDeleteRequested;
                FunctoidChannels.Remove(channel);
            }
        }

        private void ToggleChannelAll(bool newState)
        {
            foreach (var channel in FunctoidChannels)
            {
                channel.IsExpanded = newState;
            }
        }

        public void AddChannel()
        {
            var newChannel = _channelFactory.Build();
            newChannel.DeleteRequested += OnDeleteRequested;
            FunctoidChannels.Add(newChannel);
        }

        public void Close()
        {
            OnSaveLayoutCommand.Execute(null);
        }

        private void OnSaveAsLayout()
        {
            _dialogProvider.ShowSaveDialog("Save layout", FileAssociationsHelper.LayoutFilter, Save);
        }

        private void OnLoadLayout()
        {
            _dialogProvider.ShowLoadDialog("Load layout", FileAssociationsHelper.LayoutFilter, Load);
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
            RegisterEvents();
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