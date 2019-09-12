using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using IntegrateExternalAPIService1.Dtos;
using IntegrateExternalAPIService1.Entities;
using IntegrateExternalAPIService1.Helper;
using Newtonsoft.Json;

namespace IntegrateExternalAPIService1.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAllAsync(string token, string url);
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
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

        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User Create(User user, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {

            throw new NotImplementedException();
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




        public User GetById(int id)
        {
            return _context.Users.Find(id).FirstOrDefault();
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
