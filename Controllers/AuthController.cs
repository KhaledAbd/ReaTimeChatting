using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using task.app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using task.app.Dtos;

namespace task.app.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    //// api/Auth/
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        
        public AuthController(IConfiguration _config, IMapper _mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._config = _config;
            this._mapper = _mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        //// api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto){
            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);
            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);
            if(result.Succeeded){
                return CreatedAtRoute("GetUser", new {
                    Controller="Users", id = userToCreate.Id
                }, userToReturn);
            }
            return BadRequest(result.Errors);
        }
        [HttpGet("isExist/{username}")]
        public async Task<bool> isExist(string username){
            return await userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper() == username.ToUpper().Trim()) != null ? true : false;
        }
        //// api/Auth/Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto){
            try{
            var user = await userManager.FindByNameAsync(userForLoginDto.Username);
            var result = await signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
            if(result.Succeeded && user != null){
                var appUser = await userManager.Users.Include(p => p.Photos).
                    FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());
                var userForReturn = _mapper.Map<UserForListDto>(appUser);
                return Ok(
                    new{
                        token = GenerateJwtToken(appUser).Result,
                        user = userForReturn
                    }
                );
            }
            }catch(Exception e){
                Console.Error.WriteLine(e.Message);
            }
            return Unauthorized();  
        }
        private async Task<string> GenerateJwtToken(User appUser)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.NormalizedUserName)
            };
            var roles = await userManager.GetRolesAsync(appUser);
            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}