using System;

namespace NextMove.Lib
{
    public class SbdAddressInfo
    {
        public int SenderOrganisationNumber { get; }
        public long ReceiverOrganisationNumber { get; }
        public string ProcessId { get; }
        public string DocumenttypeId { get; }

        public string ForettningsmeldingType
        {
            get { return GetForretningsMelsingType(); }
        }


        public SbdAddressInfo(int sender, long receiver, string processId, string documentId)
        {
            SenderOrganisationNumber = IsValidSenderId(sender) ? sender : ThrowArgumentException(sender); 
            ReceiverOrganisationNumber = IsValidReceiverId(receiver) ? receiver : ThrowArgumentException(receiver);
            ProcessId = IsValidProcessId(processId)?processId : ThrowArgumentException(processId);
            DocumenttypeId = IsValidDocumentId(documentId)?documentId:ThrowArgumentException(documentId);
        }

        private T ThrowArgumentException<T>(T s)
        {
            throw new ArgumentException($"{s} is not a valid value for {nameof(s)}: ");
        }

        private static bool IsValidReceiverId(long receiverId)
        {
            return receiverId.ToString().Length >= 9 && receiverId.ToString().Length <= 11;
        }

        private static bool IsValidSenderId(int receiverId)
        {
            return receiverId.ToString().Length == 9;
        }

        private static bool IsValidProcessId(string prosessId)
        {
            return !string.IsNullOrWhiteSpace(prosessId);
        }

        private static bool IsValidDocumentId(string documentId)
        {
            return !string.IsNullOrEmpty(documentId) && documentId.Contains("::");
        }

        private string GetForretningsMelsingType()
        {
            return DocumenttypeId.Substring(DocumenttypeId.LastIndexOf("::") + 2);
        }
    }
}