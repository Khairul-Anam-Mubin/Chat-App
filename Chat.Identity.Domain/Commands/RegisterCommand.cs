using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands
{
    public class RegisterCommand : ACommand
    {
        public UserModel UserModel { get; set; }
        public override void ValidateCommand()
        {
            if (UserModel == null)
            {
                throw new Exception("UserModel is not set");
            }
            if (string.IsNullOrEmpty(UserModel.Email))
            {
                throw new Exception("Email can't be empty.");
            }
            if (string.IsNullOrEmpty(UserModel.Password))
            {
                throw new Exception("Password can't Empty");
            }
            if (string.IsNullOrEmpty(UserModel.FirstName)
            || string.IsNullOrEmpty(UserModel.LastName))
            {
                throw new Exception("FirstName or LastName can't be empty.");
            }
            if (UserModel.BirthDay == null)
            {
                throw new Exception("Birthday is not set");
            }
        }
    }
}