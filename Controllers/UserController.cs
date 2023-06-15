using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.bin;
using dotnet_app.Models;
using dotnet_app.Dtos.User;
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
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get() 
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetSingle(int id) 
        {
            return Ok(await _userService.GetUserById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> AddUser(AddUserDto user)
        {
            var response = await _userService.AddUser(user);
            if (response.Data is null) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> UpdateUser(UpdateUserDto user) 
        {
            var response = await _userService.UpdateUser(user);
            if (response.Data is null) 
            {
                return NotFound(response);
            }
            return Ok(await _userService.UpdateUser(user));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> DeleteUser(int id) 
        {
            var response = await _userService.DeleteUser(id);
            if (response.Data is null) 
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}