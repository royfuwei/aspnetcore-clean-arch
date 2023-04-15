using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArch.Application.Modules.Jwt.UseCases;
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(
        IConfiguration configuration
    ) {
        _configuration = configuration;
    }

    public SecurityToken ValidExpiredToken(string token)
    {
        var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");
        var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = _configuration.GetValue<string>("JwtSettings:Audience");
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey!))
        };
        handler.ValidateToken(token, validationParameters, out var validatedToken);

        return validatedToken;
    }

    public SecurityToken ValidToken(string token)
    {
        var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");
        var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = _configuration.GetValue<string>("JwtSettings:Audience");
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey!))
        };
        handler.ValidateToken(token, validationParameters, out var validatedToken);

        return validatedToken;
    }

    public string GenerateToken(string userName, int expireMinutes = 30)
    {
        var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = _configuration.GetValue<string>("JwtSettings:Audience");
        var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");


        // 設定要加入到 JWT Token 中的聲明資訊(Claims)
        var claims = new List<Claim>();

        // 在 RFC 7519 規格中(Section#4)，總共定義了 7 個預設的 Claims，我們應該只用的到兩種！
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName)); // User.Identity.Name
        //claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "The Audience"));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // JWT ID

        // 你可以自行擴充 "roles" 加入登入者該有的角色
        claims.Add(new Claim("roles", "Admin"));
        claims.Add(new Claim("roles", "Users"));

        var userClaimsIdentity = new ClaimsIdentity(claims);

        // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey!));

        // HmacSha256 有要求必須要大於 128 bits，所以 key 不能太短，至少要 16 字元以上
        // https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // 建立 SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            // Audience = audience, // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
            //NotBefore = DateTime.Now, // 預設值就是 DateTime.Now
            //IssuedAt = DateTime.Now, // 預設值就是 DateTime.Now
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(expireMinutes),
            SigningCredentials = signingCredentials
        };

        // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);

        return serializeToken;
    }
      
}