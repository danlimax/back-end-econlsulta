using api_econsulta.Interfaces;
using api_econsulta.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var token = await _authService.AuthenticateAsync(dto.Email, dto.Password);
        if (token == null)
            return Unauthorized(new { message = "Email ou senha inválidos" });

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        try
        {
            // Validação básica dos dados
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Email e senha são obrigatórios" });

            // Verifica se o email já está cadastrado
            if (await _authService.EmailExistsAsync(dto.Email))
                return Conflict(new { message = "Email já está em uso" });

            // Cria o usuário
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Type = dto.UserType,
            };

            // Registra o usuário e obtém o token
            var token = await _authService.RegisterAsync(user, dto.Password);

            return Ok(new 
            { 
                Token = token,
                Message = "Registro realizado com sucesso" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro ao registrar: {ex.Message}" });
        }
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType UserType { get; set; }
}