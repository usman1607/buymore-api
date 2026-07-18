using System;
using System.Collections.Generic;
using System.Text;

namespace BuyMoreApi.Application.Repositories
{
    public interface IBaseRepository
    {
        Task SaveChangesAsync();
    }
}
