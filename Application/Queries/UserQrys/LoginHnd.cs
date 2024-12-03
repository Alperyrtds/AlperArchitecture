using Common.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Alper.Domain.Exceptions;
using Alper.Infrastructure.Models;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Application.Queries.UserQrys;
using Common.DTO.User;
using Microsoft.Extensions.Logging;

namespace Alper.Application.Queries.UserQrys;

public sealed class LoginHnd(IConfiguration configuration, IProjectRepository<TblUsers> employeeRepository, ILogger<LoginHnd> logger)
    : IRequestHandler<LoginQry, AlperResult<LoginDto>>
{
    public async Task<AlperResult<LoginDto>> Handle(LoginQry request, CancellationToken cancellationToken)
    {
        var result = new AlperResult<LoginDto>();

        logger.LogInformation($"Login Started");
        try
        {
            var key = configuration["Jwt:Key"];
            var parolaHash = Genarate.PasswordHash(request.Password, key!);

            var user = await employeeRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                return "Kullanıcı bulunamadı";
            }

            if (user != null && user.Password != parolaHash)
            {
                return "Kullanıcı şifresi hatalı";
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id!),
                new(ClaimTypes.Email, user.Email !),
                new(ClaimTypes.GivenName, user.Name !),
                new(ClaimTypes.Surname, user.Surname !),
                new(ClaimTypes.MobilePhone, user.PhoneNumber !),
                new(ClaimTypes.Role, user.TransactionUser !),
            };

            var token = new JwtSecurityToken
            (
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(configuration["Jwt:TokenExpire"]!)),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            logger.LogInformation($"Login Successfully Finished and loggined user is Id=> {user.Id}");

            return new LoginDto(tokenString, claims.ToArray());
        }
        catch (AlperAppException ex)
        {
            logger.LogError($"Login Error => {ex.Message}");
            return ex.Message;
        }
        catch (Exception ex)
        {
            logger.LogError($"Login Error => {ex.Message}");

            return AlperResult<LoginDto>.Exception(ex.Message);
        }
    }
}

