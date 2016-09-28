using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Net;
using  System.Net.Mail;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

namespace SportStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "zamowienia@example.pl";
        public string MailFromAddress = "sklepsportowy@example.pl";
        public bool UseSsl = true;
        public string Username = "UżytkownikSmtp";
        public string Password = "HasłoSmto";
        public string ServerName = "smtp.przykład.pl";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";
    }


    public class EmailOrderProcessor : IOrderProcessor
        {
            private EmailSettings emailSettings;

            public EmailOrderProcessor(EmailSettings settingsParam)
            {
                emailSettings = settingsParam;
            }

            public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = emailSettings.UseSsl;
                    smtpClient.Host = emailSettings.ServerName;
                    smtpClient.Port = emailSettings.ServerPort;
                    smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username,emailSettings.Password);

                    if (emailSettings.WriteAsFile)
                    {
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                        smtpClient.EnableSsl = false;
                    }

                    StringBuilder body = new StringBuilder()
                        .AppendLine("Nowe zamówienie")
                        .AppendLine("---")
                        .AppendLine("Produkty:");

                    foreach (var line in cart.Lines)
                    {
                        var subtotal = line.Product.Price*line.Quantity;
                        body.AppendFormat("{0} x {1} (wartość: {2:c})", line.Quantity, line.Product.Name, subtotal);
                    }

                    body.AppendFormat("Wartość całkowita: {0:c}", cart.ComputeTotaValue())
                        .AppendLine("---")
                        .AppendLine("Wysyłka dla:")
                        .AppendLine(shippingDetails.Name)
                        .AppendLine(shippingDetails.Line1)
                        .AppendLine(shippingDetails.Line2 ?? "")
                        .AppendLine(shippingDetails.Line3 ?? "")
                        .AppendLine(shippingDetails.City)
                        .AppendLine(shippingDetails.State)
                        .AppendLine(shippingDetails.ZipCode)
                        .AppendLine(shippingDetails.Country)
                        .AppendLine("---")
                        .AppendFormat("Pakowanie prezentu: {0}", shippingDetails.GiftWrap ? "Tak" : "Nie");

                MailMessage mailMessage= new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "Otrzymano nowe zamówienie!",
                    body.ToString());

                    if (emailSettings.WriteAsFile)
                    {
                        mailMessage.BodyEncoding = Encoding.ASCII;
                    }
                    smtpClient.Send(mailMessage);
                }
            }
        }

    }

