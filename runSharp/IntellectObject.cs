using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Dynamic;

namespace Mms
{
    public class MailMessageObject
    {
      public MsgMail msg
      {
        get;
        set;
      }
      public MailMessage mail
      {
          get;
          set;
      }
    }
}
