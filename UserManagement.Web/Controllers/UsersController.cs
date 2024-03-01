using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Logger;
using UserManagement.Web.Models.Users;
namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment) {
        _userService = userService;
        _webHostEnvironment = webHostEnvironment;
    } 

    [HttpGet]
    public ViewResult List(bool? activeOnly = null)
    {
        var users = _userService.GetAll();

        if (activeOnly.HasValue)
        {
            if (activeOnly == true)
            {
                users = users.Where(u => u.IsActive);
            }
            else
            {
                users = users.Where(u => !u.IsActive);
            }
        }

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth  = p.DateOfBirth,
            IsActive = p.IsActive
        }).ToList();

        var model = new UserListViewModel
        {
            Items = items
        };
        var log = new Logs()
        {
            UserId = 0,
            Type = "Info",
            ShowMessage = "User list is loaded",
            Details = "user list is loaded at :" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString()
        };
        LogEvents.LogToFile(log, _webHostEnvironment);
        return View(model);
    }



    [Route("details/{id:int}")]
    public ActionResult Details(int id)
    {
        //delete_by_id(id);

        // Return desired view
        var res = _userService.GetUser(id);
        var model = new UserListItemViewModel
        {
            Id = res.Id,
            Forename = res.Forename,
            Surname = res.Surname,
            Email = res.Email,
            DateOfBirth = res.DateOfBirth,
            IsActive = res.IsActive
        };
        var log = new Logs()
        {
            UserId = id,
            Type = "Info",
            ShowMessage = "User details viewed",
            Details = "Viewed user details at :" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString()
        };
        LogEvents.LogToFile(log, _webHostEnvironment);
        var logs = LogEvents.ReadLogs(_webHostEnvironment);
        var _logModel = new LogsViewModel();
        if (logs != null)
        {
            var items = logs.Where(x => x.UserId == id).Select(p => new Logs
            {
                UserId = p.UserId,
                Details = p.Details,
                ShowMessage = p.ShowMessage,
                Type = p.Type,
            }).ToList();

            _logModel = new LogsViewModel
            {
                Items = items
            };
        }
        var userLogViewModels = new UserDetailsLogViewModel
        {
            userItem = model,
            userLogs = _logModel
        };
        return View(userLogViewModels);
    }

    [Route("edit/{id:int}")]
    public ActionResult Edit(int id)
    {
        var res = _userService.GetUser(id);
        var model = new UserListItemViewModel
        {
            Id = res.Id,
            Forename = res.Forename,
            Surname = res.Surname,
            Email = res.Email,
            DateOfBirth = res.DateOfBirth,
            IsActive = res.IsActive
        };
        return View(model);
    }

    [HttpPost]
    [Route("confirmedtoedit")]
    public ActionResult ConfirmedToEdit(User res)
    {
        if (ModelState.IsValid)
        {
            var model = new UserListItemViewModel
            {
                Id = res.Id,
                Forename = res.Forename,
                Surname = res.Surname,
                Email = res.Email,
                DateOfBirth = res.DateOfBirth,
                IsActive = res.IsActive
            };
            _userService.Update(res);

            var log = new Logs()
            {
                UserId = res.Id,
                Type = "Info",
                ShowMessage = "Edited user details",
                Details = "User edited successfully at :" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString()
            };
            LogEvents.LogToFile(log, _webHostEnvironment);
            TempData["message"] = "User edited successfully";
        }
        return RedirectToAction("List");
    }


    [Route("delete/{id:int}")]
    public ActionResult Delete(int id)
    {

        var deletedUser = _userService.Delete(id);
        TempData["message"] = "User deleted successfully";
        var log = new Logs()
        {
            UserId = id,
            Type = "Info",
            ShowMessage = "deleted user",
            Details = "User was deleted at :" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString()
        };
        LogEvents.LogToFile(log, _webHostEnvironment);
        return RedirectToAction("List");
    }

    [Route("create")]
    public ActionResult Create(int id)
    {
        return View();
    }

    [HttpPost]
    [Route("confirmedtoadd")]
    public ActionResult ConfirmedToAdd(User res)
    {
        if(ModelState.IsValid)
        {
            var model = new UserListItemViewModel
            {
                Id = res.Id,
                Forename = res.Forename,
                Surname = res.Surname,
                Email = res.Email,
                DateOfBirth = res.DateOfBirth,
                IsActive = res.IsActive
            };
            _userService.Add(res);
            TempData["message"] = "User added successfully";
            var log = new Logs()
            {
                UserId = res.Id,
                Type = "Info",
                ShowMessage = "Added new user",
                Details = "Added new user at :" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString()
            };
            LogEvents.LogToFile(log, _webHostEnvironment);
        }
        return RedirectToAction("List");

    }
}
