using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Net;

namespace EloPedidos.Utils
{
    public class MailSender
    {
        /// <summary>
        /// Servidor de gmail: smtp.gmail.com - Porta: 587;
        /// Servidor de hotmail: smtp.live.com - Porta: 465;
        /// </summary>
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSSL { get; set; } = false;
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        /// <summary>
        ///  Endereço para envio
        /// </summary>
        public string ToMailAddress { get; set; }
        /// <summary>
        ///  Titulo do email
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        ///  Corpo da mensagem
        /// </summary>
        public string Body { get; set; }

        public MailSender()
        { }

        public MailSender(string host, int port, bool enableSSL, string account, string password)
        {
            Host = host;
            Port = port;
            EnableSSL = enableSSL;
            Account = account;
            Password = password;
        }

        public bool EnviarEmail(string toMailAddress, string title, string message)
        {
            try
            { 
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = Host;
                    smtp.Port = Port;
                    smtp.EnableSsl = EnableSSL;
                    smtp.Credentials = new NetworkCredential(Account, Password);

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Account);
                    mail.To.Add(new MailAddress(toMailAddress));

                    mail.Subject = title;
                    mail.Body = message;
                    mail.BodyEncoding = Encoding.UTF8;

                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                Msg(ex.ToString());
                return false;
            }
        }

        public void EnviarEmail()
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = Host;
                    smtp.Port = Port;
                    smtp.EnableSsl = EnableSSL;
                    smtp.Credentials = new NetworkCredential(Account, Password);

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Account);
                    mail.To.Add(new MailAddress(ToMailAddress));

                    mail.Subject = Title;
                    mail.Body = Body;
                    mail.BodyEncoding = Encoding.UTF8;

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                Msg(ex.ToString());
            }
        }

        public void Msg(string msg) 
            => Toast.MakeText(Application.Context, msg, ToastLength.Long).Show();
    }
}