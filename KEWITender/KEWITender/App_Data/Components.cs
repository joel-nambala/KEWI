using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using KEWITender.NAVWS;

namespace KEWITender
{
	public class Components
	{
		public static Procurement ObjNav
		{
			get
			{
				var ws = new Procurement();
				try
				{
					var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"], ConfigurationManager.AppSettings["W_PWD"]);
					ws.Credentials = credentials;
					ws.PreAuthenticate = true;
                }
				catch(Exception ex)
				{
					ex.Data.Clear();
                }
				return ws;
            }
		}

        public static void SentEmailAlerts(string address, string subject, string message)
        {
            try
            {
                string email = "dynamicsselfservice@gmail.com";
                string password = "ydujienvejtdojgv";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = $"<body style='font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;'>{message}</body>";
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }
    }
}