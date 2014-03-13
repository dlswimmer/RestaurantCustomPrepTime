using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantCustomPrepTime.Business.Context;
using RestaurantCustomPrepTime.Business.Entity;
using RestaurantCustomPrepTime.Business.Processes;
using Rhino.Mocks;
using DayOfWeek = RestaurantCustomPrepTime.Business.Entity.DayOfWeek;

namespace RestaurantCustomPrepTime.Business.Tests
{
    [TestClass]
    public class CustomPrepTimeProcessTest
    {
        [TestMethod]
        public void TestGetAllReturnsAllPrepTimes()
        {
            var times = GetTestListOfPrepTimes();
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Table<CustomPrepTime>()).Return(times.AsQueryable());
            var process = new CustomPrepTimeProcess(factory);

            var result = process.GetAll();
            Assert.AreEqual(times.Count, result.Count);
            access.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestDeleteRemovesFromDatabase()
        {
            var times = GetTestListOfPrepTimes();
            var toDelete = times.First();
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Table<CustomPrepTime>()).Return(times.AsQueryable());
            foreach (var day in toDelete.PrepDays)
            {
                access.Expect(x => x.Delete(day));
            }
            access.Expect(x => x.Delete(toDelete));
            access.Expect(x => x.Save());
            var process = new CustomPrepTimeProcess(factory);

            process.Delete(toDelete.CustomPrepTimeId);
            access.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteThrowsExceptionWhenPrepTimeDoesNotExistInDatabase()
        {
            var times = GetTestListOfPrepTimes();
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Table<CustomPrepTime>()).Return(times.AsQueryable());

            var process = new CustomPrepTimeProcess(factory);

            process.Delete(-12);
        }

        [TestMethod]
        public void TestUpdateStoresInDatabase()
        {
            var times = GetTestListOfPrepTimes();
            var toUpdate = times.First();
            var item = GetTestPrepTime();
            item.CustomPrepTimeId = toUpdate.CustomPrepTimeId;
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Table<CustomPrepTime>()).Return(times.AsQueryable());
            access.Expect(x => x.Delete(toUpdate.PrepDays.First(d => d.Day == DaysOfWeek.Sunday)));
            access.Expect(x => x.Save());
            var process = new CustomPrepTimeProcess(factory);

            var result = process.Update(item);
            Assert.AreEqual(item.TimeFrom, result.TimeFrom);
            Assert.AreEqual(item.TimeTo, result.TimeTo);
            Assert.AreEqual(item.PrepTime, result.PrepTime);
            Assert.AreEqual(item.PrepDays.Count, result.PrepDays.Count);
            Assert.IsTrue(item.PrepDays.All(x => result.PrepDays.Any(p => p.Day == x.Day)));
            access.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void TestUpdateThrowsExceptionWhenPrepTimeDoesNotExistInDatabase()
        {
            var times = GetTestListOfPrepTimes();
            var item = GetTestPrepTime();
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Table<CustomPrepTime>()).Return(times.AsQueryable());

            var process = new CustomPrepTimeProcess(factory);

            process.Update(item);
        }

        [TestMethod]
        public void TestAddStoresInDatabase()
        {
            var times = GetTestListOfPrepTimes();
            var item = GetTestPrepTime();
            var factory = MockRepository.GenerateMock<IContextFactory>();
            var access = MockRepository.GenerateMock<IRestaurantContextAccess>();
            factory.Expect(x => x.GetRestaurantAccess()).Return(access);
            access.Expect(x => x.Insert(item)).IgnoreArguments();
            access.Expect(x => x.Save());
            var process = new CustomPrepTimeProcess(factory);

            var result = process.Add(item);
            Assert.AreEqual(item.TimeFrom, result.TimeFrom);
            Assert.AreEqual(item.TimeTo, result.TimeTo);
            Assert.AreEqual(item.PrepTime, result.PrepTime);
            Assert.AreEqual(item.PrepDays.Count, result.PrepDays.Count);
            Assert.IsTrue(item.PrepDays.All(x => result.PrepDays.Any(p => p.Day == x.Day)));
            access.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        private List<CustomPrepTime> GetTestListOfPrepTimes()
        {
            var result = new List<CustomPrepTime>();

            result.Add(new CustomPrepTime
            {
                CustomPrepTimeId = 1,
                TimeFrom = new DateTime(2014, 1, 1, 1, 0, 0),
                TimeTo = new DateTime(2014, 1, 1, 2, 2, 0),
                PrepTime = 30,
                PrepDays = new List<DayOfWeek> { new DayOfWeek { DayOfWeekId = 1, Day = DaysOfWeek.Tuesday }, new DayOfWeek { DayOfWeekId = 1, Day = DaysOfWeek.Sunday } }
            });
            result.Add(new CustomPrepTime
            {
                CustomPrepTimeId = 2,
                TimeFrom = new DateTime(2014, 1, 1, 5, 0, 0),
                TimeTo = new DateTime(2014, 1, 1, 6, 0, 0),
                PrepTime = 30,
                PrepDays = new List<DayOfWeek> { new DayOfWeek { DayOfWeekId = 2, Day = DaysOfWeek.Saturday } }
            });

            return result;
        }

        private CustomPrepTime GetTestPrepTime()
        {
            return new CustomPrepTime
            {
                CustomPrepTimeId = 3,
                TimeFrom = new DateTime(2014, 1, 1, 13, 0, 0),
                TimeTo = new DateTime(2014, 1, 1, 15, 0, 0),
                PrepTime = 12,
                PrepDays = new List<DayOfWeek> { new DayOfWeek { Day = DaysOfWeek.Tuesday }, new DayOfWeek { Day = DaysOfWeek.Friday } }
            };
        }
    }
}
