using System;
using System.Collections.Generic;
using System.Text;
using NextMove.Lib;

namespace NextMoveSample.Wpf.ViewModels
{
    public class DpaMessageViewModel: MessageViewModel
    {
        public DpaMessageViewModel(NextMoveClient nextMoveClient) : base(nextMoveClient)
        {
        }

        public override BusinessMessageCore GetBusinessMessage()
        {
            return new DpiDigitalBusinessMessage();
        }
    }
}
