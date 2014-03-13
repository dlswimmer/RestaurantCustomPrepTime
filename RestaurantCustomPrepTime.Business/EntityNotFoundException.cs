using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantCustomPrepTime.Business
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
            
        }
        public EntityNotFoundException(string entity, int id) : base(string.Format("Entity of type {0} could not be found with id of [{1}]", entity, id))
        {

        }
    }
}
