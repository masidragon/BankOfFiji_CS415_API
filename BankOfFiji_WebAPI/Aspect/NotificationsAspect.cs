using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;
using System.Net.Mail;
using BankOfFiji_WebAPI.Models;

namespace BankOfFiji_WebAPI.Aspects
{
    [Serializable]
	public class NotificationsAspect : OnMethodBoundaryAspect
	{
        public override void OnSuccess(MethodExecutionArgs args)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            var GetStatus = (from all in db.Notification
                             where all.NotificationType == args.Method.Name
                             select all.NotificationStatus).FirstOrDefault();

            if (GetStatus == "Allow")
            {
                try
                {

                    Transfer Handler = (Transfer)args.ReturnValue;

                    var Email = (from all in db.BankAccount
                                 where all.accountNo == Handler.Acc_ID
                                 select all.Users.email).FirstOrDefault();

                    var User = (from all in db.BankAccount
                                where all.accountNo == Handler.Acc_ID
                                select all.Users).FirstOrDefault();
                    string FirstName = User.firstName;

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    string EmailBody = Handler.TransferStatus;
                    mail.From = new MailAddress("bankoffiji01@gmail.com");

                    mail.To.Add(Email);

                    if (args.Method.Name == "EnableTransfer")
                    {
                        mail.Subject = "Bank of Fiji - Transfer Successful!";
                    }
                    else if (args.Method.Name == "EnableBillPayment")
                    {
                        mail.Subject = "Bank of Fiji - Bill Payment Successful!";
                    }
                    else if (args.Method.Name == "LowBalance")
                    {
                        mail.Subject = "Bank of Fiji - Transfer Successful!";
                        EmailBody = "Hi, " + FirstName + "! Your account #" + Handler.Acc_ID + " has a balance less than $10.00! Thank you for doing business with Bank of Fiji!";
                    }

                    mail.Body = EmailBody;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("bankoffiji01@gmail.com", "bankoff1j1");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}