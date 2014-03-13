namespace RestaurantCustomPrepTime.Business.Entity
{
    public enum DaysOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class DayOfWeek : BaseEntity
    {
        public int DayOfWeekId { get; set; }
        public DaysOfWeek Day { get; set; }
    }
}
