using MimeKit;
using MailKit.Net.Smtp;
using Music_Band_Website.Model;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Restaurant.Model
{
    public class Mailer
    {
        private static string From { get; set; } = "email@example.com";
        private static string Key { get; set; } = "password";

        private static Dictionary<string, string> Messages { get; set; } = new Dictionary<string, string>()
        {
            { "Subscription","Благодарим Ви, че се абонирахте за нашата услуга!\r\n\r\n" +
                "Ще получавате актуални новини, специални оферти директно във Вашата пощенска кутия.\r\n\r\n" +
                "Ако имате въпроси или искате да управлявате абонамента си, можете да се свържете с нас на {1}.\r\n\r\n" +
                "С най-добри пожелания,\r\n" +
                "Екипът на Mental" },
            //---
            { "News", "Имаме новини за Вас!\r\n\r\n" +
                "Посетете нашата страница за да разберете по-подробно.\r\n\r\n" +
                "С най-добри пожелания,\r\n" +
                "Екипът на Mental" },
            //---
            { "Event", "Каним Ви на специално събитие!\r\n\r\n" +
                "Очакваме Ви с нетърпение!\r\n\r\n" +
                "Посетете нашата страница за повече информация.\r\n\r\n" +
                "С най-добри пожелания,\r\n" +
                "Екипът на Mental" },
            //---
            { "Password Retreival", "Уважаеми/а {0},\r\n\r\n" +
                "Получихме заявка за нулиране на паролата за вашия акаунт. \r\n\r\n" +
                "Вашата временна нова парола е: {1} \r\n\r\n" +
                "Ако имате въпроси или се нуждаете от помощ, моля, свържете се с нашия екип за поддръжка на {2}.\r\n\r\n" +
                "С най-добри пожелания,\r\n" +
                "Екипът на Mental" }

        };

        public static async Task SendMailAsync(User receiver, string subject)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Mental", From));
                message.To.Add(new MailboxAddress(receiver.First_Name + " " + receiver.Last_Name, receiver.Email));
                message.Subject = subject;

                if (Messages.ContainsKey(subject))
                {
                    message.Body = new TextPart("plain")
                    {
                        Text = Messages[subject]
                            .Replace("{0}", receiver.First_Name + " " + receiver.Last_Name)
                            .Replace("{1}", receiver.Password ?? "")
                            .Replace("{2}", From)
                    };
                }

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.abv.bg", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(From, Key);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
