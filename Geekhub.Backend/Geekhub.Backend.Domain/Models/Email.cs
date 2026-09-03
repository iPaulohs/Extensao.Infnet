namespace Geekhub.Backend.Domain.Models
{
    public class Email
    {
        public Email(string value)
        {
            Value = value;
            var emailValue = value.Split('@');
            Domain = emailValue[1];
            Address = emailValue[0];
        }

        /// <summary>
        /// Texto completo do email, incluindo o domínio
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Domínio do email
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Username do email, sem o domínio
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Indica se o email foi verificado
        /// </summary>
        public bool Verified { get; set; } = false;

        /// <summary>
        /// Data e hora em que o email foi verificado
        /// </summary>
        public DateTime? VerifiedAt { get; set; }

        /// <summary>
        /// Retorna o valor completo do email
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}
