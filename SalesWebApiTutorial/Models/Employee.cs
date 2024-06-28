using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebApiTutorial.Models;

[Index("Email", IsUnique = true, Name = "UIDX_Email")]
public class Employee {

    public int Id { get; set; }
    [StringLength(30)]
    public string Email { get; set; } = string.Empty;
    [StringLength(30)]
    public string Password {  get; set; } = string.Empty;

}
