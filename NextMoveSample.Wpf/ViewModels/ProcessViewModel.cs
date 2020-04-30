using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accessibility;
using Caliburn.Micro;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ProcessViewModel: PropertyChangedBase
    {
        private string name;
        private string id;

        public ProcessViewModel(string id)
        {
            Id = id;
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
                SetProcessType();
                SetProcessName();
                NotifyOfPropertyChange(() => Id);
                
            }
        }

        public ProcessType ProcessType { get; private set; }

        private void SetProcessType()
        {
            if (Id.Contains("Arkivmelding"))
            {
                ProcessType = ProcessType.DPO;
            }
            else if (Id.Contains("avtalt"))
            {
                ProcessType = ProcessType.DPA;
            }
            else if (Id.Contains("digitalpost"))
            {
                if (Id.Contains("vedtak"))
                {
                    ProcessType = ProcessType.DPI_VEDTAK;
                }
                else
                {
                    ProcessType = ProcessType.DPI_INFO;
                }
            }
        }

        private void SetProcessName()
        {
            var idvalue = Id.Split(':');
            Name = $"{ idvalue[4].FirstCharToUpper()} - {idvalue[5].FirstCharToUpper()}";
        }
    }

    public enum ProcessType
    {
        DPO,
        DPA,
        DPI_VEDTAK,
        DPI_INFO
    }
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
    }
}
