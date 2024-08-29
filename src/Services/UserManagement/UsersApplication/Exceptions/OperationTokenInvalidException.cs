using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class OperationTokenInvalidException : BadRequestException
    {
        public OperationTokenInvalidException(OperationType operationType) :  base($"Token [\"{nameof(OperationType)}\"={operationType} is invalid. The validity has expired.") { }
    }
}
