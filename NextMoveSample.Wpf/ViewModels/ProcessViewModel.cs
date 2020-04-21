using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ProcessViewModel: PropertyChangedBase
    {
        private string name;
        private string id;

        public ProcessViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Id
        {
            get => id;
            set
            {
                if (value == id) return;
                id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }
    }
}
