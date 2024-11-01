using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_github_webhook.Models;

[Table("OptionChange")]
public class OptionChange
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("FileName", TypeName = "VARCHAR2")]
    [MaxLength(64)]
    public string? FileName { get; set; }


    [Column("From", TypeName = "VARCHAR2")]
    [MaxLength(64)]
    public string? From { get; set; }


    [Column("To", TypeName = "VARCHAR2")]
    [MaxLength(64)]
    public string? To { get; set; }
}
