using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Caliburn.Micro;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ShellViewModel: PropertyChangedBase
    {
        //private string sender;
        private string receiver;

        private MessageViewModel messageViewModel;
        private ProcessViewModel selectedProcess;
        private int selectedSecurityLevel;

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

        public BindableCollection<ProcessViewModel> Processes { get; set; }
       

        public ProcessViewModel SelectedProcess
        {
            get => selectedProcess;
            set
            {
                if (Equals(value, selectedProcess)) return;
                selectedProcess = value;
                NotifyOfPropertyChange(() => SelectedProcess);
            }
        }

        public BindableCollection<int> SecurityLevels { get; set; }

        public int SelectedSecurityLevel
        {
            get => selectedSecurityLevel;
            set
            {
                if (value == selectedSecurityLevel) return;
                selectedSecurityLevel = value;
                NotifyOfPropertyChange(() => SelectedSecurityLevel);
            }
        }


       
        public bool CanSend
        {
            get { return ((!string.IsNullOrEmpty(MessageViewModel.Sender.Id)) && (!string.IsNullOrEmpty(MessageViewModel.Receiver.Id))); }
        }

        public ShellViewModel()
        {
            InitTestData();
        }

        private void InitTestData()
        {
            SecurityLevels = new BindableCollection<int>() {3, 4};
            MessageViewModel = new MessageViewModel
            {
                Sender = new ParticipantViewModel(false), 
                Receiver = new ParticipantViewModel(true)
            };
        }

        

        public void AddFiles(FileInfo file)
        {
            MessageViewModel.PayloadInfo.Add(file);
        }
    }
}
