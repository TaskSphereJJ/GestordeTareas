using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendProjectInvitationAsync(string toEmail, string projectTitle, string acceptLink, string rejectLink, DateTime expirationDate);
        Task SendPasswordResetAsync(string toEmail, int codigo);
    }
}
