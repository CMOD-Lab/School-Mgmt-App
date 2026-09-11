using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SchoolManagementApplciation
{
    public partial class WelcomeScreen : Form
    {
        public WelcomeScreen()
        {
            InitializeComponent();

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Updated: Replaced SQL Server EXEC dbo. syntax with PostgreSQL CALL syntax
            new SqlControl().ExecProc("CALL update_fees()");
            this.Close();
        }
    }
}
