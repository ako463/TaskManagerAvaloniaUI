using System;

namespace TaskManager.Desktop.Domain.Exceptions;

public class InvalidTaskItemException : Exception
{
    public InvalidTaskItemException(string message) : base(message)
    {        
    }
}
