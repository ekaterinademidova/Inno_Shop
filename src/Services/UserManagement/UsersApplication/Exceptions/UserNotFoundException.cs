﻿using BuildingBlocks.Exceptions;

namespace UsersApplication.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid Id) : base("User", Id) { }
    }
}
