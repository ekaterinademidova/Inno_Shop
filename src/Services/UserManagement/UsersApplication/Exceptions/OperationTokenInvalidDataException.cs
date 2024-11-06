﻿using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class OperationTokenInvalidDataException : BadRequestException
    {
        public OperationTokenInvalidDataException(OperationType operationType) 
            :  base($"{nameof(OperationType)} [{operationType}] is invalid. The validity has expired.") { }
    }
}
