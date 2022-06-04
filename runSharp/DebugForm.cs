using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mms
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }
        public void AddString(String myString)
        {
            listBox1.Items.Insert(0, myString);
            
            if (listBox1.Items.Count > 1000)
            {
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != listBox1.SelectedItem)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && (((int)m.WParam & 0xFFF0) == SC_CLOSE))
            {
                // User click close button
                // #if !DEBUG
                Hide();
                return;
                //#endif
            }
            base.WndProc(ref m);
        }
    }
}
