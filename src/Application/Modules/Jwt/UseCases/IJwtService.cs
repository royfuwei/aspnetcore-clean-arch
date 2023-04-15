using Microsoft.IdentityModel.Tokens;

namespace CleanArch.Application.Modules.Jwt.UseCases;
public interface IJwtService
{
    /// <summary>
    /// 驗證jwt時，不檢查時間
    /// </summary>
    /// <param name="token">jwt</param>
    /// <returns></returns>
    SecurityToken ValidExpiredToken(string token);

    /// <summary>
    /// 驗證jwt
    /// </summary>
    /// <param name="token">jwt</param>
    /// <returns></returns>
    SecurityToken ValidToken(string token);

    string GenerateToken(string userName, int expireMinutes = 30);
}