using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestaurantCustomPrepTime.Business.Context;
using RestaurantCustomPrepTime.Business.Processes;
using StructureMap.Configuration.DSL;

namespace RestaurantCustomPrepTime.Business
{
    public class BusinessRegistry : Registry
    {
        public BusinessRegistry()
        {
            For<ICustomPrepTimeProcess>().Use<CustomPrepTimeProcess>();

            For<IRestaurantContextAccess>().Use<RestaurantContextAccess>();
            For<IContextFactory>().Use<ContextFactory>();
        }
    }
}
