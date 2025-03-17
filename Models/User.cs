using System;
using System.Collections.Generic;

namespace MedicalAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Role Role { get; set; } = null!;

    public object toJson()
    {
        return new
        {
            Id,
            FullName,
            Username,
            Email,
            Password,
            RoleName = Role.Name,
            Observations,
            Reports
        };
    }
}
