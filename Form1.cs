using System;
using System.Drawing;
using System.Windows.Forms;

namespace programmeerimine2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "menüü";
            Size = new Size(300, 220);
            StartPosition = FormStartPosition.CenterScreen;

            var silt = new Label { Text = "vali:", Location = new Point(20, 10), AutoSize = true };
            var nupp1 = new Button { Text = "1. pildid", Location = new Point(20, 35), Size = new Size(240, 35) };
            var nupp2 = new Button { Text = "2. matemaatika", Location = new Point(20, 75), Size = new Size(240, 35) };
            var nupp3 = new Button { Text = "3. paarid", Location = new Point(20, 115), Size = new Size(240, 35) };

            nupp1.Click += (s, e) => new PildidForm().ShowDialog();
            nupp2.Click += (s, e) => new MängForm().ShowDialog();
            nupp3.Click += (s, e) => new PaaridForm().ShowDialog();

            Controls.AddRange(new Control[] { silt, nupp1, nupp2, nupp3 });
        }
    }
}