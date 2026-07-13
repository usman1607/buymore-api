using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Helpers
{
    public static class Util
    {
        public static string GenerateReference(string prefix)
        {
            return $"{prefix}-{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7).ToUpper()}";
        }
    }
}
