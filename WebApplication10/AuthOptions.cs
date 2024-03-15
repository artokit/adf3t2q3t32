using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication10;

public class AuthOptions
{
    public const string Issuer = "admin";
    public const string Audience = "client";
    private const string Key = "gwejkteowewt-wet-wet-234twtfgw4-t234-t23r1rt3t3-125r3";
    public const int LifeTimeAccessToken = 5;
    public const int LifeTimeRefreshToken = 5 * 60;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}