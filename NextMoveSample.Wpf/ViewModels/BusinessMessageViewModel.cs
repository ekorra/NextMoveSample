using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public abstract class BusinessMessageViewModel: Screen , IHandle<ProcessMessage>
    {
        private readonly IEventAggregator eventAggregator;
        private string primaryDocument;

        private int? selectedSecurityLevel;

        public string SelectedProcess { get; private set; }

        public BusinessMessageViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            SecurityLevels = new BindableCollection<int> { 3, 4 };
        }

        public BindableCollection<int> SecurityLevels { get; set; }

        public abstract BusinessMessageCore GetBusinessMessage();
        public void Handle(ProcessMessage message)
        {
            SelectedProcess = message.SelectedProcess;
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

        public virtual bool IsValid => SelectedSecurityLevel != null;
    }
}
