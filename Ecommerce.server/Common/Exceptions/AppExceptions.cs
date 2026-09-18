namespace EcommerceApi.Common.Exceptions;

/// <summary>
/// Excepción base de la aplicación. El middleware de errores la traduce
/// a una respuesta HTTP con el StatusCode correspondiente.
/// </summary>
public abstract class AppException(string message) : Exception(message)
{
    public abstract int StatusCode { get; }
}

public class NotFoundException(string message) : AppException(message)
{
    public override int StatusCode => StatusCodes.Status404NotFound;
}

public class BadRequestException(string message) : AppException(message)
{
    public override int StatusCode => StatusCodes.Status400BadRequest;
}

public class UnauthorizedAppException(string message) : AppException(message)
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}

public class ConflictException(string message) : AppException(message)
{
    public override int StatusCode => StatusCodes.Status409Conflict;
}
