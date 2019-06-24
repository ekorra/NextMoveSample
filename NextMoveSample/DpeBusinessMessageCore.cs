using System.Runtime.Serialization;

namespace NextMove.Lib
{

    public abstract class DpeBusinessMessageCore
    {
        public string Orgnr { get; set; }
    }

    [DataContract(Name = "publisering")]
    public class DpeBusinessJournalBusniessMessageCore: DpeBusinessMessageCore
    {

    }

    [DataContract(Name = "innsynskrav")]
    public class DpeBusinessInnsynsBusniessMessageCore : DpeBusinessMessageCore
    {
        public string email { get; set; }
    }

    [DataContract(Name = "publisering")]
    public class DpeBusinessMeetingBusniessMessageCore : DpeBusinessMessageCore
    {
        
    }
}
