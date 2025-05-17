using api_econsulta.Models;

namespace api_econsulta.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Autentica um usuário e retorna um token JWT
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns>Token JWT ou null se a autenticação falhar</returns>
        Task<string> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="user">Dados do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns>Token JWT para o novo usuário</returns>
        Task<string> RegisterAsync(User user, string password);

        /// <summary>
        /// Gera um novo token JWT para um usuário existente
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <returns>Novo token JWT</returns>
        Task<string> RefreshTokenAsync(int userId);

        /// <summary>
        /// Altera a senha de um usuário
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <param name="currentPassword">Senha atual</param>
        /// <param name="newPassword">Nova senha</param>
        /// <returns>True se a alteração foi bem-sucedida</returns>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        /// <summary>
        /// Verifica se um email já está cadastrado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True se o email já estiver em uso</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}