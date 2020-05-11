using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using NextMove.Lib;
using NextMoveSample.Wpf.ValidationHandler.Attributes;


namespace NextMoveSample.Wpf.ViewModels
{
    public class MessageViewModel : ValidationViewModel
    {
        private readonly NextMoveClient nextMoveClient;
        private readonly IEventAggregator eventAggregator;
        private ParticipantViewModel sender;
        private ParticipantViewModel receiver;
        private ProcessViewModel selectedProcess;
        private string conversationId;
        private string messageId;
        private ObservableCollection<FileInfo> payloadInfo;
        private DocumentViewModel selectedDocument;


        public MessageViewModel(NextMoveClient nextMoveClient, IEventAggregator eventAggregator):base(nameof(MessageViewModel))
        {
            this.nextMoveClient = nextMoveClient;
            this.eventAggregator = eventAggregator;
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

        [Required(ErrorMessage = "h")]
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

        [Required (ErrorMessage = "MessageId is required")]
        [GuidValidation]
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
                SetDocument();
                eventAggregator.PublishOnUIThreadAsync(new ProcessMessage(selectedProcess.Id));
            }
        }

        private void SetDocument()
        {
            switch (SelectedProcess.ProcessType)
            {
                case ProcessType.DPO:
                    SelectedDocument = new DocumentViewModel
                        {Id = "urn:no:difi:arkivmelding:xsd::arkivmelding", Name = "Arkivmelding"};
                    break;
                case ProcessType.DPA:
                    SelectedDocument = new DocumentViewModel {Id = "urn:no:difi:avtalt:xsd::avtalt", Name = "Avtalt"};
                    break;
                case ProcessType.DPI_INFO:
                    break;
                case ProcessType.DPI_VEDTAK:
                    break;
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

        //public bool IsValid => Sender.IsValid && Receiver.IsValid && SelectedProcess != null && HasPayload();

         

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
    }
}
