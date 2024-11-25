using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:SMTPServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderDisplayName"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public string GenerateInvitationEmailBody(string correoElectronico, string projectTitle, string acceptLink, string rejectLink, DateTime expirationDate)
    {
        return $@"
        <!DOCTYPE html>
        <html lang=""es"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Invitación a un proyecto</title>
        </head>
        <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333333; background-color: #f4f4f4; margin: 0; padding: 20px;"">
            <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);"">
                <div style=""background-color: #5e35b1; padding: 30px; text-align: center;"">
                    <h1 style=""color: #ffffff; margin: 0; font-size: 28px; font-weight: bold;"">Invitación a un Proyecto</h1>
                </div>
                <div style=""padding: 30px;"">
                    <p style=""margin-bottom: 20px; font-size: 16px;"">Hola,{correoElectronico}</p>

                    <p style=""margin-bottom: 20px; font-size: 16px;"">Has sido invitado a unirte al proyecto <strong style=""color: #5e35b1;"">{projectTitle}</strong> en nuestra plataforma de gestión de tareas.</p>

                    <div style=""text-align: center; margin: 30px 0;"">
                        <a href=""{acceptLink}"" style=""background-color: #5e35b1; color: #ffffff; padding: 14px 30px; text-decoration: none; border-radius: 50px; display: inline-block; font-weight: bold; font-size: 16px; transition: background-color 0.3s ease;"">Aceptar invitación</a>
                    </div>

                    <p style=""margin-bottom: 20px; font-size: 16px; text-align: center;"">Si no deseas unirte, puedes <a href=""{rejectLink}"" style=""color: #5e35b1; text-decoration: none; font-weight: bold;"">rechazar la invitación</a>.</p>

                    <p style=""margin-bottom: 20px; font-style: italic; color: #666666; text-align: center; font-size: 14px; background-color: #f8f8f8; padding: 10px; border-radius: 4px;"">Este enlace es válido hasta {expirationDate.ToString("dd/MM/yyyy")}.</p>

                    <hr style=""border: none; border-top: 1px solid #e0e0e0; margin: 30px 0;"">

                    <p style=""margin-bottom: 10px; text-align: center; font-size: 16px;"">Saludos cordiales,<br><strong>El equipo de Gestor de Tareas</strong></p>
                </div>
                <div style=""background-color: #f8f8f8; padding: 20px; text-align: center;"">
                    <p style=""font-size: 12px; color: #888888; margin: 0;"">Este es un correo electrónico automático, por favor no responda a este mensaje.</p>
                </div>
            </div>
        </body>
        </html>";
    }

    public async Task EnviarCorreoRestablecimientoAsync(string toEmail, int codigo)
    {
        var subject = "Restablecimiento de Contraseña";
        var body = GeneratePasswordResetEmailBody(toEmail, codigo);

        var expirationDate = DateTime.Now.AddMinutes(15);

        await SendEmailAsync(toEmail, subject, body);
    }

    private string GeneratePasswordResetEmailBody(string toEmail, int codigo)
    {
        var expirationDate = DateTime.Now.AddMinutes(15); // EXpira en 15 minutos

        return $@"
    <!DOCTYPE html>
    <html lang=""es"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Restablecimiento de Contraseña</title>
    </head>
    <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333333; background-color: #f4f4f4; margin: 0; padding: 20px;"">
        <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);"">
            <div style=""background-color: #5e35b1; padding: 30px; text-align: center;"">
                <h1 style=""color: #ffffff; margin: 0; font-size: 28px; font-weight: bold;"">Restablecimiento de Contraseña</h1>
            </div>
            <div style=""padding: 30px;"">
                <p style=""margin-bottom: 20px; font-size: 16px;"">Hola,{toEmail}</p>

                <p style=""margin-bottom: 20px; font-size: 16px;"">Has solicitado un restablecimiento de contraseña en nuestra plataforma. Para proceder, ingresa el siguiente código en la página de restablecimiento:</p>

                <h2 style=""font-size: 24px; color: #5e35b1; font-weight: bold; text-align: center;"">{codigo}</h2>

                <p style=""margin-top: 20px; font-size: 16px;"">Este código es válido por 15 minutos. Si no solicitaste este cambio, ignora este correo.</p>

                <p style=""margin-bottom: 20px; font-style: italic; color: #666666; text-align: center; font-size: 14px; background-color: #f8f8f8; padding: 10px; border-radius: 4px;"">Este código será válido hasta {expirationDate.ToString("dd/MM/yyyy HH:mm")}</p>

                <hr style=""border: none; border-top: 1px solid #e0e0e0; margin: 30px 0;"">

                <p style=""margin-bottom: 10px; text-align: center; font-size: 16px;"">Saludos cordiales,<br><strong>El equipo de Soporte</strong></p>
            </div>
            <div style=""background-color: #f8f8f8; padding: 20px; text-align: center;"">
                <p style=""font-size: 12px; color: #888888; margin: 0;"">Este es un correo electrónico automático, por favor no responda a este mensaje.</p>
            </div>
        </div>
    </body>
    </html>";
    }


}
