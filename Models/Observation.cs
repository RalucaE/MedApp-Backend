using System;
using System.Collections.Generic;

namespace MedicalAPI.Models;

public partial class Observation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ReportId { get; set; }

    public string ObservationType { get; set; } = null!;

    public double Value { get; set; }

    public string Unit { get; set; } = null!;

    public double UpperLimit { get; set; }

    public double LowerLimit { get; set; }

    public string MeasurementDate { get; set; } = null!;

    public virtual Report Report { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
