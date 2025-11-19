using System.Net;
using System.Net.Mail;

namespace cafeservellocontroler.Helper
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;

        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Enviar(string email, string assunto, string mensagem)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string username = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, nome)
                };

                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(username, senha);

                    smtp.EnableSsl = true;           // Gmail exige SSL/TLS
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    // ❌ REMOVIDO – era só para Outlook
                    // smtp.TargetName = "STARTTLS/smtp-mail.outlook.com";

                    smtp.Send(mail);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Erro SMTP: " + ex.Message, ex);
            }
        }
    }
}