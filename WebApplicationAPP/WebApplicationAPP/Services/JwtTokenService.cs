using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebApplicationAPP.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpiraEn) CrearTokenParaComercio(int idComercio)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var issuer = jwtSection["Issuer"] ?? "Grupo6.Identity";
            var audience = jwtSection["Audience"] ?? "Grupo6.Api";
            var expiraMinutos = int.TryParse(jwtSection["ExpireMinutes"], out var minutos) ? minutos : 60;
            var expiraEn = DateTime.UtcNow.AddMinutes(expiraMinutos);

            var header = new Dictionary<string, object>
            {
                ["alg"] = "HS256",
                ["typ"] = "JWT"
            };

            var payload = new Dictionary<string, object>
            {
                ["sub"] = idComercio.ToString(),
                ["idComercio"] = idComercio.ToString(),
                ["iss"] = issuer,
                ["aud"] = audience,
                ["exp"] = new DateTimeOffset(expiraEn).ToUnixTimeSeconds()
            };

            var headerSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
            var payloadSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
            var unsignedToken = $"{headerSegment}.{payloadSegment}";
            var signature = GenerarFirma(unsignedToken);

            return ($"{unsignedToken}.{signature}", expiraEn);
        }

        public ClaimsPrincipal? ValidarToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                return null;
            }

            var unsignedToken = $"{parts[0]}.{parts[1]}";
            var expectedSignature = GenerarFirma(unsignedToken);

            if (!FirmaCoincide(parts[2], expectedSignature))
            {
                return null;
            }

            try
            {
                var payloadBytes = Base64UrlDecode(parts[1]);
                var payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadBytes);

                if (payload is null)
                {
                    return null;
                }

                var jwtSection = _configuration.GetSection("Jwt");
                var issuer = jwtSection["Issuer"] ?? "Grupo6.Identity";
                var audience = jwtSection["Audience"] ?? "Grupo6.Api";

                if (!payload.TryGetValue("iss", out var iss) || iss.GetString() != issuer)
                {
                    return null;
                }

                if (!payload.TryGetValue("aud", out var aud) || aud.GetString() != audience)
                {
                    return null;
                }

                if (!payload.TryGetValue("exp", out var exp) || !exp.TryGetInt64(out var expUnix))
                {
                    return null;
                }

                var expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix);
                if (expiration < DateTimeOffset.UtcNow)
                {
                    return null;
                }

                var claims = new List<Claim>();

                if (payload.TryGetValue("sub", out var sub))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, sub.GetString() ?? string.Empty));
                }

                if (payload.TryGetValue("idComercio", out var idComercio))
                {
                    claims.Add(new Claim("idComercio", idComercio.GetString() ?? string.Empty));
                }

                var identity = new ClaimsIdentity(claims, "CustomJwt");
                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return null;
            }
        }

        private string GenerarFirma(string content)
        {
            var secret = ObtenerClave();
            using var hmac = new HMACSHA256(secret);
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
            return Base64UrlEncode(signatureBytes);
        }

        private byte[] ObtenerClave()
        {
            var key = _configuration.GetSection("Jwt")["Key"] ?? "Grupo6_ClaveJwt_De_Desarrollo_2026_Segura";
            return Encoding.UTF8.GetBytes(key);
        }

        private static bool FirmaCoincide(string left, string right)
        {
            var leftBytes = Encoding.UTF8.GetBytes(left);
            var rightBytes = Encoding.UTF8.GetBytes(right);

            if (leftBytes.Length != rightBytes.Length)
            {
                return false;
            }

            return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static byte[] Base64UrlDecode(string input)
        {
            var padded = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch (padded.Length % 4)
            {
                case 2:
                    padded += "==";
                    break;
                case 3:
                    padded += "=";
                    break;
            }

            return Convert.FromBase64String(padded);
        }
    }
}
