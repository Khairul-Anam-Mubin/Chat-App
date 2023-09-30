using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS
{
    public interface ICommand : IMetaDataDictionary
    { 
        string ApiUrl { get; set; }
        bool FireAndForget { get; set; }
        void ValidateCommand();
        CommandResponse CreateResponse();
        CommandResponse CreateResponse(CommandResponse response);
    }
}