using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class LayoutViewModel : ViewModelBase, IFileSerializableType<LayoutViewModel.LayoutData>
    {
        public ObservableCollection<FunctoidChannelViewModel> FunctoidChannels { get; set; }

        public ICommand OnAddChannelCommand { get; set; }
        public ICommand OnSaveLayoutCommand { get; set; }
        public ICommand OnSaveAsLayoutCommand { get; set; }
        public ICommand OnLoadLayoutCommand { get; set; }
        public ICommand OnCollaspeAllCommand { get; set; }
        public ICommand OnExpandAllCommand { get; set; }

        public LayoutViewModel()
        {
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

        public LayoutViewModel(LayoutData data, string path) : this()
        {
            this.FunctoidChannels = new ObservableCollection<FunctoidChannelViewModel>(data.Channels.ConvertAll(d => new FunctoidChannelViewModel(d)));
            this.Path = path;
        }

        public void AddChannel()
        {
            this.FunctoidChannels.Add(new FunctoidChannelViewModel());
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

        public void Load(LayoutViewModel data)
        {
            this.FunctoidChannels.Clear();
            data.FunctoidChannels.ToList().ForEach(this.FunctoidChannels.Add);
            this.Path = data.Path;
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
                DataSerializer<LayoutViewModel, LayoutData>.SaveToFile(this);
            }
        }

        private void OnLoadLayout()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst layout|*.mblayout";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                Load(DataSerializer<LayoutViewModel, LayoutData>.LoadFromFile(openFileDialog1.FileName));
            }
        }

        private void OnSaveLayout()
        {
            if (String.IsNullOrEmpty(this.Path))
                OnSaveAsLayout();
            else
                DataSerializer<LayoutViewModel, LayoutData>.SaveToFile(this);
        }

        public string Path { get; set; }
        public string Default { get => "default_layout.xml"; }
        public LayoutData GetData()
        {
            return new LayoutData()
            {
                Channels = this.FunctoidChannels.ToList().ConvertAll(f => f.GetData())
            };
        }

        public IFileSerializableType<LayoutData> CreateFromData(LayoutData data, string path)
        {
            return new LayoutViewModel(data, path);
        }

        public class LayoutData : IFileSerializableData
        {
            public List<FunctoidChannelViewModel.FunctoidChannelData> Channels { get; set; }

            public LayoutData()
            {
                Channels = new List<FunctoidChannelViewModel.FunctoidChannelData>();
            }
        }
    }
}