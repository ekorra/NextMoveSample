using System;

namespace NextMove.Lib
{
    public class EnvelopeInfo
    {
        public string SenderOrganisationNumber { get; }
        public string ReceiverOrganisationNumber { get; }
        public string ProcessId { get; }
        public string DocumenttypeId { get; }

        public string ConversationId { get; set; }
        public string MessageId { get; set; }
        public string SendignSystem { get; set; }
        public string SenderRef { get; set; }
        public string ReceivinSystem { get; set; }
        public string ReceiverRef { get; set; }

        public string ForettningsmeldingType
        {
            get { return GetForretningsMelsingType(); }
        }


        public EnvelopeInfo(string sender, string receiver, string processId, string documentId)
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

        private static bool IsValidReceiverId(string receiverId)
        {
            
            if (receiverId.Contains(':'))
            {
                receiverId = receiverId.Split(':')[1];
                if (receiverId.Length > 9)
                    return false;
            }

            if (!long.TryParse(receiverId, out long n))
                return false;

            return receiverId.Length >= 9 && receiverId.Length <= 11;
        }

        private static bool IsValidSenderId(string receiverId)
        {
            
            if (receiverId.Contains(':'))
            {
                receiverId = receiverId.Split(':')[1];
            }

            if (!int.TryParse(receiverId, out int n))
                return false;

                
            return receiverId.Length == 9;
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