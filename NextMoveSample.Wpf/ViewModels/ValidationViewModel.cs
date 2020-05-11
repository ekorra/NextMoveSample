using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using NextMoveSample.Wpf.ValidationHandler.ViewModel;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ValidationViewModel: ValidationViewModelBase
    {
        private bool isValid;


        public bool IsValid
        {
            get => isValid;
            set
            {
                if (value == isValid) return;
                isValid = value;
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public string ViewModelName { get; private set; }
        public string Name
        {
            get { return ViewModelName.Substring(0, ViewModelName.IndexOf("ViewModel")); }
        }

        public ValidationViewModel(string viewModelName)
        {
            ViewModelName = viewModelName;
            OnPropertyChangedCompleted(string.Empty);
        }

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);
            OnPropertyChangedCompleted(propertyName);
        }

        protected void OnPropertyChangedCompleted(string propertyName)
        {
            // test prevent infinite loop while settings IsValid 
            // (which causes an PropertyChanged to be raised)
            if (propertyName != nameof(IsValid))
            {
                // update the isValid status
                if (string.IsNullOrEmpty(this.Error) && this.ValidPropertiesCount == this.TotalPropertiesWithValidationCount)
                {
                    this.IsValid = true;
                }
                else
                {
                    this.IsValid = false;
                }
            }
        }
    }
}
