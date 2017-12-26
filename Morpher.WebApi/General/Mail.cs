using System;
using System.Net.Mail;

namespace Morpher.WebService.V3.General
{
    public static class Mail
    {
        public static void SendErrorEmail(Exception e)
        {
            var client = new SmtpClient();
            client.Send("robot2@morpher.ru", "support@morpher.ru", "ws3 exception", e.Message);
        }
    }
}