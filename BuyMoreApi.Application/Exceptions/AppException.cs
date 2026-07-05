using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Exceptions
{
    public class AppException: Exception
    {
        public string StatusCode { get; set; } = "400";

        public AppException(string message, string statusCode = "400") : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException: AppException
    {
        public NotFoundException(string message) : base(message, "404")
        {
        }
    }

    public class BadRequestException: AppException
    {
        public BadRequestException(string message) : base(message, "400")
        {
        }
    }

    public class UnauthorizedException: AppException
    {
        public UnauthorizedException(string message) : base(message, "401")
        {
        }
    }

    public class ForbiddenException: AppException
    {
        public ForbiddenException(string message) : base(message, "403")
        {
        }
    }
}