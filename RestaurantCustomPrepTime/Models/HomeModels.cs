using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RestaurantCustomPrepTime.Business.Entity;

namespace RestaurantCustomPrepTime.Models
{
    public class HomeIndexModel
    {
        public List<PrepTimeModel> PrepTimes { get; set; } 
    }

    public class PrepTimeModel
    {
        public PrepTimeModel()
        {
        }

        public PrepTimeModel(CustomPrepTime item)
        {
            Id = item.CustomPrepTimeId;
            TimeFrom = new DateTime(item.TimeFrom.Ticks).ToString("hh:mm tt");
            TimeTo = new DateTime(item.TimeTo.Ticks).ToString("hh:mm tt");
            PrepTime = item.PrepTime;
            Days = item.PrepDays.Select(x => (int) x).ToList();
        }
        public int Id { get; set; }
        [Required]
        public string TimeFrom { get; set; }
        [Required]
        public string TimeTo { get; set; }
        [Required]
        public int PrepTime { get; set; }

        public List<int> Days { get; set; } 

        public CustomPrepTime GetEntity()
        {
            var result = new CustomPrepTime();
            result.CustomPrepTimeId = Id;
            DateTime date;
            if (DateTime.TryParse(TimeFrom, out date))
            {
                result.TimeFrom = date.TimeOfDay;
            }
            if (DateTime.TryParse(TimeTo, out date))
            {
                result.TimeTo = date.TimeOfDay;
            }
            result.PrepTime = PrepTime;
            result.PrepDays = Days.Select(x => (DaysOfWeek) x).ToList();
            return result;
        }
    }
}