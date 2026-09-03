namespace Geekhub.Backend.Domain.Models
{
    public class Account
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public Guid Id { get; private init; } = Guid.NewGuid();

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Sobrenome do usuário    
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Nome de exibição do usuário na plataforma
        /// </summary>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Username do usuário na plataforma
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Pequena biografia do usuário
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// URL do avatar do usuário na plataforma
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        public required Email Email { get; set; }

        /// <summary>
        /// Senha do usuário para autenticação na plataforma
        /// </summary>
        public required Password Password { get; set; }
    }
}
