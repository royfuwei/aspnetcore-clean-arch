using CleanArch.Application.Modules.Jwt.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CleanArch.Services.API.Controllers;

[ApiController]
[Route("api/jwt")]
[Authorize]
public class JwtController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<WeatherForecastController> _logger;
    public JwtController(
        IJwtService jwtService,
        ILogger<WeatherForecastController> logger
    ) {
        _jwtService = jwtService ??  throw new ArgumentNullException(nameof(jwtService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet("token/test")]
    [AllowAnonymous]
    public Task<string> GetTestJwt()
    {
        return Task.FromResult(_jwtService.GenerateToken("test"));
    }

    /// <summary>
    /// 驗證 jwt
    /// </summary>
    /// <returns></returns>
    [HttpGet("verify")]
    public Task<SecurityToken> GetVerifyClaims()
    {
        var request = Request;
        var heaaders = request.Headers;
        string authorization = heaaders["Authorization"]!;
        var token = authorization.Split(" ")[1];

        var result = _jwtService.ValidToken(token);
        return Task.FromResult(result);
    }

    /// <summary>
    /// 驗證過期的 jwt
    /// </summary>
    /// <returns></returns>
    [HttpGet("verify-expired"), AllowAnonymous]
    [Obsolete("Deprecated")]
    public Task<SecurityToken> GetVerifyExpiredClaims()
    {
        var request = Request;
        var headers = request.Headers;
        string authorization = (string)headers["Authorization"] ?? throw new UnauthorizedAccessException();
        var token = authorization.Split(" ")[1];

        var result =  _jwtService.ValidExpiredToken(token);
        return Task.FromResult(result);
    }

}