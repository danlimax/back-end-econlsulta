namespace api_econsulta.Models
{
    // Schedule.cs
public class Schedule
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public User Doctor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int? PatientId { get; set; }
    public User Patient { get; set; }
}
}