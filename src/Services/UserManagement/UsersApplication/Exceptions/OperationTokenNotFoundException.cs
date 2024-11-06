using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class OperationTokenNotFoundException : NotFoundException
    {
        public OperationTokenNotFoundException(Guid Code, OperationType operationType) : base($"{nameof(OperationType)} [{operationType}]", Code) { }
    }
}
