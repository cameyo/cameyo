using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cameyo.OpenSrc.Common
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void SetProgress(int percent)
        {
            progressBar1.Value = percent;
        }

        public int GetProgress()
        {
            return (progressBar1.Value);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= 100)
                progressBar1.Value = 0;
            progressBar1.Value += 10;
        }
    }
}
