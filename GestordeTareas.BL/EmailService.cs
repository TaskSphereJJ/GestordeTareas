using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GestordeTareas.BL
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateService _templateService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration,
            IEmailTemplateService templateService,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _templateService = templateService;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["EmailSettings:SMTPServer"])
                {
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SenderEmail"],
                        _configuration["EmailSettings:SenderPassword"]
                    ),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(
                        _configuration["EmailSettings:SenderEmail"],
                        _configuration["EmailSettings:SenderDisplayName"]
                    ),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Correo enviado correctamente a {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo a {toEmail}");
                throw new EmailSendException("Error al enviar el correo electrónico", ex);
            }
        }


        public async Task SendProjectInvitationAsync(
            string toEmail,
            string projectTitle,
            string acceptLink,
            string rejectLink,
            DateTime expirationDate)
        {
            try
            {
                var body = await _templateService.GetProjectInvitationTemplateAsync(
                    toEmail,
                    projectTitle,
                    acceptLink,
                    rejectLink,
                    expirationDate
                );

                await SendEmailAsync(toEmail, $"Invitación al proyecto '{projectTitle}'", body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar invitación de proyecto a {toEmail}");
                throw new EmailSendException("Error al enviar la invitación de proyecto", ex);
            }
        }


        public async Task SendPasswordResetAsync(string toEmail, int codigo)
        {
            try
            {
                var expirationDate = DateTime.Now.AddMinutes(15);

                var body = await _templateService.GetPasswordResetTemplateAsync(
                    toEmail,
                    codigo,
                    expirationDate
                );

                await SendEmailAsync(toEmail, "Restablecimiento de Contraseña", body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo de restablecimiento a {toEmail}");
                throw new EmailSendException("Error al enviar correo de restablecimiento", ex);
            }
        }

        // Método auxiliar opcional (por compatibilidad con código existente)
        public async Task EnviarCorreoRestablecimientoAsync(string toEmail, int codigo)
        {
            await SendPasswordResetAsync(toEmail, codigo);
        }
    }
}
