using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_app.Dtos.User;
using dotnet_app.Models;
using dotnet_app.bin;
using dotnet_app.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_app.Services.CharacterService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public UserService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }
        public async Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();
            try
            {
                var user = _mapper.Map<User>(newUser);
                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = 
                    await _dataContext.Users.Select(c => _mapper.Map<GetUserDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();
            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new Exception($"Character with Id '{id}' not found.");

                _dataContext.Remove(user);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = 
                    await _dataContext.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();
            serviceResponse.Data = 
                await _dataContext.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            serviceResponse.Data = _mapper.Map<GetUserDto>(dbUser);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();
            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == updatedUser.Id) 
                    ?? throw new Exception($"Character with Id '{updatedUser.Id}' not found.");
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.Password = updatedUser.Password;
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}