using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SC4013.Infrastructure.Identity;
using SC4013.Web.Common;

namespace SC4013.Web.Endpoints;

//public class Identity : EndpointGroupBase
//{
    // TODO: move to use IConfiguration
    //private const string TokenSecret = "superSecretKeyForUsedInDevelopmentOnly";
    //private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
    
    // public override void Map(WebApplication app)
    // {
    //     app.MapGroup(this)
    //         .MapIdentityApi<ApplicationUser>();
    // }

    // private async Task<IActionResult> GenerateToken([FromBody] TokenGenerationRequest request)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var key = Encoding.UTF8.GetBytes(TokenSecret);
    //     var claims = new List<Claim>
    //     {
    //         new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //         new(JwtRegisteredClaimNames.Sub, request.Email),
    //         new(JwtRegisteredClaimNames.Email, request.Email),
    //         new("userid", request.UserId),
    //         new(ClaimTypes.Role, "Admin")
    //     };
    //     
    // }
    
    
//}

public class TokenGenerationRequest
{
    public string UserId { get; set; }
    public string Email { get; set; }
}