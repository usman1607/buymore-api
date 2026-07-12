using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Entities;

namespace BuyMoreApi.Application.Authentication
{
    public interface ICurrentUser
    {
        User LoggedInUser();
        string LoggedInUserEmail();
    }
}