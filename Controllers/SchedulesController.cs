using api_econsulta.Interfaces;
using api_econsulta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_econsulta.Controllers
{
    // SchedulesController.cs
[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly ISchedulingService _schedulingService;

    public SchedulesController(ISchedulingService schedulingService)
    {
        _schedulingService = schedulingService;
    }

    /// <summary>
    /// Cadastra horários disponíveis para um médico
    /// </summary>
    [HttpPost("availability")]
    [Authorize(Roles = "Doctor")] // Apenas médicos podem acessar
    public async Task<IActionResult> CreateAvailability([FromBody] CreateAvailabilityRequest request)
    {
        try
        {
            var schedules = new List<Schedule>();
            
            foreach (var slot in request.TimeSlots)
            {
                var schedule = await _schedulingService.CreateScheduleAsync(
                    request.DoctorId,
                    slot.StartTime,
                    slot.EndTime);
                
                schedules.Add(schedule);
            }

            return CreatedAtAction(nameof(GetAvailability), new { doctorId = request.DoctorId }, schedules);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Lista horários disponíveis de um médico
    /// </summary>
    [HttpGet("availability/{doctorId}")]
    public async Task<IActionResult> GetAvailability(int doctorId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var schedules = await _schedulingService.GetAvailableTimeSlotsAsync(startDate, endDate, doctorId);
        return Ok(schedules);
    }
}

// DTOs
public class CreateAvailabilityRequest
{
    /// <summary>
    /// ID do médico
    /// </summary>
    /// <example>123</example>
    public int DoctorId { get; set; }

    /// <summary>
    /// Lista de horários disponíveis
    /// </summary>
    public List<TimeSlotDto> TimeSlots { get; set; }
}

public class TimeSlotDto
{
    /// <summary>
    /// Data/hora de início (UTC)
    /// </summary>
    /// <example>2024-05-20T09:00:00Z</example>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Data/hora de término (UTC)
    /// </summary>
    /// <example>2024-05-20T09:30:00Z</example>
    public DateTime EndTime { get; set; }
}

// BookAppointmentRequest.cs
public class BookAppointmentRequest
{
    public int PatientId { get; set; }
}
}