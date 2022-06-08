using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RichardTestJson
{
    public partial class ErrorLog : Form
    {
        private BindingSource _src;

        public ErrorLog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SabLogger.Instance.LogDebug("Hello World");
        }

        private void ErrorLog_Load(object sender, EventArgs e)
        {
            try
            {
                SabLogger.Instance.NewMessage += Instance_NewMessage;
                _src = new BindingSource
                    {
                        DataSource = SabLogger.Instance.Messages
                    };
                dataGridView1.DataSource = _src;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        void Instance_NewMessage()
        {
            try
            {
                _src.ResetBindings(false);
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }
            catch (Exception ex)
            {
                // would cause loop
            }
        }
    }
}
