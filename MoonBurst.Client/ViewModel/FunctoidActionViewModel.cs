using System.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class TriggeredActionMessage : MessageBase
    {
        public FunctoidActionViewModel.FunctoidActionData Data { get; set; }
    }

    public class FunctoidActionViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private bool _isExpanded;
        private string _actionName;
        private FootTrigger _trigger;
        private ChannelCommand _command;
        private int _data2;
        private int _data1;
        private int _midiChannel;
        private int _delay;

        public int MidiChannel
        {
            get => _midiChannel;
            set
            {
                _midiChannel = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public int Data1
        {
            get => _data1;
            set
            {
                _data1 = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public int Data2
        {
            get => _data2;
            set
            {
                _data2 = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public ChannelCommand Command
        {
            get => _command;
            set
            {
                _command = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public FootTrigger Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
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

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                RaisePropertyChanged();
            }
        }

        public string ActionName
        {
            get => $"On {Trigger} \n{Command}({Data1},{Data2}) Channel({MidiChannel})";
        }

        public ICommand OnDeleteActionCommand { get; set; }
        public ICommand OnTestActionCommand { get; set; }

        public FunctoidActionViewModel()
        {
            OnDeleteActionCommand = new RelayCommand(OnDelete);
            OnTestActionCommand = new RelayCommand(OnTestAction);
        }

        private void OnTestAction()
        {
            Messenger.Default.Send(new TriggeredActionMessage() { Data = this.GetData() });
        }

        private void OnDelete()
        {
            MessengerInstance.Send(new DeleteFunctoidActionMessage(this));
        }

        public FunctoidActionViewModel(FunctoidActionData data) : this()
        {
            this.MidiChannel = data.MidiChannel;
            this.Data2 = data.Data2;
            this.Data1 = data.Data1;
            this.Command = data.Command;
            this.Trigger = data.Trigger;
            this.IsEnabled = data.IsEnabled;
            this.IsExpanded = data.IsExpanded;
        }

        public FunctoidActionData GetData()
        {
            return new FunctoidActionData()
            {
                MidiChannel = this.MidiChannel,
                Data1 = this.Data1,
                Data2 = this.Data2,
                Command = this.Command,
                Trigger = this.Trigger,
                IsEnabled = this.IsEnabled,
                IsExpanded = this.IsExpanded
            };
        }

        public class FunctoidActionData
        {
            public int MidiChannel { get; set; }
            public int Data1 { get; set; }
            public int Data2 { get; set; }
            public ChannelCommand Command { get; set; }
            public FootTrigger Trigger { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsExpanded { get; set; }
        }
    }
}