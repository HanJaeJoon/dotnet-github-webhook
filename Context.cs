using dotnet_github_webhook.Models;
using Microsoft.EntityFrameworkCore;
using OptionVersion = dotnet_github_webhook.Models.OptionVersion;
namespace dotnet_github_webhook;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public required DbSet<OptionVersion> OptionVersions { get; set; }
    public required DbSet<OptionChange> OptionChanges { get; set; }
}
