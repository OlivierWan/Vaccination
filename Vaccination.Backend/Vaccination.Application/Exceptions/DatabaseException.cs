﻿namespace Vaccination.Application.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        { }
    }
}