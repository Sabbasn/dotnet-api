using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Dtos.User;
using dotnet_app.bin;
using dotnet_app.Data;
using dotnet_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterUserDto request) 
        {
            var response = await _authRepository.Register(
                new User {Name = request.Name, Email=request.Email}, request.Password
            );

            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(LoginUserDto request) 
        {
            var response = await _authRepository.Login(request.Name, request.Password);

            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}