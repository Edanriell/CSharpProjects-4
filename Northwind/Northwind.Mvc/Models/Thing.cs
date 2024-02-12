using System.ComponentModel.DataAnnotations;

namespace Mvc.Models;

public record Thing([Range(1, 10)] int? Id, [Required] string? Color, [EmailAddress] string? Email);
