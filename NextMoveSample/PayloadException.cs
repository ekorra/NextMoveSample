using System;
using System.Collections.Generic;
using System.Text;

namespace NextMove.Lib
{
    [Serializable]
    public class PayloadException: Exception
    {
        public PayloadException()
        {
            
        }

        public PayloadException(string message): base(message)
        {

        }

        public PayloadException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
