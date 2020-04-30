using System;
using System.Collections.Generic;
using System.Text;

namespace NextMoveSample.Wpf.ViewModels
{
    public class ProcessMessage
    {
      
            public ProcessMessage(string selectedProcess)
            {
                SelectedProcess = selectedProcess;
            }

            public string SelectedProcess { get; }
        }
    
}
