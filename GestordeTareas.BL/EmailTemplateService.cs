using Microsoft.AspNetCore.Hosting;

namespace GestordeTareas.BL
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IWebHostEnvironment _environment;

        public EmailTemplateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> GetProjectInvitationTemplateAsync(
            string email,
            string projectTitle,
            string acceptLink,
            string rejectLink,
            DateTime expirationDate)
        {
            var templatePath = Path.Combine(_environment.ContentRootPath, "Templates", "Email", "ProjectInvitation.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"No se encontró la plantilla en: {templatePath}");
            }

            var template = await File.ReadAllTextAsync(templatePath);

            return template
                .Replace("{{EMAIL}}", email)
                .Replace("{{PROJECT_TITLE}}", projectTitle)
                .Replace("{{ACCEPT_LINK}}", acceptLink)
                .Replace("{{REJECT_LINK}}", rejectLink)
                .Replace("{{EXPIRATION_DATE}}", expirationDate.ToString("dd/MM/yyyy"));
        }

        public async Task<string> GetPasswordResetTemplateAsync(
            string email,
            int codigo,
            DateTime expirationDate)
        {
            var templatePath = Path.Combine(_environment.ContentRootPath, "Templates", "Email", "PasswordReset.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"No se encontró la plantilla en: {templatePath}");
            }

            var template = await File.ReadAllTextAsync(templatePath);

            return template
                .Replace("{{EMAIL}}", email)
                .Replace("{{CODE}}", codigo.ToString())
                .Replace("{{EXPIRATION_DATE}}", expirationDate.ToString("dd/MM/yyyy HH:mm"));
        }
    }
}
