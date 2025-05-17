using api_econsulta.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly ISchedulingService _schedulingService;

    public AppointmentsController(ISchedulingService schedulingService)
    {
        _schedulingService = schedulingService;
    }

    /// <summary>
    /// Realiza um agendamento
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Patient")] // Apenas pacientes podem acessar
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentRequest request)
    {
        try
        {
            var schedule = await _schedulingService.BookAppointmentAsync(request.ScheduleId, request.PatientId);
            return Ok(new {
                Message = "Agendamento realizado com sucesso",
                Schedule = schedule
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Lista agendamentos de um paciente
    /// </summary>
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientAppointments(int patientId, [FromQuery] bool includePast = false)
    {
        var appointments = await _schedulingService.GetPatientSchedulesAsync(patientId, includePast);
        return Ok(appointments);
    }
}

// DTOs
public class BookAppointmentRequest
{
    /// <summary>
    /// ID do horário disponível
    /// </summary>
    /// <example>456</example>
    public int ScheduleId { get; set; }

    /// <summary>
    /// ID do paciente
    /// </summary>
    /// <example>789</example>
    public int PatientId { get; set; }
}