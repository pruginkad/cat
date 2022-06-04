using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mms
{
  public class Mailer
  {
    public string smtp { get; set; } = "mail.nic.ru";
    public int port { get; set; } = 587;
    public bool tls { get; set; } = true;
    public string smtp_username { get; set; } = "ssu@lf.spb.ru";
    public string smtp_password { get; set; } = "$Power321";
  }
}
