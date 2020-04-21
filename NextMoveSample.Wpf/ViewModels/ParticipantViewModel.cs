using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using IFormatProvider = System.IFormatProvider;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ParticipantViewModel: PropertyChangedBase
    {
        private string id;
        private string name;
        private BindableCollection<ProcessViewModel> supportedProcesses;

        public string Id
        {
            get => id;
            set
            {
                if (value == id) return;
                id = value;
                NotifyOfPropertyChange(() => Id);
                if (id.Length == 9 || id.Length == 11)
                {
                    Name = GetName();
                }

                if (IsReceiver)
                {
                    LoadProcesses();
                }
            }
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

        public bool IsReceiver { get; }

        public ParticipantViewModel(bool isReceiver)
        {
            IsReceiver = isReceiver;
            SupportedProcesses = new BindableCollection<ProcessViewModel>();
        }

        private string GetName()
        {
            return $"Testname {Id}";
        }

        private void LoadProcesses()
        {
            supportedProcesses.Clear();
            var processes =  new List<ProcessViewModel>
            {
                new ProcessViewModel( @"urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0", "Plan bygg og geodata"),
                new ProcessViewModel( @"urn:no:difi:profile:arkivmelding:helseSosialOgOmsorg:ver1.0", "Helse sosial og omsorg"),
                new ProcessViewModel( @"urn:no:difi:profile:arkivmelding:oppvekstOgUtdanning:ver1.0", "Oppvekst og utdanning")

            };
            SupportedProcesses.AddRange(processes);
        }

        public BindableCollection<ProcessViewModel> SupportedProcesses
        {
            get => supportedProcesses;
            set
            {
                if (Equals(value, supportedProcesses)) return;
                supportedProcesses = value;
                NotifyOfPropertyChange(() => SupportedProcesses);
            }
        }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Id)) return false;

                return (Id.Length == 9 || Id.Length == 11) && long.TryParse(Id, out _);
            }
        }
    }
}
