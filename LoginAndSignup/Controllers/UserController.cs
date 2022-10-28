using LoginAndSignup.Context;
using LoginAndSignup.Helper;
using LoginAndSignup.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.RegularExpressions;

namespace LoginAndSignup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext db;
        public UserController(DataContext _db)
        {
            db = _db;
        }
        [HttpPost("authonticate")]
        public async Task<IActionResult> Authonticate([FromBody] User u)
        {
            if(u == null)
            {
                return BadRequest();
            }
            var user = await db.users.FirstOrDefaultAsync(x => x.username == u.username && x.password == u.password);
            if (user == null) {
            return NotFound(new {message="User Not Found"});
            }

            return Ok(new
            {
                message="Login Successfully" 
            });
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] User u)
        {
            if (u == null)
            {
                return BadRequest();
            }
            if (await CheckUsername(u.username))
            {
                return BadRequest(new { message = "User Already exist" });
            }
            if (await Checkemail(u.email))
            {
                return BadRequest(new { message = "Email Already exist" });
            }
            var pass = passwordstrength(u.password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new{message = pass.ToString()});
           
            
                u.password = Passwordhash.hashpassword(u.password);
                u.role = "User";
                u.token = "";
                await db.users.AddAsync(u);
                await db.SaveChangesAsync();
                return Ok(new
                {
                    message = "User Successfully Register"
                });
            
             

        }
        private Task<bool> CheckUsername(string username)
        {
            return db.users.AnyAsync(x => x.username == username);
        }
        private Task<bool> Checkemail(string email)
        {
            return db.users.AnyAsync(x => x.email == email);
        }
        private string passwordstrength(string password)
        {
            StringBuilder sb = new StringBuilder();
                if(password.Length < 8)
            {
                sb.Append("minmum password length must be 8 or greate then 8"+Environment.NewLine);
            }
                if(!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password,"[A-Z]")
                && Regex.IsMatch(password,"[0-9]")))
            { 
                sb.Append("password should be alphanumeric"+Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=,]"))
            {
                sb.Append("password should contain special charector"+Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
