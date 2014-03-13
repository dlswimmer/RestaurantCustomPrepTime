using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using RestaurantCustomPrepTime.Business;
using RestaurantCustomPrepTime.Business.Entity;
using DayOfWeek = RestaurantCustomPrepTime.Business.Entity.DayOfWeek;

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
            TimeFrom = item.TimeFrom.ToString("hh:mm tt");
            TimeTo = item.TimeTo.ToString("hh:mm tt");
            PrepTime = item.PrepTime;

            OnMonday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Monday);
            OnTuesday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Tuesday);
            OnWednesday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Wednesday);
            OnThursday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Thursday);
            OnFriday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Friday);
            OnSaturday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Saturday);
            OnSunday = item.PrepDays.Any(x => x.Day == DaysOfWeek.Sunday);
        }
        public int Id { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public int PrepTime { get; set; }

        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public bool OnSunday { get; set; }

        public CustomPrepTime GetEntity()
        {
            var result = new CustomPrepTime();
            result.CustomPrepTimeId = Id;
            DateTime date;
            if (DateTime.TryParse(TimeFrom, out date))
            {
                result.TimeFrom = date;
            }
            if (DateTime.TryParse(TimeTo, out date))
            {
                result.TimeTo = date;
            }
            result.PrepTime = PrepTime;
            result.PrepDays = new List<DayOfWeek>();

            var addDayAction =
                new Action<bool, DaysOfWeek>(
                    (cond, day) => { if (cond) result.PrepDays.Add(new DayOfWeek {Day = day}); });
            addDayAction(OnMonday, DaysOfWeek.Monday);
            addDayAction(OnTuesday, DaysOfWeek.Tuesday);
            addDayAction(OnWednesday, DaysOfWeek.Wednesday);
            addDayAction(OnThursday, DaysOfWeek.Thursday);
            addDayAction(OnFriday, DaysOfWeek.Friday);
            addDayAction(OnSaturday, DaysOfWeek.Saturday);
            addDayAction(OnSunday, DaysOfWeek.Sunday);
            return result;
        }
    }
}