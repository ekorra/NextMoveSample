using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NextMove.Lib
{
    public interface INextMoveClient
    {
        Task<bool> IsIpRunning();
        Task<bool> SendMessage(EnvelopeInfo envelopeInfo, BusinessMessageCore businessMessage,  IEnumerable<FileInfo> files);
        Task<Capabilities> GetCapabilities(string orgnr);
        Task<IList<StandardBusinessDocument>> GetAllMessages();
        Task<StandardBusinessDocument> GetMessage(MessageTypes messageType, DirectoryInfo storagePath);

        Task<string> GetStatus(string messageId);
    }
}