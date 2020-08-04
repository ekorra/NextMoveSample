using Accessibility;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ReceivedMessagesViewModel : PropertyChangedBase
    {
        private readonly StandardBusinessDocument standardBusinessDocument;
        private readonly string filePath;
        private readonly INextMoveClient nextMoveClient;
        private readonly IEventAggregator eventAggregator;
        private MessageViewModel messageViewModel;
        private BusinessMessageViewModel businessMessageViewModel;

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

        public BusinessMessageViewModel BusinessMessageViewModel
        {
            get => businessMessageViewModel;
            set
            {
                if (Equals(value, businessMessageViewModel)) return;
                businessMessageViewModel = value;
                NotifyOfPropertyChange(() => BusinessMessageViewModel);
            }
        }

        public ReceivedMessagesViewModel(StandardBusinessDocument standardBusinessDocument, string filePath, INextMoveClient nextMoveClient, IEventAggregator eventAggregator)
        {
            this.standardBusinessDocument = standardBusinessDocument;
            this.filePath = filePath;
            this.nextMoveClient = nextMoveClient;
            this.eventAggregator = eventAggregator;
            this.MessageViewModel = new MessageViewModel(standardBusinessDocument.StandardBusinessDocumentHeader, this.nextMoveClient , this.eventAggregator);
            switch (standardBusinessDocument.BusinessMessageCore)
            {
                case DpaBusinessMessage businessMessage1:
                    BusinessMessageViewModel = new BusinessMessageDpaViewModel(businessMessage1, eventAggregator);
                    break;
                case DpoBusinessMessage businessMessage2:
                    BusinessMessageViewModel = new BusinessMessageDpoViewModel(businessMessage2, eventAggregator);
                    break;
            }
        }
    }
}