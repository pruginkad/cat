using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mms
{
  public class MsgMail
  {
    public string from { get; set; } = "leftffront@lf.spb.ru";
    public string to { get; set; } = "serovdanil@gmail.com";
    public string body { get; set; } = "test";
    public bool is_body_html { get; set; } = true;
    public string subject { get; set; } = "news";
    public string cc { get; set; }
    public string bcc { get; set; }
    public string attachments { get; set; }
    public bool inline { get; set; } = true;
    public bool pack { get; set; } = false;
    public string content_path { get; set; } = @"D:\TESTS\Mms\MailFolder\1.htm";
  }
}
