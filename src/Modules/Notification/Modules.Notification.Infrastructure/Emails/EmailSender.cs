using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modules.Notification.Application.Abstractions;
using Modules.Notification.Application.ParishManagement.Requests;
using Modules.Notification.Application.PendingMembers;

namespace Modules.Notification.Infrastructure.Emails;

internal sealed class EmailSender : IEmailSender, IDisposable
{
    private readonly EmailSenderOptions _emailOptions;
    private readonly ILogger<EmailSender> _logger;
    private readonly SmtpClient _client;

    public EmailSender(IOptions<EmailSenderOptions> options, ILogger<EmailSender> logger)
    {
        _emailOptions = options.Value;
        _client = CreateClient();
        _logger = logger;
    }

    private SmtpClient CreateClient()
    {
        var client = new SmtpClient();

        client.EnableSsl = true;
        client.Port = _emailOptions.Port;
        client.Host = _emailOptions.Host;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailOptions.FromEmail, _emailOptions.ApiKey);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        return client;
    }

    private static string GenerateEmailTemplate(string title, string subtitle, string content, string buttonText, string buttonLink, string logoUrl = null)
    {
        return $@"
            <html lang='pt-BR'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>{title}</title>
                <style>
                    body {{
                        font-family: 'Segoe UI', Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f8f9fa;
                        color: #333333;
                    }}
                    .header {{
                        background-color: #ffffff;
                        padding: 20px 0;
                        text-align: center;
                        border-bottom: 1px solid #eaeaea;
                    }}
                    .header img {{
                        max-width: 200px;
                        height: auto;
                    }}
                    .email-container {{
                        width: 100%;
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        border-radius: 12px;
                        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        overflow: hidden;
                    }}
                    .content {{
                        padding: 40px 30px;
                    }}
                    .email-title {{
                        font-size: 24px;
                        font-weight: 600;
                        color: #1a1a1a;
                        margin-bottom: 25px;
                        text-align: center;
                    }}
                    .email-subtitle {{
                        font-size: 16px;
                        color: #4a4a4a;
                        line-height: 1.6;
                        margin-bottom: 20px;
                        text-align: center;
                    }}
                    .email-body {{
                        font-size: 16px;
                        color: #4a4a4a;
                        line-height: 1.6;
                        margin-bottom: 20px;
                    }}
                    .button-container {{
                        text-align: center;
                        margin: 30px 0;
                    }}
                    .button {{
                        background-color: rgb(23, 67, 117);
                        color: #ffffff!important;
                        padding: 14px 28px;
                        font-size: 16px;
                        text-decoration: none!important;
                        border-radius: 8px;
                        font-weight: 600;
                        display: inline-block;
                        transition: background-color 0.3s ease;
                    }}
                    .button:hover {{
                        background-color: rgb(26, 42, 114);
                    }}
                    .email-signature {{
                        font-size: 16px;
                        color: #555555;
                        margin-top: 30px;
                        padding-top: 20px;
                        border-top: 1px solid #eaeaea;
                    }}
                    .footer {{
                        background-color: #f8f9fa;
                        padding: 20px;
                        text-align: center;
                        font-size: 14px;
                        color: #555555;
                    }}
                    .warning {{
                        margin: 20px 0;
                        font-style: italic;
                        background-color: #fff3cd;
                        border: 1px solid #ffeaa7;
                        border-radius: 8px;
                        padding: 15px;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='header'>
                        {(logoUrl != null ? $"<img src='{logoUrl}' alt='Logo' />" : "Paróquia São Luiz Gonzaga")}
                    </div>
                    
                    <div class='content'>
                        <h2 class='email-title'>{title}</h2>
                        {(subtitle != null ? $"<h3 class='email-subtitle'>{subtitle}</h3>" : "")}
            
                        {content}
                        
                        {(buttonText != null && buttonLink != null ? $@"
                        <div class='button-container'>
                            <a href='{buttonLink}' class='button'>{buttonText}</a>
                        </div>" : "")}
            
                        <div class='email-signature'>
                            Atenciosamente,<br>
                            <strong>Paróquia São Luiz Gonzaga</strong>
                        </div>
                    </div>
                    
                    <div class='footer'>
                        © 2025 Paróquia São Luiz Gonzaga. Todos os direitos reservados.
                    </div>
                </div>
            </body>
            </html>";
    }

    private static string GenerateResetPasswordContent(string name, string link)
    {
        var title = "Redefinição de senha";
        var content = $@"
            <p class='email-body'>Olá {name},</p>
            <p class='email-body'>Você solicitou a redefinição de sua senha.</p>
            <p class='email-body'>Para continuar, clique no link abaixo.</p>";

        return GenerateEmailTemplate(title, null, content, "Redefinir senha", link);
    }

    private static string GeneratePasswordResetedContent(string name, string link)
    {
        var title = "Senha alterada com sucesso";
        var content = $@"
            <p class='email-body'>Olá {name},</p>
            <p class='email-body'>Sua senha foi alterada com sucesso.</p>
            <div class='warning'>
                <p class='email-body'>Se você não solicitou a redefinição de senha, por favor, entre em contato com o suporte.</p>
            </div>";

        return GenerateEmailTemplate(title, null, content, "Acessar área de membros", link);
    }

    private static string GeneratePendingMemberConfirmationContent(string name, string link)
    {
        var title = "Faça seu cadastro";
        var subtitle = "Área gerencial - Paróquia São Luiz Gonzaga";
        var content = $@"
            <p class='email-body'>Olá {name},</p>
            <p class='email-body'>Você recebeu um convite para criar sua conta na área gerencial da Paróquia São Luiz Gonzaga.</p>
            <p class='email-body'>Para continuar, clique no link abaixo.</p>";

        return GenerateEmailTemplate(title, subtitle, content, "Criar conta", link);
    }



    public async Task SendTokenToResetPassword(SendTokenToResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new MailMessage();

            message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
            message.To.Add(new MailAddress(request.Email, request.FullName));
            message.Subject = "Recupere a sua senha | Paróquia São Luiz Gonzaga";

            var link = $"{_emailOptions.BaseUrlFront}/redefinir-senha?token={request.Token}";

            var htmlContent = GenerateResetPasswordContent(request.FullName, link);

            message.Body = htmlContent;
            message.IsBodyHtml = true;

            await _client.SendMailAsync(message, cancellationToken);

            _logger.LogInformation("Password reset email sent successfully to {Email}", request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", request.Email);
            throw;
        }
    }

    public async Task PasswordReseted(SendPasswordResetedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new MailMessage();

            message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
            message.Subject = "Sua senha foi alterada com sucesso | Paróquia São Luiz Gonzaga";
            message.To.Add(new MailAddress(request.Email, request.FullName));

            var link = $"{_emailOptions.BaseUrlFront}/login";

            var htmlContent = GeneratePasswordResetedContent(request.FullName, link);

            message.Body = htmlContent;
            message.IsBodyHtml = true;

            await _client.SendMailAsync(message, cancellationToken);

            _logger.LogInformation("Password reset confirmation email sent successfully to {Email}", request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset confirmation email to {Email}", request.Email);
            throw;
        }
    }

    public async Task PendingMemberConfirmation(PendingMemberConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new MailMessage();

            message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
            message.To.Add(new MailAddress(request.Email, request.FullName));
            message.Subject = "Faça seu cadastro na área de membros da Paróquia São Luiz Gonzaga";

            var link = $"{_emailOptions.BaseUrlFront}/criar-conta?token={request.Token}";

            var htmlContent = GeneratePendingMemberConfirmationContent(request.FullName, link);

            message.Body = htmlContent;
            message.IsBodyHtml = true;

            await _client.SendMailAsync(message, cancellationToken);

            _logger.LogInformation("Pending member confirmation email sent successfully to {Email}", request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send pending member confirmation email to {Email}", request.Email);
            throw;
        }
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}
