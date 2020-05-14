using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class BusinessMessageDpaViewModel: BusinessMessageViewModel
    {
        private string identifier;
        private string content;

        public BusinessMessageDpaViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            
        }

        public BusinessMessageDpaViewModel(DpaBusinessMessage dpaBusiness, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            PrimaryDocument = dpaBusiness.PrimaryDocumentName;
            Identifier = dpaBusiness.identifier;
            SelectedSecurityLevel = dpaBusiness.SecurityLevel;
            Content = dpaBusiness.content;
        }

        public override BusinessMessageCore GetBusinessMessage()
        {
            return new DpaBusinessMessage()
            {
                PrimaryDocumentName = PrimaryDocument,
                identifier = Identifier,
                content = Content,
                SecurityLevel = SelectedSecurityLevel ?? 3
            };
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
