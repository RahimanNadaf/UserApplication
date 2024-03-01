using System.Linq;
using Microsoft.AspNetCore.Hosting;
using UserManagement.Web.Logger;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Controllers;
public class LogController : Controller
{
    // GET: LogController
    private readonly IWebHostEnvironment _webHostEnvironment;
    public LogController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public ActionResult Index()
    {
        var logs = LogEvents.ReadLogs(_webHostEnvironment);
        if (logs != null)
        {
            var items = logs.Select(p => new Logs
            {
                UserId = p.UserId,
                Details = p.Details,
                ShowMessage = p.ShowMessage,
                Type = p.Type,
            }).ToList();

            var model = new LogsViewModel
            {
                Items = items
            };
            return View(model);
        }
        return View();
    }

    [HttpPost]
    [Route("Details")]
    public ActionResult Details(Logs model)
    {
        return View(model);
    }
}
