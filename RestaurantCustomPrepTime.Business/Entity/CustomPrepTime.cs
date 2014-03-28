using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RestaurantCustomPrepTime.Business.Entity
{
    public class CustomPrepTime : BaseEntity
    {
        private readonly BindingList<DaysOfWeek> _prepDays;
        private int _prepDaysValue;

        public CustomPrepTime()
        {
            _prepDays = new BindingList<DaysOfWeek>();
            _prepDays.ListChanged += ListChanged;
        }

        private void ListChanged(object sender, ListChangedEventArgs e)
        {
            _prepDaysValue = _prepDays.Aggregate(0, (l, r) => l | (int)r);
        }

        public int CustomPrepTimeId { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int PrepTime { get; set; }

        public int PrepDaysValue
        {
            get { return _prepDaysValue; }
            protected internal set
            {
                _prepDaysValue = value;
                PrepDays = Enum.GetValues(typeof (DaysOfWeek)).Cast<DaysOfWeek>().Where(x => (value & (int) x) > 0).ToList();
            }
        }

        public ICollection<DaysOfWeek> PrepDays
        {
            get { return _prepDays; }
            set
            {
                _prepDays.Clear();
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        _prepDays.Add(item);
                    }
                }
            }
        }
    }
}