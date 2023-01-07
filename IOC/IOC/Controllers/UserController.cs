using IOC.Models;
using IOC.RequestModels;
using IOC.Services;
using IOC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-user-by-id")]
        public async Task<ActionResult<UserInfoRequest>> GetUserByID(int id)
        {
            var users = await _userService.GetUserByID(id);
            UserInfoRequest requestedUser = new UserInfoRequest(users.IDUser, users.Name, users.Surname, users.PhoneNumber);
            return (users == null) ? NotFound("User not found") : requestedUser;
        }

        [HttpGet("get-user-by-name")]
        public async Task<ActionResult<UserInfoRequest>> GetUserByName(string name)
        {
            var users = await _userService.GetUserByName(name);
            UserInfoRequest req = new UserInfoRequest(users.IDUser, users.Name, users.Surname, users.PhoneNumber);
            return (users == null) ? NotFound("User not found") : req;
        }

        [HttpGet("get-all-users")]
        public async Task<ActionResult<List<UserInfoRequest>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            List<UserInfoRequest> requestedUsers = new List<UserInfoRequest>();

            for (int i = 0; i < users.Count; i++)
            {
                UserInfoRequest u = new UserInfoRequest(users[i].IDUser, users[i].Name, users[i].Surname, users[i].PhoneNumber);
                requestedUsers.Add(u);
            }
            return (requestedUsers == null) ? NotFound("No users found") : requestedUsers;
        }

        [HttpPut("edit-user")]
        public async Task<bool> EditUser([FromBody] UserInfoRequest @user)
        {
            User u = new User();
            u.IDUser = (int)@user.IDUser;
            u.Name = @user.Name;
            u.Surname = @user.Surname;
            u.PhoneNumber = @user.PhoneNumber;

            if (@user == null)
                return false;
            else
            {
                var result = await _userService.EditUser(u);
                return true;
            }
        }

    }
}
