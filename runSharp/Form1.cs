using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Mms
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      m_DebugForm.Hide();
    }

    protected DebugForm m_DebugForm = new DebugForm();

    private void Form1_Load(object sender, EventArgs e)
    {
      textBoxMailer.Text = Settings.Default.def_mailer;
      textBoxMail.Text = Settings.Default.def_mail;

      if (string.IsNullOrEmpty(textBoxMailer.Text))
      {
        textBoxMailer.Text = JsonConvert.SerializeObject(new Mailer(), Formatting.Indented);
      }

      if (string.IsNullOrEmpty(textBoxMail.Text))
      {
        textBoxMail.Text = JsonConvert.SerializeObject(new MsgMail(), Formatting.Indented);
      }
    }

    private async Task SendEMail(MsgMail msgMail, SmtpClient m_client, CancellationToken token)
    {
      try
      {
        await Task.Delay(10, token);

        // Create a message and set up the recipients.
        MailMessage message = new MailMessage(
           msgMail.from,
           msgMail.to
           );

        if (!string.IsNullOrEmpty(msgMail.content_path))
        {
          message.Body = File.ReadAllText(msgMail.content_path);
          
          var sourceDirectory = Path.GetDirectoryName(msgMail.content_path);
          var fileName = Path.GetFileName(msgMail.content_path);

          var folder = msgMail.content_path.Replace(fileName, Path.GetFileNameWithoutExtension(fileName));
          sourceDirectory = Path.Combine(sourceDirectory, $"{folder}_files");
          var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

          var filesDir = Path.GetFileName(sourceDirectory);
          message.Body = message.Body.Replace($"{filesDir}/", "cid:");

          foreach (string currentFile in txtFiles)
          {
            string fileName1 = currentFile.Substring(sourceDirectory.Length + 1);
            msgMail.attachments += currentFile + ";";
          }
        }
        else
        { 
          message.Body = msgMail.body; 
        }


        string s1 = msgMail.subject;
        message.Subject = s1;

        message.SubjectEncoding = System.Text.Encoding.UTF8;
        message.HeadersEncoding = Encoding.UTF8;
        message.BodyEncoding = Encoding.UTF8;

        message.IsBodyHtml = msgMail.is_body_html;

        if (!string.IsNullOrEmpty(msgMail.bcc))
        {
          message.Bcc.Add(msgMail.bcc);
        }

        if (!string.IsNullOrEmpty(msgMail.cc))
        {
          message.CC.Add(msgMail.cc);
        }

        // Create  the file attachment for this e-mail message.
        if (!string.IsNullOrEmpty(msgMail.attachments))
        {
          string[] filenames = msgMail.attachments.ToLower().Split(';');
          foreach (string filename in filenames)
          {
            if (string.IsNullOrEmpty(filename))
            {
              continue;
            }

            Attachment data = null;
            string mimetype = MediaTypeNames.Application.Octet;

            if (filename.Contains(".jpeg") || filename.Contains(".jpg"))
            {
              mimetype = MediaTypeNames.Image.Jpeg;
            }
            else if (filename.Contains(".bmp"))
            {
              mimetype = "image/bmp";
            }
            else if (filename.Contains(".png"))
            {
              mimetype = "image/png";
            }
            else if (filename.Contains(".thmx"))
            {
              mimetype = "application/vnd.ms-officetheme";
            }
            else if (filename.Contains(".xml"))
            {
              mimetype = MediaTypeNames.Text.Xml;
            }


            if (msgMail.pack)
            {
              FileStream sourceFileStream = File.OpenRead(filename);
              using (FileStream destFileStream = File.Create(filename + ".gz"))
              {


                GZipStream compressingStream = new GZipStream(destFileStream,
                    CompressionMode.Compress);

                byte[] bytes = new byte[2048];
                int bytesRead;

                while ((bytesRead = sourceFileStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                  compressingStream.Write(bytes, 0, bytesRead);
                }

                sourceFileStream.Close();
                compressingStream.Close();
                destFileStream.Close();
                data = new Attachment(destFileStream.Name, MediaTypeNames.Application.Zip);
              }
            }
            else
            {
              using (FileStream fs = File.OpenRead(filename))
              {
                data = new Attachment(filename, mimetype);
                if (message.IsBodyHtml)
                {
                  string cid = Path.GetFileName(filename);
                  data.Name = cid;
                  data.ContentId = cid;
                }
                if (msgMail.inline)
                {
                  data.ContentDisposition.Inline = true;
                }
              }
            }
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filename);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filename);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filename);

            message.Attachments.Add(data);
          }

        }

        //Send the message.

        
        await m_client.SendMailAsync(message);
        NotifyAboutSent(msgMail);
      }
      catch (Exception ex)
      {
        OnSendError(msgMail, ex);
      }
    }

    private void OnSendError(MsgMail msgMail, Exception ex)
    {
      this.BeginInvoke(new Action(() =>
      {
        var item = listViewStat.Items.Add(msgMail.to);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "error"));
      }      
      )) ;
    }

    private void NotifyAboutSent(MsgMail msgMail)
    {
      this.BeginInvoke(new Action(() =>
      {
        var item = listViewStat.Items.Add(msgMail.to);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "OK"));
      }
      ));
    }
    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
      // Get the unique identifier for this asynchronous operation.
      //try
      //{
      //  MailMessageObject msgMail = e.UserState as MailMessageObject;
      //  foreach (Attachment attachment in msgMail.mail.Attachments)
      //  {
      //    attachment.Dispose();
      //  }

      //  if (e.Cancelled || e.Error != null)
      //  {
      //    OnSendError(msgMail.msg, e.Error);
      //  }
      //  else
      //  {
      //    NotifyAboutSent(msgMail.msg);
      //  }
      //}
      //catch (Exception ex)
      //{
      //  MessageBox.Show(ex.Message);
      //}
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.Visible)
      {
        this.Activate();
      }
      m_DebugForm.Show();
      m_DebugForm.Activate();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }


    private void timer1_Tick(object sender, EventArgs e)
    {

    }

    private void showDebugToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        m_DebugForm.Show();
      }
      catch
      {

      }
    }



    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.def_mailer = textBoxMailer.Text;
      Settings.Default.def_mail = textBoxMail.Text;
      Settings.Default.Save();
    }

    private async void buttonSend_Click(object sender, EventArgs e)
    {
      buttonSend.Enabled = false;

      _cancellToken = new CancellationTokenSource();
      listViewStat.Items.Clear();

      Mailer mailer = JsonConvert.DeserializeObject<Mailer>(textBoxMailer.Text);
      MsgMail mail = JsonConvert.DeserializeObject<MsgMail>(textBoxMail.Text);
      
      var m_client = new SmtpClient(mailer.smtp, mailer.port);
      m_client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
      m_client.Host = mailer.smtp;
      m_client.Port = mailer.port;
      m_client.UseDefaultCredentials = false;
      m_client.EnableSsl = mailer.tls;

      // Add credentials if the SMTP server requires them.

      string password = mailer.smtp_password;
      m_client.Credentials = new NetworkCredential(mailer.smtp_username, password);

      ServicePointManager.ServerCertificateValidationCallback =
          delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
          {
            return true;
          };

      var addrs = GetAddressList(mail.content_path);
      var token = _cancellToken.Token;

      await SendEMail (mail, m_client, token);

      foreach (var addr in addrs)
      {
        if (_cancellToken.IsCancellationRequested)
        {
          break;
        }
        mail.to = addr;
        await SendEMail(mail, m_client, token);
      }
      buttonSend.Enabled = true;
    }

    private List<string> GetAddressList(string path)
    {
      var retList = new List<string>();

      var sourceDirectory = Path.GetDirectoryName(path);

      var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.csv", SearchOption.AllDirectories);


      foreach (string currentFile in txtFiles)
      {
        var lines = File.ReadAllLines(currentFile);
        foreach(var line in lines)
        {
          var clean_line = line.Replace("\"", string.Empty);
          var parts = clean_line.Split(',');
          if( parts.Length < 1)
          {
            continue;
          }

          if (!parts[0].Contains("@"))
          {
            continue;
          }

          retList.Add(parts[0]);
        }
      }

      return retList;
    }

    CancellationTokenSource _cancellToken = new CancellationTokenSource();
    
    private void buttonStop_Click(object sender, EventArgs e)
    {
      _cancellToken.Cancel();
    }
  }
}