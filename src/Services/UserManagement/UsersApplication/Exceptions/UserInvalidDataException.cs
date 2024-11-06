using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class UserInvalidDataException : BadRequestException
    {
        public UserInvalidDataException(string message) :  base(message) { }
    }
}
