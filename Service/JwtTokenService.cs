// Service/Implementations/JwtTokenService.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;
using WebApi_Coris.Service.Abstractions;

namespace WebApi_Coris.Service.Implementations
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }


    }
}
