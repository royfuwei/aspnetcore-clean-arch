using CleanArch.Application.Modules.Identity;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArch.Infrastructure.Persistence.EFCore;

/// <summary>
/// EFCore WeatherForecastContext Initialise, Seed Data
/// </summary>
public class IdentityContextSeed
{
    private readonly ILogger<IdentityContextSeed> _logger;
    private readonly IdentityContext _context;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityContextSeed(
        ILogger<IdentityContextSeed> logger,
        IdentityContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// 初始化時，如果連線db 是 sql server 就migration
    /// </summary>
    /// <returns></returns>
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task SeedAsync(string username, string email, string password)
    {
        try
        {
            await TrySeedAsync(username, email, password);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync(string username, string email, string password)
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = username, Email = email };
        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, password);
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }
    }
}