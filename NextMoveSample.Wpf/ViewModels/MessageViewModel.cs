using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Caliburn.Micro;


namespace NextMoveSample.Wpf.ViewModels
{
    public class MessageViewModel : PropertyChangedBase
    {
        private ParticipantViewModel sender;
        private ParticipantViewModel receiver;
        private int? selectedSecurityLevel;
        private ProcessViewModel selectedProcess;
        private string conversationId;
        private string messageId;
        private ObservableCollection<FileInfo> payloadInfo;
        

        public MessageViewModel()
        {
            ConversationId = Guid.NewGuid().ToString();
            MessageId = Guid.NewGuid().ToString();
            PayloadInfo = new ObservableCollection<FileInfo>();

        }

        public ParticipantViewModel Sender
        {
            get => sender;
            set
            {
                if (Equals(value, sender)) return;
                sender = value;
                NotifyOfPropertyChange(() => Sender);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public ParticipantViewModel Receiver
        {
            get => receiver;
            set
            {
                if (Equals(value, receiver)) return;
                receiver = value;
                NotifyOfPropertyChange(() => Receiver);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public string ConversationId
        {
            get => conversationId;
            set
            {
                if (value == conversationId) return;
                conversationId = value;
                NotifyOfPropertyChange(() => ConversationId);
            }
        }

        public string MessageId
        {
            get => messageId;
            set
            {
                if (value == messageId) return;
                messageId = value;
                NotifyOfPropertyChange(() => MessageId);
            }
        }

        public ProcessViewModel SelectedProcess
        {
            get => selectedProcess;
            set
            {
                if (Equals(value, selectedProcess)) return;
                selectedProcess = value;
                NotifyOfPropertyChange(() => SelectedProcess);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public DocumentViewModel SelectedDocument { get; set; }

        public int? SelectedSecurityLevel
        {
            get => selectedSecurityLevel;
            set
            {
                if (value == selectedSecurityLevel) return;
                selectedSecurityLevel = value;
                NotifyOfPropertyChange(() => SelectedSecurityLevel);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public ObservableCollection<FileInfo> PayloadInfo
        {
            get => payloadInfo;
            set
            {
                if (Equals(value, payloadInfo)) return;
                payloadInfo = value;
                NotifyOfPropertyChange(() => PayloadInfo);
            }
        }

        public bool IsValid
        {
            get { return Sender.IsValid && Receiver.IsValid && SelectedSecurityLevel != null && SelectedProcess != null ; }
        }


    }
}
