using System;
using System.Runtime.Serialization;

namespace NextMove.Lib
{
    [DataContract(Name = "DpvMessage")]
    public class DpvMessage
    {
        public string NonSensitiveTitle { get; set; }
        public string Language { get; set; }
        public Notification Notification { get; set; }
        public DateTime EffectiveTime { get; set; }
        public string MessageSumary { get; set; }
        public string MessageBody { get; set; }
        public DateTime AllowSystemDeleteDateTime { get; set; }
        public bool AllowForwarding { get; set; }
    }
}
