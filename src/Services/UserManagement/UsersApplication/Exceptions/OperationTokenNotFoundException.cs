﻿using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class OperationTokenNotFoundException : NotFoundException
    {
        public OperationTokenNotFoundException(Guid Code, OperationType operationType) : base($"OperationToken [\"{nameof(OperationType)}\"={operationType}", Code) { }
    }
}
