using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Common.Interfaces.Authentication;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Domain.Entities;
using Messenger.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeService _dateTimeService;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(
        IDateTimeService dateTimeService, 
        IOptions<JwtSettings> jwtSettings)
    {
        _dateTimeService = dateTimeService;
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtToken(UserEntity user)
    {
        var signCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), 
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            expires: _dateTimeService.UtcNow.AddDays(1),
            claims: claims, 
            audience: _jwtSettings.Audience,
            signingCredentials: signCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}