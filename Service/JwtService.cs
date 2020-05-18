using System;  
using System.Text;  
using System.Security.Claims;  
using Microsoft.IdentityModel.Tokens;  
using System.IdentityModel.Tokens.Jwt;  
using Microsoft.Extensions.Configuration;  

using userService.Model;
  
namespace userService.Service
{  
    public class JwtService  
    {  
        private readonly string _secret;  
        private readonly string _expDate;  
  
        public JwtService(IConfiguration config)  
        {  
            _secret = config.GetSection("JwtConfig").GetSection("secret").Value;  
            _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;  
        }  
  
        public string GenerateSecurityToken(User user)  
        {  
            var tokenHandler = new JwtSecurityTokenHandler();  
            var key = Encoding.ASCII.GetBytes(_secret);  
            var tokenDescriptor = new SecurityTokenDescriptor  
            {  
                Subject = new ClaimsIdentity(new[]  
                {  
                    //new Claim("email", user.email_user),
                    //new Claim("username", user.username_user),
                    //new Claim("cellphone", user.cellphone_user),
                    //new Claim("address" , user.address_user),
                    new Claim("id", user.id_user.ToString())
                    

                }),  
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),  
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  
            };  
  
            var token = tokenHandler.CreateToken(tokenDescriptor);  
  
            return tokenHandler.WriteToken(token);  
  
        }  
    }  
} 