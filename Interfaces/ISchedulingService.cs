using api_econsulta.Models;

namespace api_econsulta.Interfaces
{
    // ISchedulingService.cs
    public interface ISchedulingService
    {
        Task<IEnumerable<Schedule>> GetAvailableTimeSlotsAsync(DateTime? startDate, DateTime? endDate, int? doctorId);
        Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId, bool includePast = false);
        Task<IEnumerable<Schedule>> GetPatientSchedulesAsync(int patientId, bool includePast = false);
        Task<Schedule> CreateScheduleAsync(int doctorId, DateTime startTime, DateTime endTime);
        Task<Schedule> BookAppointmentAsync(int scheduleId, int patientId);
        Task CancelAppointmentAsync(int scheduleId);
        Task DeleteScheduleAsync(int scheduleId);
    }

}