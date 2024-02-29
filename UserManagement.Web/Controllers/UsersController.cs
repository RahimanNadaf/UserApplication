﻿using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

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
            IsActive = res.IsActive
        };
        return View(model);
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
            IsActive = res.IsActive
        };
        return View(model);
    }

    [HttpPost]
    [Route("confirmedtoedit")]
    public ActionResult ConfirmedToEdit(User res)
    {
        var model = new UserListItemViewModel
        {
            Id = res.Id,
            Forename = res.Forename,
            Surname = res.Surname,
            Email = res.Email,
            IsActive = res.IsActive
        };
        _userService.Update(res);
        return RedirectToAction("List");
    }


    [Route("delete/{id:int}")]
    public ActionResult Delete(int id)
    {

        var deletedUser = _userService.Delete(id);
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
        var model = new UserListItemViewModel
        {
            Id = res.Id,
            Forename = res.Forename,
            Surname = res.Surname,
            Email = res.Email,
            IsActive = res.IsActive
        };
        _userService.Add(res);
        return RedirectToAction("List");
    }
}