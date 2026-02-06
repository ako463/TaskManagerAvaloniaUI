using System;

namespace TaskManager.Desktop.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {        
    }
}
