// ScheduleRepository.cs
using api_econsulta.DB;
using api_econsulta.Interfaces;
using api_econsulta.Models;
using Microsoft.EntityFrameworkCore;


namespace api_econsulta.Repositories
{
   public class ScheduleRepository : IScheduleRepository
    {
        private readonly EconsultaDbContext _context;

        public ScheduleRepository(EconsultaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetAvailableSchedulesAsync(DateTime? startDate, DateTime? endDate, int? doctorId)
        {
             // Comece com Include
    var query = _context.Schedules
        .Include(s => s.Doctor)
        .Where(s => s.IsAvailable);

    if (startDate.HasValue)
        query = query.Where(s => s.StartTime >= startDate.Value);
        
    if (endDate.HasValue)
        query = query.Where(s => s.EndTime <= endDate.Value);
        
    if (doctorId.HasValue)
        query = query.Where(s => s.DoctorId == doctorId.Value);
        
    return await query
        .OrderBy(s => s.StartTime)
        .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId, bool includePast = false)
        {
            var query = _context.Schedules
        .Include(s => s.Patient)  // Inclua primeiro
        .Where(s => s.DoctorId == doctorId);  // Depois filtre

        if (!includePast)
        {
        query = query.Where(s => s.StartTime >= DateTime.Now);
        }

        return await query.ToListAsync();

        }

       public async Task<IEnumerable<Schedule>> GetPatientSchedulesAsync(int patientId, bool includePast = false)
{
    // Comece com Include para carregar o médico relacionado
    var query = _context.Schedules
        .Include(s => s.Doctor)  // Carrega primeiro os dados do médico
        .Where(s => s.PatientId == patientId);  // Depois aplica o filtro

    // Filtro para agendamentos futuros (se necessário)
    if (!includePast)
    {
        query = query.Where(s => s.StartTime >= DateTime.Now);
    }

    // Ordena e executa a consulta
    return await query
        .OrderBy(s => s.StartTime)
        .AsNoTracking()  // Recomendado para consultas somente leitura
        .ToListAsync();
}

        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            return await _context.Schedules
                .Include(s => s.Doctor)
                .Include(s => s.Patient)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _context.Entry(schedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsTimeSlotAvailable(int doctorId, DateTime startTime, DateTime endTime, int? excludeScheduleId = null)
        {
            var query = _context.Schedules
                .Where(s => s.DoctorId == doctorId &&
                            s.StartTime < endTime && s.EndTime > startTime);

            if (excludeScheduleId.HasValue)
                query = query.Where(s => s.Id != excludeScheduleId.Value);

            var conflictingSchedules = await query.ToListAsync();
            return !conflictingSchedules.Any();
        }
    }
}