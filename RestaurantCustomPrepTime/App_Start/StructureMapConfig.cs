using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantCustomPrepTime.Business;
using StructureMap;

namespace RestaurantCustomPrepTime
{
    public class StructureMapConfig
    {
        public static void RegisterStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<BusinessRegistry>();
            });
        }
    }
}