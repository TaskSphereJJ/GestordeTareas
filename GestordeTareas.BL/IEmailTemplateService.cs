using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public interface IEmailTemplateService
    {
        Task<string> GetProjectInvitationTemplateAsync(string email, string projectTitle, string acceptLink, string rejectLink, DateTime expirationDate);
        Task<string> GetPasswordResetTemplateAsync(string email, int codigo, DateTime expirationDate);
    }
}
