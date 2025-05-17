using api_econsulta.Interfaces;
using api_econsulta.Models;

namespace api_econsulta.Services
{
    // SchedulingService.cs
 public class SchedulingService : ISchedulingService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;

        public SchedulingService(IScheduleRepository scheduleRepository, IUserRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Schedule>> GetAvailableTimeSlotsAsync(DateTime? startDate, DateTime? endDate, int? doctorId)
        {
            return await _scheduleRepository.GetAvailableSchedulesAsync(startDate, endDate, doctorId);
        }

        public async Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId, bool includePast = false)
        {
            if (!await _userRepository.UserExistsAsync(doctorId))
                throw new KeyNotFoundException("Doctor not found");

            return await _scheduleRepository.GetDoctorSchedulesAsync(doctorId, includePast);
        }

        public async Task<IEnumerable<Schedule>> GetPatientSchedulesAsync(int patientId, bool includePast = false)
        {
            if (!await _userRepository.UserExistsAsync(patientId))
                throw new KeyNotFoundException("Patient not found");

            return await _scheduleRepository.GetPatientSchedulesAsync(patientId, includePast);
        }

        public async Task<Schedule> CreateScheduleAsync(int doctorId, DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new ArgumentException("End time must be after start time");

            if (!await _userRepository.UserExistsAsync(doctorId))
                throw new KeyNotFoundException("Doctor not found");

            var doctor = await _userRepository.GetByIdAsync(doctorId);
            if (doctor.Type != UserType.Doctor)
                throw new InvalidOperationException("Only doctors can create schedules");

            if (!await _scheduleRepository.IsTimeSlotAvailable(doctorId, startTime, endTime))
                throw new InvalidOperationException("Time slot is not available");

            var schedule = new Schedule
            {
                DoctorId = doctorId,
                StartTime = startTime,
                EndTime = endTime,
                IsAvailable = true
            };

            await _scheduleRepository.AddScheduleAsync(schedule);
            return schedule;
        }

        public async Task<Schedule> BookAppointmentAsync(int scheduleId, int patientId)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException("Schedule not found");

            if (!schedule.IsAvailable)
                throw new InvalidOperationException("This time slot is already booked");

            var patient = await _userRepository.GetByIdAsync(patientId);
            if (patient == null || patient.Type != UserType.Patient)
                throw new KeyNotFoundException("Patient not found");

            schedule.IsAvailable = false;
            schedule.PatientId = patientId;

            await _scheduleRepository.UpdateScheduleAsync(schedule);
            return schedule;
        }

        public async Task CancelAppointmentAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException("Schedule not found");

            if (schedule.IsAvailable)
                throw new InvalidOperationException("This time slot is not booked");

            schedule.IsAvailable = true;
            schedule.PatientId = null;

            await _scheduleRepository.UpdateScheduleAsync(schedule);
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException("Schedule not found");

            if (!schedule.IsAvailable && schedule.StartTime > DateTime.Now)
                throw new InvalidOperationException("Cannot delete a booked future schedule");

            await _scheduleRepository.DeleteScheduleAsync(scheduleId);
        }
    }
}
