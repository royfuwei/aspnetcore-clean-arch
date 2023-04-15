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
    public Task<string> GetTestJwt()
    {
        return Task.FromResult(_jwtHelpers.GenerateToken("test"));
    }

    /// <summary>
    /// 驗證 jwt
    /// </summary>
    /// <returns></returns>
    [HttpGet("verify")]
    public async Task<IDictionary<string, object>> GetVerifyClaims()
    {
        var request = Request;
        var heaaders = request.Headers;
        string authorization = heaaders["Authorization"]!;
        var token = authorization.Split(" ")[1];

        var result = await _jwtHelpers.ValidToken(token);
        return result.Claims;
    }

    /// <summary>
    /// 驗證過期的 jwt
    /// </summary>
    /// <returns></returns>
    [HttpGet("verify-expired"), AllowAnonymous]
    [Obsolete("Deprecated")]
    public async Task<IDictionary<string, object>> GetVerifyExpiredClaims()
    {
        var request = Request;
        var headers = request.Headers;
        string authorization = (string)headers["Authorization"] ?? throw new UnauthorizedAccessException();
        var token = authorization.Split(" ")[1];

        var result = await _jwtHelpers.ValidExpiredToken(token);
        return result.Claims;
    }

}