using System;
using System.Collections.Generic;

namespace RestaurantCustomPrepTime.Business.Entity
{
    public class CustomPrepTime : BaseEntity
    {
        public int CustomPrepTimeId { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int PrepTime { get; set; }

        public virtual ICollection<DayOfWeek> PrepDays { get; set; } 
    }
}