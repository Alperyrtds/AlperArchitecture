using Common.DTO;
using Common.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Queries.UserQrys;

public sealed class LoginHnd : IRequestHandler<LoginQry, AlperResult<LoginDto>>
{
    private readonly IConfiguration _configuration;
    private readonly IProjectRepository<TblEmployee> _employeeRepository;
    public LoginHnd(IConfiguration configuration, IProjectRepository<TblEmployee> employeeRepository)
    {
        _configuration = configuration;
        _employeeRepository = employeeRepository;
    }
    public async Task<AlperResult<LoginDto>> Handle(LoginQry request, CancellationToken cancellationToken)
    {
        var result = new AlperResult<LoginDto>();
        try
        {
            var key = _configuration["Jwt:Key"];
            var parolaHash = Genarate.PasswordHash(request.Password, key!);


            //var user = await _employeeRepository.GetByEmailAsync(request.Email);
            var user = await _employeeRepository.GetByEmailAsync(request.Email);

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
                new(ClaimTypes.NameIdentifier, user.Id),
                //new(ClaimTypes.Role, dagitimSirketFirmaKullaniciDto.Rol.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.Name),
                new(ClaimTypes.Surname, user.Surname),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
                //new(ClaimTypes.System, firmaId),
                //new(ClaimTypes.UserData, dagitimSirketFirmaKullaniciDto.KullaniciDurum.ToString()),
                //new(ClaimTypes.Dns, JsonSerializer.Serialize(firmaTip)),
                //new(ClaimTypes.Dsa, string.Join(',', firmaTip.Select(ft => ft.FirmaMenuTip).Distinct())),
                //new(ClaimTypes.GroupSid, firmaAdi),
                //new(ClaimTypes.Country, dagitimSirketFirmaKullaniciDto.KullaniciDagitimSirketId)
            };

         

            var token = new JwtSecurityToken
            (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:TokenExpire"]!)),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


            return new LoginDto(tokenString, claims.ToArray());
        }
        catch (AlperAppException ex)
        {
            return ex.Message;
        }
        catch (Exception e)
        {
            return AlperResult<LoginDto>.Exception(e.Message);
        }
    }
}

