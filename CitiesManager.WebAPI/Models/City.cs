using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models;

public class City
{
    [Key] public Guid Id { get; set; }

    public string? Name { get; set; }
}