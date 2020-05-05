using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using Caliburn.Micro;

namespace NextMoveSample.Wpf.ViewModels
{
    public class SentMessageViewModel : PropertyChangedBase
    {
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

        public SentMessageViewModel(MessageViewModel messageViewModel, BusinessMessageViewModel businessMessageViewModel)
        {
            this.messageViewModel = messageViewModel;
            this.businessMessageViewModel = businessMessageViewModel;
        }
    }
}
