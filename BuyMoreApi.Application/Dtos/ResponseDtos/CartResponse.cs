using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMoreApi.Application.Dtos.ResponseDtos
{
    public record CartResponse(Guid id, Dictionary<Guid, int> itesm);
}