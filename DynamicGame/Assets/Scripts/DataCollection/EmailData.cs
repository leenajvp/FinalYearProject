using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EmailData : MonoBehaviour
{
    private DDA.DDAManager ddaManager => FindObjectOfType<DDA.DDAManager>();

    const string kSenderEmailAddress = "xx";
    const string kSenderPassword = "xx";
    const string kReceiverEmailAddress = "xx";

    public void SendAnEmail()
    {
        // Set up the email
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        mail.To.Add(kReceiverEmailAddress);
        mail.Subject = "Dynamic Study Data " + System.DateTime.Now.ToString();
        mail.Body = "Players total enemy hits: " + ddaManager.totaEHits + "\n Players shots: " + ddaManager.totalShots + "\n Player died: " + ddaManager.totalDeaths + "\n Player killed enemies: " + ddaManager.totalKills + "\n Player been hit: " + ddaManager.totalPhits + ddaManager.events + "\n total time " + Time.time.ToString();
        // mail.Attachments.Add(new Attachment("DynamicGame/screenshot.png"));

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                Debug.Log("Email success!");
                return true;
            };

        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e.Message);
        }
        finally
        {
            Debug.Log("Email sent");
        }
    }
}
