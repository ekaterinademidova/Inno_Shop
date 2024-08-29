using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class UserInvalidException : BadRequestException
    {
        public UserInvalidException(string message) :  base(message) { }
    }
}
