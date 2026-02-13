using System.ComponentModel.DataAnnotations;

namespace SimpleService.Domain.Entities;

public class GenderEntity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<UserEntity> Users { get; set; }
}