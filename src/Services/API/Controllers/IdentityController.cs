using CleanArch.Application.Modules.Identity;
using CleanArch.Application.Modules.Identity.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Services.API.Controllers;
[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(
        IIdentityService identityService,
        ILogger<IdentityController> logger
    ) {
        _identityService = identityService ??  throw new ArgumentNullException(nameof(identityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet("users")]
    public async Task<List<ApplicationUser>> GetUsers()
    {
        var users = await _identityService.GetUsersAsync();
        return users;
    }
}