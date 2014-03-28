using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using RestaurantCustomPrepTime.Business.Context;
using RestaurantCustomPrepTime.Business.Entity;

namespace RestaurantCustomPrepTime.Business.Processes
{
    public interface ICustomPrepTimeProcess
    {
        IList<CustomPrepTime> GetAll();
        void Delete(int id);
        CustomPrepTime Add(CustomPrepTime item);
        CustomPrepTime Update(CustomPrepTime item);
    }

    internal class CustomPrepTimeProcess : ICustomPrepTimeProcess
    {
        private readonly IContextFactory _contextFactory;

        public CustomPrepTimeProcess(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IList<CustomPrepTime> GetAll()
        {
            using (var context = _contextFactory.GetRestaurantAccess())
            {
                return (from x in context.Table<CustomPrepTime>() select x).ToList();
            }
        }

        public void Delete(int id)
        {
            using (var context = _contextFactory.GetRestaurantAccess())
            {
                var entity = context.Table<CustomPrepTime>().FirstOrDefault(x => x.CustomPrepTimeId == id);
                if (entity == null)
                {
                    throw new EntityNotFoundException("CustomPrepTime", id);
                }
                entity.PrepDays.Clear();
                context.Delete(entity);
                context.Save();
            }
        }

        public CustomPrepTime Add(CustomPrepTime item)
        {
            using (var context = _contextFactory.GetRestaurantAccess())
            {
                var entity = new CustomPrepTime
                {
                    TimeFrom = item.TimeFrom,
                    TimeTo = item.TimeTo,
                    PrepTime = item.PrepTime,
                    PrepDays = item.PrepDays
                };
                context.Insert(entity);
                context.Save();
                return entity;
            }
        }

        public CustomPrepTime Update(CustomPrepTime item)
        {
            using (var context = _contextFactory.GetRestaurantAccess())
            {
                var entity = context.Table<CustomPrepTime>().FirstOrDefault(x => x.CustomPrepTimeId == item.CustomPrepTimeId);
                if (entity == null)
                {
                    throw new EntityNotFoundException("CustomPrepTime", item.CustomPrepTimeId);
                }
                entity.TimeFrom = item.TimeFrom;
                entity.TimeTo = item.TimeTo;
                entity.PrepTime = item.PrepTime;
                foreach (var day in entity.PrepDays.Where(x => item.PrepDays.All(p => p != x)).ToList())
                {
                    entity.PrepDays.Remove(day);
                }
                foreach (var day in item.PrepDays.Where(x => entity.PrepDays.All(p => p != x)))
                {
                    entity.PrepDays.Add(day);
                }

                context.Save();
                return entity;
            }
        }
    }
}
