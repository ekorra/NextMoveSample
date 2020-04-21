using Caliburn.Micro;

namespace NextMoveSample.Wpf.ViewModels
{
    public class DocumentViewModel : PropertyChangedBase
    {
        private string id;
        private string name;

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

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => PrettyName);
            }
        }

        public string PrettyName { get;  }
    }
}