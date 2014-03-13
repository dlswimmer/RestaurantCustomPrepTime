using System;
using System.Linq;
using System.Web.Mvc;
using RestaurantCustomPrepTime.Business.Processes;
using RestaurantCustomPrepTime.Models;

namespace RestaurantCustomPrepTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomPrepTimeProcess _process;

        public HomeController(ICustomPrepTimeProcess process)
        {
            _process = process;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPrepTimes()
        {
            var times = _process.GetAll().Select(x => new PrepTimeModel(x));
            return Json(times, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            _process.Delete(id);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Add(PrepTimeModel model)
        {
            if (ModelState.IsValid)
            {
                var item = _process.Add(model.GetEntity());

                return Json(new PrepTimeModel(item));
            }
            throw new Exception("json request is not valid");
        }

        [HttpPost]
        public JsonResult Edit(PrepTimeModel model)
        {
            if (ModelState.IsValid)
            {
                var item = _process.Update(model.GetEntity());
                return Json(new PrepTimeModel(item));
            }
            throw new Exception("json request is not valid");
        }
    }
}
