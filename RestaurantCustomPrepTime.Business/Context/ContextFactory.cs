using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantCustomPrepTime.Business.Context
{
    internal class ContextFactory : IContextFactory
    {
        public IRestaurantContextAccess GetRestaurantAccess()
        {
            return new RestaurantContextAccess();
        }
    }

    public interface IContextFactory
    {
        IRestaurantContextAccess GetRestaurantAccess();
    }
}
