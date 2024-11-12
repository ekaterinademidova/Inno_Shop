using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class OperationTokenInvalidDataException : BadRequestException
    {
        public OperationTokenInvalidDataException(OperationType operationType) 
            :  base($"{nameof(OperationToken)} [{operationType}] is invalid. The validity has expired.") { }
    }
}
