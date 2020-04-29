using System;
using System.Collections.Generic;
using System.Text;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class DpaMessageViewModel: MessageViewModel
    {
        private string primaryDocument;
        private string identifier;
        private string content;

        public DpaMessageViewModel(NextMoveClient nextMoveClient) : base(nextMoveClient)
        {
            SelectedSecurityLevel = 3;
        }

        public override BusinessMessageCore GetBusinessMessage()
        {
            return new DpaBusinessMessage{SecurityLevel = SelectedSecurityLevel.Value};

        }

        public string PrimaryDocument

        {
            get => primaryDocument;
            set
            {
                if (value == primaryDocument) return;
                primaryDocument = value;
                NotifyOfPropertyChange(() => PrimaryDocument);
            }
        }

        public string Identifier
        {
            get => identifier;
            set
            {
                if (value == identifier) return;
                identifier = value;
                NotifyOfPropertyChange(() => Identifier);
            }
        }

        public string Content
        {
            get => content;
            set
            {
                if (value == content) return;
                content = value;
                NotifyOfPropertyChange(() => Content);
            }
        }
    }
}
