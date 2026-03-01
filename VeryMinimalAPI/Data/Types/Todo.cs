using System.ComponentModel.DataAnnotations;

namespace VeryMinimalAPI.Data.Types;

public class Todo : Entity
{
    [MaxLength(256)]
    public string? Name  { get; set; }
    
    public bool IsComplete { get; set; }
}