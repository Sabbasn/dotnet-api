using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.bin;
using dotnet_app.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<User>> Get() 
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetSingle(int id) 
        {
            return Ok(_userService.GetUserById(id));
        }

        [HttpPost]
        public ActionResult<List<User>> AddUser(User user) 
        {
            return Ok(_userService.AddUser(user));
        }
    }
}