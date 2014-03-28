using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantCustomPrepTime.Business.Entity;
using RestaurantCustomPrepTime.Business.Processes;
using RestaurantCustomPrepTime.Controllers;
using RestaurantCustomPrepTime.Models;
using Rhino.Mocks;

namespace RestaurantCustomPrepTime.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestGetPrepTimesCallsProcess()
        {
            var times = GetTestListOfPrepTimes();
            var process = MockRepository.GenerateMock<ICustomPrepTimeProcess>();
            process.Expect(x => x.GetAll()).Return(times);
            var controller = new HomeController(process);
            var actionResult = controller.GetPrepTimes();

            Assert.IsNotNull(actionResult);
            process.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestDeleteCallsProcess()
        {
            const int id = 10;
            var process = MockRepository.GenerateMock<ICustomPrepTimeProcess>();
            process.Expect(x => x.Delete(id));
            var controller = new HomeController(process);

            controller.Delete(id);

            process.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestUpdateCallsProcess()
        {
            var time = GetTestPrepTime();
            var model = new PrepTimeModel(time);
            var process = MockRepository.GenerateMock<ICustomPrepTimeProcess>();
            process.Expect(x => x.Update(time)).IgnoreArguments().Return(time);
            var controller = new HomeController(process);

            controller.Edit(model);

            process.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestAddCallsProcess()
        {
            var time = GetTestPrepTime();
            var model = new PrepTimeModel(time);
            var process = MockRepository.GenerateMock<ICustomPrepTimeProcess>();
            process.Expect(x => x.Add(time)).IgnoreArguments().Return(time);
            var controller = new HomeController(process);

            controller.Add(model);

            process.VerifyAllExpectations();
        }

        private List<CustomPrepTime> GetTestListOfPrepTimes()
        {
            var result = new List<CustomPrepTime>();

            result.Add(new CustomPrepTime
            {
                CustomPrepTimeId = 1,
                TimeFrom = new TimeSpan(1, 0, 0),
                TimeTo = new TimeSpan(2, 0, 0),
                PrepTime = 30,
                PrepDays = new List<DaysOfWeek> {DaysOfWeek.Tuesday}
            });
            result.Add(new CustomPrepTime
            {
                CustomPrepTimeId = 2,
                TimeFrom = new TimeSpan(5, 0, 0),
                TimeTo = new TimeSpan(6, 0, 0),
                PrepTime = 30,
                PrepDays = new List<DaysOfWeek> { DaysOfWeek.Tuesday }
            });

            return result;
        }

        private CustomPrepTime GetTestPrepTime()
        {
            return new CustomPrepTime
            {
                CustomPrepTimeId = 1,
                TimeFrom = new TimeSpan(1, 0, 0),
                TimeTo = new TimeSpan(2, 0, 0),
                PrepTime = 30,
                PrepDays = new List<DaysOfWeek> { DaysOfWeek.Tuesday }
            };
        }
    }
}
