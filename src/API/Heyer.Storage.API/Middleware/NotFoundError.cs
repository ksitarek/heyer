using FluentResults;

namespace Heyer.Storage.API.Middleware;

public class NotFoundError : Error
{
    public NotFoundError() : base("Not found.")
    {
        
    }
}