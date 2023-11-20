using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Models;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController: ControllerBase
    {
        private List<User> users;
        public ManagerController()
        {
            users = new List<User>
            {
                new User { UserId = 1, Username = "Avraham", Password = "A1234!", Manager = true},
                new User { UserId = 2, Username = "Itshak", Password = "Y1234@"},
                new User { UserId = 3, Username = "Yaakov", Password = "Y1234#"}
            };
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;

            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.Manager ? "Manager" : "Agent"),
                new Claim("userId", user.UserId.ToString()),

            };

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}




