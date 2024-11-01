using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            string? compareUrl = null;
            string? refValue = null;

            using (var document = JsonDocument.Parse(payload))
            {
                var root = document.RootElement;
                compareUrl = root.GetProperty("compare").GetString();
                refValue = root.GetProperty("ref").GetString();
            }

            if (compareUrl is null)
            {
                throw new InvalidOperationException();
            }

            if (refValue?.StartsWith("refs/tags/") == true)
            {
                var tagName = refValue.Replace("refs/tags/", "");
                Console.WriteLine($"Tag pushed: {tagName}");
            }

            var apiUrl = compareUrl.Replace("https://github.com/", "https://api.github.com/repos/");

            using var httpClient = new HttpClient();
            var token = configuration["Github:Pat"] ?? throw new InvalidOperationException();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(responseContent);
                var root = document.RootElement;

                if (root.TryGetProperty("files", out JsonElement filesElement) && filesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var fileElement in filesElement.EnumerateArray())
                    {
                        var filename = fileElement.GetProperty("filename").GetString();
                        var status = fileElement.GetProperty("status").GetString();

                        string? patch = null;
                        if (fileElement.TryGetProperty("patch", out JsonElement patchElement))
                        {
                            patch = patchElement.GetString();
                        }

                        if (string.IsNullOrEmpty(patch)) continue;

                        ParsePatch(patch);

                        //Console.WriteLine($"""
                        //File Name: {filename}"
                        //Status: {status}
                        //{(!string.IsNullOrEmpty(patch) ? $"Content:\n{patch}" : "")}
                        //--------------------------------------------------------------
                        //""");
                    }
                }
            }
            else
            {
                return NotFound();
            }
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

    private static void ParsePatch(string patch)
    {
        var patchLines = patch.Split('\n');

        for (int i = 0; i < patchLines.Length; i++)
        {
            var line = patchLines[i];

            if (line.StartsWith('-'))
            {
                var removedLine = line[1..].Trim();

                if (i + 1 < patchLines.Length && patchLines[i + 1].StartsWith('+'))
                {
                    var addedLine = patchLines[i + 1][1..].Trim();

                    if (TryExtractOptionName(removedLine, out var removedOptionName) &&
                        TryExtractOptionName(addedLine, out var addedOptionName))
                    {
                        Console.WriteLine($"The option name has been changed: {removedOptionName} -> {addedOptionName}");
                        i++;
                    }
                    else
                    {
                        if (TryExtractOptionName(removedLine, out removedOptionName))
                        {
                            Console.WriteLine($"The option has been removed: {removedOptionName}");
                        }
                    }
                }
                else
                {
                    if (TryExtractOptionName(removedLine, out var removedOptionName))
                    {
                        Console.WriteLine($"The option has been removed: {removedOptionName}");
                    }
                }
            }
            else if (line.StartsWith('+'))
            {
                var addedLine = line[1..].Trim();

                if (TryExtractOptionName(addedLine, out var addedOptionName))
                {
                    Console.WriteLine($"New option added: {addedOptionName}");
                }
            }
        }
    }

    static bool TryExtractOptionName(string codeLine, [NotNullWhen(true)] out string? optionName)
    {
        optionName = null;
        var pattern = @"public.*?\s+(\w+)\s*\{.*\}";
        var match = Regex.Match(codeLine, pattern);

        if (match.Success)
        {
            optionName = match.Groups[1].Value;
            return true;
        }

        return false;
    }
}
