using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public User GetUser(int id)
    {
        return _dataAccess.GetAll<User>().First(x=>x.Id==id);
    }

    public User? Delete(int id)
    {
        User userToDelete = _dataAccess.GetAll<User>().First(x => x.Id == id);
        if(userToDelete==null)
        {
            return null;
        }
        _dataAccess.Delete<User>(userToDelete);
        return userToDelete;
    }

    public void Add(User user)
    {
        _dataAccess.Create<User>(user);
    }

    public void Update(User user)
    {
        _dataAccess.Update<User>(user);
    }
}
