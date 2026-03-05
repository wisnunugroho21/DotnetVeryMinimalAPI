using System.ComponentModel.DataAnnotations;

namespace VeryMinimalAPI.Data.Types;

public class User : Entity
{
    [MaxLength(50)]
    public required string Username  { get; set; }
    
    public required string Password  { get; set; }
    
    [MaxLength(256)]
    public string? Name { get; set; }
}