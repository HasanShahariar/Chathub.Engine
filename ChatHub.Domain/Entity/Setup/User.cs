using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHub.Domain.Entity.setup;

[Table("Users")]
public class User:BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public string? ProfileImageUrl { get; set; }

    
    public string? PasswordHash { get; set; }


}
