using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace dotnet_github_webhook.Controllers;

[ApiController]
[Route("[controller]")]
public class WebhookController(IConfiguration configuration) : ControllerBase
{
    private readonly string Secret = configuration["Github:WebhookSecret"] ?? throw new InvalidOperationException();

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();

        var signature = Request.Headers["X-Hub-Signature-256"].ToString();
        var hash = $"sha256={GetHash(payload, Secret)}";

        if (!hash.Equals(signature))
        {
            return Unauthorized();
        }

        var eventType = Request.Headers["X-GitHub-Event"].ToString();
        if (eventType == "push")
        {
        }

        return Ok();
    }

    private static string GetHash(string payload, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        using var hasher = new HMACSHA256(secretBytes);
        var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return ToHexString(hashBytes);
    }

    private static string ToHexString(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.AppendFormat("{0:x2}", b);
        return sb.ToString();
    }
}
