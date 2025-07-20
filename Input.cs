using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiliLiveRecorder
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }
        public delegate void TextEventHandler(string strText);

        public TextEventHandler TextHandler;
        private void button1_Click(object sender, EventArgs e)
        {
            if (null != TextHandler)
            {
                TextHandler.Invoke(textBox1.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter == (Keys)e.KeyChar)
            {
                if (null != TextHandler)
                {
                    TextHandler.Invoke(textBox1.Text);
                    DialogResult = DialogResult.OK;
                }
            }
        }
        public static DialogResult Show(out string strText)
        {
            string strTemp = string.Empty;

            Input inputDialog = new Input();
            inputDialog.TextHandler = (str) => { strTemp = str; };

            DialogResult result = inputDialog.ShowDialog();
            strText = strTemp;

            return result;
        }
        private void Input_Load(object sender, EventArgs e)
        {
           
        }
    }
}
