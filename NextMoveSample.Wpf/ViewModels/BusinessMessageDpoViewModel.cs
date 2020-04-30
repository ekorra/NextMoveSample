using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class BusinessMessageDpoViewModel: BusinessMessageViewModel
    {
        public BusinessMessageDpoViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            
        }

        public override BusinessMessageCore GetBusinessMessage()
        {
            return new DpoBusinessMessage
            {
                PrimaryDocumentName = "arkivmelding.xml"
            };
        }
    }
}
