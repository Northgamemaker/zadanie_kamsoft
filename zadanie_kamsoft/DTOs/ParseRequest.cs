using System.ComponentModel.DataAnnotations;


namespace zadanie_kamsoft.DTOs;

public record ParseRequest
(
    [Required] content_type Type,
    [Required] string Content
);