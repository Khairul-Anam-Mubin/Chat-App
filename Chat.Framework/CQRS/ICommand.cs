using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS
{
    public interface ICommand : IMetaDataDictionary
    { 
        void ValidateCommand();
        CommandResponse CreateResponse();
        CommandResponse CreateResponse(CommandResponse response);
    }
}