using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ShellViewModel: PropertyChangedBase
    {
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

        public BindableCollection<int> SecurityLevels { get; set; }


        public bool CanSend => ((!string.IsNullOrEmpty(MessageViewModel.Sender.Id)) && (!string.IsNullOrEmpty(MessageViewModel.Receiver.Id)));

        public async Task Send()
        {
            SetWorkingState(true);
            try
            {
                await nextMoveClient.SendMessage(MessageViewModel.GetEnvelopeInfo(), MessageViewModel.GetBusinessMessage(),
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
            InitData();
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

        public ShellViewModel()
        {
            nextMoveClient = new NextMoveClient(new HttpClient());
            InitData();
            SetWorkingState(false);
        }

        private void InitData()
        {
            MessageViewModel = new MessageViewModel(nextMoveClient);
            MessageViewModel.Sender.Id = "910075918";
            MessageViewModel.Receiver.Id = "910075918";
            SecurityLevels = new BindableCollection<int>{3,4};
        }

        

        public void AddFiles(FileInfo file)
        {
            MessageViewModel.PayloadInfo.Add(file);
        }
    }
}
