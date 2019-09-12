using System;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntegrateExternalAPIService1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;
        private readonly string _baseUrl;
        public TestAuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _token = _configuration["Token"];
            _baseUrl = _configuration["BaseUrl"];

        }
        public async Task<IActionResult> IndexAsync()
        {
            //We will make a GET request to api

            //The 'using' will help to prevent memory leaks.
            //Create a new instance of HttpClient
            using (HttpClient client = new HttpClient())

            //Setting up the response...         
            using (HttpResponseMessage res = await client.GetAsync(_baseUrl + "posts?access-token=" + _token))
            using (HttpContent content = res.Content)
            {
                //Send request https://gorest.co.in/public-api/posts?access-token=N9_76nHoDfC2adFYzKDz7Js_WNpNSGuhZato
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    Console.WriteLine(res.StatusCode);
                    ViewData["StatusCode"] = res.StatusCode;
                }
            }
            return View();
        }
    }
}