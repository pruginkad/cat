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
using System.Linq;
using System.Collections;

namespace Mms
{
  public partial class Form1 : Form
  {
    T[] GetMax<T>(int number, List<T> source, T minVal)
    {
      T[] results = new T[number];

      for (int i = 0; i < number; i++)
      {
        results[i] = minVal;
      }

      var curMin = minVal;

      foreach (var e in source)
      {
        int resComp = Comparer.DefaultInvariant.Compare(curMin, e);

        if (resComp < 0)
        {
          int minIndex = Array.IndexOf(results, curMin);
          results[minIndex] = e;
          curMin = results.Min();
        }
      }

      return results;
    }

    public Form1()
    {
      var source = new int[] { 5, 7, 3, 2, 4, 3 }.ToList();
      GetMax(3, source, int.MinValue);

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

    private MailMessage CreateMail(MsgMail msgMail)
    {
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
        List<string> txtFiles = new List<string>();

        try
        {
          txtFiles = Directory.EnumerateFiles(
            sourceDirectory, "*.*", SearchOption.AllDirectories
            ).ToList();
        }
        catch(Exception ex)
        { }
        

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
      return message;
    }

    private async Task SendEMail(MailMessage message, SmtpClient m_client, CancellationToken token)
    {
      try
      {
        await Task.Delay(10, token);

        // Create a message and set up the recipients.
       

        //Send the message.

        
        await m_client.SendMailAsync(message);
        NotifyAboutSent(message);
      }
      catch (Exception ex)
      {
        OnSendError(message, ex);
      }
    }

    private void OnSendError(MailMessage msgMail, Exception ex)
    {
      this.BeginInvoke(new Action(() =>
      {
        foreach(var addr in msgMail.To)
        {
          var item = listViewStat.Items.Insert(0, addr.Address);
          item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "error"));
        }        
      }      
      )) ;
    }

    public delegate void MyDelegate(List<string> msgMail);
    public void DelegateMethod(List<string> msgMail)
    {
      foreach (var addr in msgMail)
      {
        var item = listViewStat.Items.Insert(0, addr);
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "OK"));
        AddLineToSent(addr);
      }
    }
    private void NotifyAboutSent(MailMessage msgMail)
    {
      List<string> addrs = new List<string>();
      foreach (var addr in msgMail.To)
      {
        addrs.Add(addr.Address);
      }
      object[] myArray = new object[1];
      myArray[0] = addrs;
      this.BeginInvoke(new MyDelegate(DelegateMethod), myArray);
    }

    private void AddLineToSent(string line)
    {
      string path = m_content_path;
      // This text is added only once to the file.
      if (!File.Exists(path))
      {
        // Create a file to write to.
        using (StreamWriter sw = File.CreateText(path))
        {
          sw.WriteLine(line);
        }
        return;
      }

      // This text is always added, making the file longer over time
      // if it is not deleted.
      using (StreamWriter sw = File.AppendText(path))
      {
        sw.WriteLine(line);
      }
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

    string m_content_path = @"C:\\exclude.txt";

    private SmtpClient CreateClient()
    {
      Mailer mailer = JsonConvert.DeserializeObject<Mailer>(textBoxMailer.Text);
      var client = new SmtpClient(mailer.smtp, mailer.port);
      client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
      client.Host = mailer.smtp;
      client.Port = mailer.port;
      client.UseDefaultCredentials = false;
      client.EnableSsl = mailer.tls;

      // Add credentials if the SMTP server requires them.

      string password = mailer.smtp_password;
      client.Credentials = new NetworkCredential(mailer.smtp_username, password);

      ServicePointManager.ServerCertificateValidationCallback =
          delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
          {
            return true;
          };
      return client;
    }

    private async void buttonSend_Click(object sender, EventArgs e)
    {
      buttonSend.Enabled = false;

      _cancellToken = new CancellationTokenSource();
      listViewStat.Items.Clear();

      
      MsgMail mail = JsonConvert.DeserializeObject<MsgMail>(textBoxMail.Text);
      
      


      m_content_path = Path.GetDirectoryName(mail.content_path);
      m_content_path = Path.Combine(m_content_path, "exclude.txt");

      var addrs = GetAddressList(mail.content_path);
      var token = _cancellToken.Token;

      var mailPacket = CreateMail(mail);

      //await SendEMail (mailPacket, m_client, token);

      while (addrs.Count > 0)
      {
        var client = CreateClient();

        for (int k = 0; k < 50; k++)
        {
          // 50 Letters
          mailPacket.To.Clear();

          for (int i = 0; i < 50; i++)
          {
            // By 50 addresses
            var addr = addrs.FirstOrDefault();

            if (string.IsNullOrEmpty(addr))
            {
              break;
            }

            addrs.RemoveAt(0);
            mailPacket.To.Add(new MailAddress(addr));
          }
          
          if (mailPacket.To.Count <= 0 || _cancellToken.IsCancellationRequested)
          {
            break;
          }
          
          await SendEMail(mailPacket, client, token);
        }

        // Wait 16 minutes
        if (addrs.Count > 0)
          await Task.Delay(1000 * 60 * 16, token);
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
          parts[0] = parts[0].Replace(" ", string.Empty);
          retList.Add(parts[0]);
        }
      }

      try
      {
        var exclude = File.ReadAllLines(m_content_path);
        retList = retList.Where(t => !exclude.Contains(t)).ToList();
      }
      catch(Exception ex)
      {
        
      }

      retList = retList.OrderBy(email => email.Split('@')[1]).ToList();

      return retList;
    }

    CancellationTokenSource _cancellToken = new CancellationTokenSource();
    
    private void buttonStop_Click(object sender, EventArgs e)
    {
      _cancellToken.Cancel();
    }
  }
}