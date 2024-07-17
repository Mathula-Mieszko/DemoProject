﻿using DemoProject.Dtos.Account;
using DemoProject.Interfaces;
using DemoProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DemoProject.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager
;        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto )
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == loginDto.Username.ToLower());
            
            if (user == null) return Unauthorized("Invalid Username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Username or Password not correct");

            return Ok(
                new NewUserDto
                {
                    UserName=user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }    
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };
                var createdUser = await _userManager.CreateAsync(appUser,registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(appUser, "User");
                    if (role.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser),
                            }    
                        );
                    }
                    else
                    {
                        return StatusCode(500, role.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }


    }
}