using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace RestaurantCustomPrepTime
{
    public class IocControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, null);
            }
            return ObjectFactory.GetInstance(controllerType) as Controller;
        }
    }
}