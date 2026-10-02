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

            Text = "Menüü";
            ClientSize = new Size(400, 260);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var group = new GroupBox
            {
                Text = "Vali mäng",
                Location = new Point(20, 20),
                Size = new Size(360, 220)
            };

            var nupp1 = new Button
            {
                Text = "1. Pildid",
                Location = new Point(35, 40),
                Size = new Size(290, 40)
            };

            var nupp2 = new Button
            {
                Text = "2. Matemaatika",
                Location = new Point(35, 90),
                Size = new Size(290, 40)
            };

            var nupp3 = new Button
            {
                Text = "3. Paarid",
                Location = new Point(35, 140),
                Size = new Size(290, 40)
            };

            nupp1.Click += (s, e) => new PildidForm().ShowDialog(this);
            nupp2.Click += (s, e) => new MängForm().ShowDialog(this);
            nupp3.Click += (s, e) => new PaaridForm().ShowDialog(this);

            group.Controls.Add(nupp1);
            group.Controls.Add(nupp2);
            group.Controls.Add(nupp3);

            Controls.Add(group);
        }
    }
}