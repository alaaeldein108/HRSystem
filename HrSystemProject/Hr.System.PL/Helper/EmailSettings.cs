﻿using System.Net;
using System.Net.Mail;

namespace Hr.System.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("alaaeldein108@gmail.com", "rankuaxohlkuzkaf");
            client.Send("alaaeldein108@gmail.com",email.To,email.Title,email.Body);
        }
    }
}
