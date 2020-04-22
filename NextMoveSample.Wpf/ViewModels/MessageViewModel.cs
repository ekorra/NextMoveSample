using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using NextMove.Lib;


namespace NextMoveSample.Wpf.ViewModels
{
    public class MessageViewModel : PropertyChangedBase
    {
        private readonly NextMoveClient nextMoveClient;
        private ParticipantViewModel sender;
        private ParticipantViewModel receiver;
        private int? selectedSecurityLevel;
        private ProcessViewModel selectedProcess;
        private string conversationId;
        private string messageId;
        private ObservableCollection<FileInfo> payloadInfo;
        private DocumentViewModel selectedDocument;


        public MessageViewModel(NextMoveClient nextMoveClient)
        {
            this.nextMoveClient = nextMoveClient;
            ConversationId = Guid.NewGuid().ToString();
            Sender = new ParticipantViewModel(false, nextMoveClient);
            Receiver = new ParticipantViewModel(true, nextMoveClient);
            MessageId = Guid.NewGuid().ToString();
            PayloadInfo = new ObservableCollection<FileInfo>();
            SelectedDocument = new DocumentViewModel
                {Id = @"urn:no:difi:arkivmelding:xsd::arkivmelding", Name = "Arkivmelding"};

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

        public DocumentViewModel SelectedDocument
        {
            get => selectedDocument;
            set
            {
                if (Equals(value, selectedDocument)) return;
                selectedDocument = value;
                NotifyOfPropertyChange(() => SelectedDocument);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

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
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        private bool HasPayload()
        {
            return PayloadInfo != null && PayloadInfo.Any();
        }

        public bool IsValid => Sender.IsValid && Receiver.IsValid && SelectedSecurityLevel != null && SelectedProcess != null && HasPayload();

        public EnvelopeInfo GetEnvelopeInfo()
        {
            // "urn:no:difi:arkivmelding:xsd::arkivmelding"
            var envelope = new EnvelopeInfo(Sender.Id, Receiver.Id, selectedProcess.Id, SelectedDocument.Id)
            {
                ConversationId = this.ConversationId,
                MessageId = this.MessageId
            };
            return envelope;
        }

        public BusinessMessageCore GetBusinessMessage()
        {
            return new DpoBusinessMessage
            {
                PrimaryDocumentName = "arkivmelding.xml",
                SecurityLevel = SelectedSecurityLevel.Value
            };
        }
    }
}
