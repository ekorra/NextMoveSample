using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ShellViewModel : Conductor<BusinessMessageViewModel>.Collection.OneActive
    {
        private readonly IEventAggregator eventAggregator;
        private MessageViewModel messageViewModel;
        private readonly NextMoveClient nextMoveClient;
        private bool isEnabled;

        public MessageViewModel MessageViewModel
        {
            get => messageViewModel;
            set
            {
                if (Equals(value, messageViewModel)) return;
                messageViewModel = value;
                NotifyOfPropertyChange(() => MessageViewModel);

                
            }
        }

        public ShellViewModel()
        {
            this.eventAggregator = new EventAggregator();
            nextMoveClient = new NextMoveClient(new HttpClient());
            InitContext();
        }

        private void InitContext()
        {
            SetWorkingState(true);
            MessageViewModel = new MessageViewModel(nextMoveClient, eventAggregator);
            messageViewModel.PropertyChanged += MessageViewModelOnPropertyChanged;
            InitData();
            SetWorkingState(false);
        }

        private void MessageViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(messageViewModel.SelectedProcess))
            {
                switch (messageViewModel.SelectedProcess.ProcessType)
                {
                    case ProcessType.DPA:
                        ActivateItem(new BusinessMessageDpaViewModel(eventAggregator));
                        break;
                    case ProcessType.DPO:
                        ActivateItem(new BusinessMessageDpoViewModel(eventAggregator));
                        break;
                    case ProcessType.DPI_INFO:
                        throw new NotImplementedException();
                    case ProcessType.DPI_VEDTAK:
                        throw new NotImplementedException();
                    default:
                        ActivateItem(null);
                        break;
                }
            }
            
        }

        public bool CanSend => ((!string.IsNullOrEmpty(MessageViewModel.Sender.Id)) && (!string.IsNullOrEmpty(MessageViewModel.Receiver.Id)));

        public async Task Send()
        {
            SetWorkingState(true);
            try
            {
                await nextMoveClient.SendMessage(MessageViewModel.GetEnvelopeInfo(), ActiveItem.GetBusinessMessage(),
                    MessageViewModel.PayloadInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
            }
            SetWorkingState(false); 
        }

        private void SetWorkingState(bool isBusy)
        {
            IsEnabled = isBusy == false;
        }

        public void Reset()
        {
            InitContext();
            switch (ActiveItem)
            {
                case BusinessMessageDpaViewModel _:
                    ActiveItem = new BusinessMessageDpaViewModel(eventAggregator);
                    break;
                case BusinessMessageDpoViewModel _:
                    ActiveItem = new BusinessMessageDpoViewModel(eventAggregator);
                    break;
            }
            
            
        }

        public bool IsEnabled
        {
            get => isEnabled;
            private set
            {
                if (value == isEnabled) return;
                isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }


        private void InitData()
        {
            MessageViewModel.Sender.Id = "910075918";
            MessageViewModel.Receiver.Id = "910075918";
        }

        public void AddFiles(FileInfo file)
        {
            MessageViewModel.PayloadInfo.Add(file);
        }
    }
}
