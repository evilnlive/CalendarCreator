namespace CalendarCreator.Web.Controllers
{
    using System.Web.Mvc;

    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string calendarData)
        {
            return View();
        }
    }
}