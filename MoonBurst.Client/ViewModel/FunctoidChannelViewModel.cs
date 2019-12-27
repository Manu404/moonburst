using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class FunctoidChannelViewModel : ViewModelBase
    {
        private int _index;
        private string _name;
        private bool _isEnabled;
        private bool _isExpanded;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                RaisePropertyChanged();
            }
        }

        public int ArduinoBitMask { get; set; }
        public ObservableCollection<FunctoidActionViewModel> Actions { get; set; }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddActionCommand { get; set; }

        public FunctoidChannelViewModel()
        {
            OnDeleteCommand = new RelayCommand(OnDelete);
            OnAddActionCommand = new RelayCommand(OnAddAction);
            Actions = new ObservableCollection<FunctoidActionViewModel>();
            MessengerInstance.Register< DeleteFunctoidActionMessage>(this, OnDeleteAction);
        }

        private void OnDeleteAction(DeleteFunctoidActionMessage obj)
        {
            Actions.Remove(obj.Item);
        }

        private void OnAddAction()
        {
            Actions.Add(new FunctoidActionViewModel());
        }

        public FunctoidChannelViewModel(FunctoidChannelData data) : this()
        {
            this.ArduinoBitMask = data.ArduinoBitMask;
            this.Index = data.Index;
            this.Name = data.Name;
            this.Actions = new ObservableCollection<FunctoidActionViewModel>(data.Actions.ConvertAll(d => new FunctoidActionViewModel(d)));
            this.IsEnabled = data.IsEnabled;
            this.IsExpanded = data.IsExpanded;
        }

        private void OnDelete()
        {
            MessengerInstance.Send(new DeleteFunctoidChannelMessage(this));
        }

        public FunctoidChannelData GetData()
        {
            return new FunctoidChannelData()
            {
                Index = this.Index,
                Name = this.Name,
                ArduinoBitMask = this.ArduinoBitMask,
                Actions = this.Actions?.ToList().ConvertAll(input => input.GetData()),
                IsEnabled = this.IsEnabled,
                IsExpanded = this.IsExpanded
            };
        }

        public class FunctoidChannelData
        {
            public int ArduinoBitMask { get; set; }
            public int Index { get; set; }
            public string Name { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsExpanded { get; set; }

            public List<FunctoidActionViewModel.FunctoidActionData> Actions { get; set; }

            public FunctoidChannelData()
            {
                Actions = new List<FunctoidActionViewModel.FunctoidActionData>();
            }
        }
    }
}