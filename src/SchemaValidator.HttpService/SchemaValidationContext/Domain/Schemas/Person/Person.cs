using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas.Person;

public record Person
{
    [Required]
    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    [Required]
    public string LastName { get; set; }

    public Gender Gender { get; set; }

    [Range(2, 5)]
    public int NumberWithRange { get; set; }
    
    public DateTime Birthday { get; set; }

    public Company Company { get; set; }

    public Collection<Car> Cars { get; set; }
}

public enum Gender
{
    Male,
    Female
}

public record Car
{
    public string Name { get; set; }

    public Company Manufacturer { get; set; }
}

public record Company
{
    public string Name { get; set; }
}