using System.Security.Claims;
using CleanArch.Infrastructure.Identity.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Services.API.Controllers;

[ApiController]
[Route("api/jwt")]
[Authorize]
public class JwtController : ControllerBase
{
    private readonly JwtHelpers _jwtHelpers;
    private readonly ILogger<WeatherForecastController> _logger;
    public JwtController(
        JwtHelpers jwtHelpers,
        ILogger<WeatherForecastController> logger
    ) {
        _jwtHelpers = jwtHelpers ??  throw new ArgumentNullException(nameof(jwtHelpers));;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet("token/test")]
    [AllowAnonymous]
    public Task<string> getTestJwt()
    {
        return Task.FromResult($"Bearer {_jwtHelpers.GenerateToken("test")}");
    }

    [HttpGet("claims")]
    public IEnumerable<object> getClaims()
    {
        return User.Claims.Select(item => new { item.Type, item.Value });
    }

    // 回傳我們剛剛在產Token時輸入的username
    [HttpGet("username")]
    public IActionResult GetUserName()
    {
        Console.WriteLine($"User.Identity.Name: {User.Identity.Name}");
        return Ok(User.Identity.Name);
    }

    // 傳回Jwt的id
    [HttpGet("jwtid")]
    public IActionResult GetUniqueId()
    {
        var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
        return Ok(jti.Value);
    }
}