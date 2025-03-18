
using System.Text;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System.Text;

namespace SportifyKerala.Utilitys
{
    public class MailHelper
    {
        public bool SendAccountCreationEmail(string Name, string userEmail)
        {
            try
            {
                // Build the email body
                var emailBody = new StringBuilder();
                emailBody.Append("<html><body>");
                emailBody.Append($"<h1>Dear {Name},</h1>");
                emailBody.Append("<p>We are pleased to inform you that your account has been created successfully in Sportify</p>");
                //emailBody.Append("<p>You can now log in to the system using the following credentials:</p>");
                //emailBody.Append("<p><b>Email:</b> " + userEmail + "</p>");
                //emailBody.Append("<p><b>Password:</b> " + temporaryPassword + "</p>");
                emailBody.Append("<p>You can now see all the Tournament Related Posts from this Website.</p>");
                //emailBody.Append("<p>If you have any questions or require assistance, please contact the college administration.</p>");
                emailBody.Append("<p>Let's Go,<br>______ Sportfy</p>");
                emailBody.Append("</body></html>");

                // Create a new MimeMessage
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse("ansilouseph@notetech.com"));
                mimeMessage.To.Add(MailboxAddress.Parse(userEmail));
                mimeMessage.Subject = "Sportify";
                mimeMessage.Body = new TextPart(TextFormat.Html) { Text = emailBody.ToString() };

                // Send email via SMTP
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("ansilouseph@notetech.com", "koxt txmb gzmu cnop");
                smtp.Send(mimeMessage);
                smtp.Disconnect(true);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }


        //for Club Registration ,here we are sending an otp for registartion
        public bool SendOtpEmail(string userName, string userEmail, string otp)
        {
            try
            {
                // Build the email body
                var emailBody = new StringBuilder();
                emailBody.Append("<html><body>");
                emailBody.Append($"<h1>Hi {userName},</h1>");
                emailBody.Append("<p>Thank you for registering with us!</p>");
                emailBody.Append("<p>Please use the following OTP to verify your account:</p>");
                emailBody.Append($"<p><b>OTP:</b> {otp}</p>");
                emailBody.Append("<p>This OTP is valid for a limited time. Please do not share it with anyone.</p>");
                emailBody.Append("<p>Best regards,<br>Your Support Team</p>");
                emailBody.Append("</body></html>");

                // Create a new MimeMessage
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse("ansilouseph@notetech.com"));
                mimeMessage.To.Add(MailboxAddress.Parse(userEmail));
                mimeMessage.Subject = "Account Verification - OTP";
                mimeMessage.Body = new TextPart(TextFormat.Html) { Text = emailBody.ToString() };

                // Send email via SMTP
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("ansilouseph@notetech.com", "koxt txmb gzmu cnop");
                smtp.Send(mimeMessage);
                smtp.Disconnect(true);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP email: {ex.Message}");
                return false;

            }
        }

    }
}


   

