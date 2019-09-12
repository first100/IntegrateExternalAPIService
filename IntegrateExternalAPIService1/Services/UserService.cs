using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IntegrateExternalAPIService1.Dtos;
using IntegrateExternalAPIService1.Entities;
using IntegrateExternalAPIService1.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegrateExternalAPIService1.Services
{
    public interface IUserService
    {
        //User Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAllAsync(string token, string url);
        Task<UserDto> GetByIdAsync(int id, string token, string baseUrl);
        Task<HttpResponseMessage> CreateAsync(string _token, string _baseUrl, UserDto userDto);
        Task<HttpResponseMessage> UpdateAsync(int id, string token, string baseUrl,UserDto user);
        Task<HttpResponseMessage> DeleteAsync(int id, string _token, string _baseUrl);
    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public User Authenticate(string username, string password)
        //{
        //    //throw new NotImplementedException();
        //}

        public async Task<HttpResponseMessage> CreateAsync(string _token,string _baseUrl,UserDto userDto)
        {
            using (HttpClient client = new HttpClient()) {
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "N9_76nHoDfC2adFYzKDz7Js_WNpNSGuhZato");
                var result = await client.PostAsync(_baseUrl + "users", new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));
                return result;
            }
             
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id, string _token, string _baseUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "N9_76nHoDfC2adFYzKDz7Js_WNpNSGuhZato");
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = client.DeleteAsync(_baseUrl+ "users/"+id).Result;
            return response;
            
        }

       
        public async Task<HttpResponseMessage> UpdateAsync(int id, string token, string baseUrl, UserDto user)
        {
            using (HttpClient client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "N9_76nHoDfC2adFYzKDz7Js_WNpNSGuhZato");
                var result = await client.PutAsync(baseUrl + "users", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                return result;
            }

        }
        public async Task<IEnumerable<UserDto>> GetAllAsync(string token, string baseUrl)
        {
            using (HttpClient client = new HttpClient())

            //Setting up the response...         
            using (HttpResponseMessage res = await client.GetAsync(baseUrl +
            "users?access-token=" + token))
            using (HttpContent content = res.Content)
            {
                string json = await content.ReadAsStringAsync();
                var rootObject = JsonConvert.DeserializeObject<RootObject>(json);

                var usersDto = new List<UserDto>();

                // Now map the result to UserDto
                foreach (var r in rootObject.result)
                {
                    usersDto.Add(
                        new UserDto
                        {
                            id = int.Parse(r.id),
                            first_name = r.first_name,
                            last_name = r.last_name,
                            //username = r.id, // We don't receive a username back?
                            gender = r.gender,
                            dob = r.dob,
                            email = r.email,
                            phone = r.phone,
                            website = r.website,
                            address = r.address,
                            status = r.status,
                            links = new List<string> //  the response includes 3 links to self, edit and avatar
                            {

                                r._links.self.href,
                                r._links.edit.href,
                                r._links.avatar.href
                            }

                        });

                }

                var users = _mapper.Map<IList<User>>(usersDto);
                _context.Users.Add(users);
                _context.SaveChanges();
                return usersDto;
            }
        }




        public async Task<UserDto> GetByIdAsync(int id, string token, string baseUrl)
        {
            UserDto userDto = null;
            using (HttpClient client = new HttpClient())
            //Setting up the response...         
            using (HttpResponseMessage res = await client.GetAsync(baseUrl +
            "users/" + id + "?access-token=" + token))
            using (HttpContent content = res.Content)
            {
                string json = await content.ReadAsStringAsync();
                var rootObject = JsonConvert.DeserializeObject<RootObjectSingleResult>(json);

                //var usersDto = new List<UserDto>();

                // Now map the result to UserDto
                //foreach (var r in rootObject.result)
                //{
                userDto =
                    new UserDto
                    {
                        id = int.Parse(rootObject.result.id),
                        first_name = rootObject.result.first_name,
                        last_name = rootObject.result.last_name,
                            //username = r.id, // We don't receive a username back?
                            gender = rootObject.result.gender,
                        dob = rootObject.result.dob,
                        email = rootObject.result.email,
                        phone = rootObject.result.phone,
                        website = rootObject.result.website,
                        address = rootObject.result.address,
                        status = rootObject.result.status,
                        links = new List<string> //  the response includes 3 links to self, edit and avatar
                        {

                                rootObject.result._links.self.href,
                                rootObject.result._links.edit.href,
                                rootObject.result._links.avatar.href
                        }

                    };

            }

            //var users = _mapper.Map<IList<User>>(usersDto);
            //_context.Users.Add(users);
            //_context.SaveChanges();
            return userDto;
        }
        //return _context.Users.Find(id).FirstOrDefault();
    }


}
