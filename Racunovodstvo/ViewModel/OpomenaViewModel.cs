using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Racunovodstvo.ViewModel
{
    public class OpomenaViewModel : BindableBase
    {
        private Notification notification;
        private Opomena opomena;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private string textMail;
        public MyICommand<string> PosaljiCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackCommand { get; private set; }
        public string TextLabel { get; private set; }

        public OpomenaViewModel(Notification notification)
        {
            PosaljiCommand = new MyICommand<string>(sendMail);
            OtkaziCommand = new MyICommand<string>(Back);
            BackCommand = new MyICommand<string>(Back);
            this.notification = notification;
            if (notification != null)
            {
                
                opomena = new Opomena();
                opomena.datum = DateTime.Now;
                opomena.redovnafaktura_id = (int)notification.idDokumenta;
                opomena.Faktura = dbContext.Fakturas.FirstOrDefault(x => x.id == opomena.redovnafaktura_id);
                TextLabel = $"Opomena za poslovnog partnera {opomena.Faktura.PoslovniPartner.naziv} po fakturi {opomena.Faktura.oznaka}";
                TextMail = $"Molimo Vas da izmirite dugovanja po fakturi {opomena.Faktura.oznaka} " +
                    $"Čiji rok za plaćanje je bio {opomena.Faktura.rokplacanja.Value.ToShortDateString()}\n dug po fakturi iznosi {MainWindowViewModel.Instance.UkupnaCenaSaPDV(opomena.Faktura)} RSD";
            }
            
            

        }

        private void Back(string obj)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.obavestenja);
        }

        public string TextMail { get => textMail; set { textMail = value; OnPropertyChanged("TextMail"); } }

        private void sendMail(string text)
        {
            string email_to = opomena.Faktura.PoslovniPartner.email;
            string email_from = "deltaexim021@gmail.com";
            string email_from_sifra = "slavija22";
            string new_pass = Guid.NewGuid().ToString().Substring(0, 10);
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(email_from, email_from_sifra);

                MailMessage mm = new MailMessage(email_from, email_to);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.Subject = @"Neizmiren dug [DELTAEXIM]";
                mm.Body = TextMail;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                opomena.datum = DateTime.Now;
                dbContext.Opomenas.Add(opomena);
                dbContext.Notifications.FirstOrDefault(x => x.Id == notification.Id).obradjena = true;
                dbContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                Notifications.Error er = new Notifications.Error("Problemi sa konekcijom!");
                er.Show();
               
            }

            Back("");
        }
    }
}
