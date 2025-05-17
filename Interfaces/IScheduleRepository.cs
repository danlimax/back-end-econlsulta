// IScheduleRepository.cs
using api_econsulta.Models;


namespace api_econsulta.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAvailableSchedulesAsync(DateTime? startDate, DateTime? endDate, int? doctorId);
        Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId, bool includePast = false);
        Task<IEnumerable<Schedule>> GetPatientSchedulesAsync(int patientId, bool includePast = false);
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task AddScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
        Task<bool> IsTimeSlotAvailable(int doctorId, DateTime startTime, DateTime endTime, int? excludeScheduleId = null);
    }
}