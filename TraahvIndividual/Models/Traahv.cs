using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Net;
using System.Net.Mail;

namespace TraahvIndividual.Models
{
    [Table("Traahv")]
    public class Traahv
    {
        [Key]
        public int Id { get; set; }
        public string SoidukeNumber { get; set; }
        public string OmanikuNimi { get; set; }
        public string OmanikuEpost { get; set; }
        public DateTime Rikkumisekuupaev { get; set; }
        public int KiiruseUletamine { get; set; }

        public int TrahviSuurus { get; set; }
        public virtual ICollection<Login> Logins { get; set; }

      
        public void CalculateFine()
        {
            if (KiiruseUletamine <= 20)
            {
                TrahviSuurus = 50;
            }
            else if (KiiruseUletamine <= 40)
            {
                TrahviSuurus = 100;
            }
            else if (KiiruseUletamine <= 60)
            {
                TrahviSuurus = 200;
            }
            else
            {
                TrahviSuurus = 400;
            }
        }
        public void SendMessage() {

            try
            {
              
                var fromAddress = new MailAddress("matveikulakovski@gmail.com", "Oleg Ivanovih Politseinik");
                var toAddress = new MailAddress(OmanikuEpost, OmanikuNimi);
                const string fromPassword = "xrzf pymd nlox vexs";
                string message1 = "Trahv infromatsion " + SoidukeNumber;
                 string subject = message1;
                 string body = $"Tere, kell {Rikkumisekuupaev} rikute kiirusepiirangut ({KiiruseUletamine}), nii et te trahv {TrahviSuurus}. Maksmiseks on teil 2 kuud";

             
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // E.g., smtp.gmail.com for Gmail
                    Port = 587, // 465 for SSL, 587 for TLS
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                // Create the email message
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    // Send the email
                    smtp.Send(message);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error: " + ex.Message);
            }


        }
    }
}
