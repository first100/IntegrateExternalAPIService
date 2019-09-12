using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using IntegrateExternalAPIService1.Dtos;
using IntegrateExternalAPIService1.Entities;
using IntegrateExternalAPIService1.Helper;
using IntegrateExternalAPIService1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntegrateExternalAPIService1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly string _baseUrl;
      
        public UserController(IConfiguration configuration, IUserService userService)
        {


            _configuration = configuration;
            _token = _configuration["Token"];
            _userService = userService;
            _baseUrl = _configuration["BaseUrl"];

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //public IActionResult GetAllUsers()
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var usersList = _userService.GetAllAsync(_token, _baseUrl);
            return (await Task.FromResult( Ok(_userService.GetAllAsync(_token, _baseUrl)) ));
            
        }
       
        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            return (await Task.FromResult( Ok(_userService.GetById(id))));
         

          
        }
    }
}