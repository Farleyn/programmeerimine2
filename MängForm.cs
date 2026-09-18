using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class MängForm : Form
    {
        private Label aegSilt = new Label { Text = "aeg: 30", Location = new Point(200, 10), AutoSize = true };
        private Label l1 = new Label { Location = new Point(20, 40), AutoSize = true }, l2 = new Label { Location = new Point(20, 70), AutoSize = true };
        private Label l3 = new Label { Location = new Point(20, 100), AutoSize = true }, l4 = new Label { Location = new Point(20, 130), AutoSize = true };
        private NumericUpDown n1 = new NumericUpDown { Location = new Point(180, 38), Width = 80, Enabled = false }, n2 = new NumericUpDown { Location = new Point(180, 68), Width = 80, Enabled = false };
        private NumericUpDown n3 = new NumericUpDown { Location = new Point(180, 98), Width = 80, Enabled = false }, n4 = new NumericUpDown { Location = new Point(180, 128), Width = 80, Enabled = false };
        private Button alusta = new Button { Text = "alusta", Location = new Point(80, 170), Size = new Size(120, 30) };
        private Timer taimer = new Timer { Interval = 1000 };

        private int aeg, v1, v2, v3, v4;
        private Random rand = new Random();

        public MängForm()
        {
            Text = "matemaatika";
            Size = new Size(300, 250);

            alusta.Click += (s, e) => AlustaMängu();
            taimer.Tick += TaimerTiksub;

            Controls.AddRange(new Control[] { aegSilt, l1, n1, l2, n2, l3, n3, l4, n4, alusta });
        }

        private void AlustaMängu()
        {
            int a = rand.Next(1, 20), b = rand.Next(1, 20);
            v1 = a + b; l1.Text = $"{a} + {b} ="; n1.Value = 0; n1.Enabled = true;

            a = rand.Next(10, 20); b = rand.Next(1, a);
            v2 = a - b; l2.Text = $"{a} - {b} ="; n2.Value = 0; n2.Enabled = true;

            a = rand.Next(1, 10); b = rand.Next(1, 10);
            v3 = a * b; l3.Text = $"{a} * {b} ="; n3.Value = 0; n3.Enabled = true;

            b = rand.Next(1, 10); a = b * rand.Next(1, 10);
            v4 = a / b; l4.Text = $"{a} / {b} ="; n4.Value = 0; n4.Enabled = true;

            aeg = 30;
            aegSilt.Text = $"aeg: {aeg}";
            alusta.Enabled = false;
            taimer.Start();
        }

        private void TaimerTiksub(object sender, EventArgs e)
        {
            if (n1.Value == v1 && n2.Value == v2 && n3.Value == v3 && n4.Value == v4)
            {
                taimer.Stop();
                MessageBox.Show("võit!");
                alusta.Enabled = true;
            }
            else if (aeg > 0)
            {
                aeg--;
                aegSilt.Text = $"aeg: {aeg}";
            }
            else
            {
                taimer.Stop();
                MessageBox.Show("aeg läbi!");
                alusta.Enabled = true;
            }
        }
    }
}