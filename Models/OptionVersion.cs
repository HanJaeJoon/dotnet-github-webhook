using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_github_webhook.Models;


[Table("OptionVersion")]
public class OptionVersion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Tag", TypeName = "VARCHAR2")]
    [MaxLength(64)]
    public string? Tag { get; set; }
}
