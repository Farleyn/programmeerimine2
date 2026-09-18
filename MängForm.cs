using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class MängForm : Form
    {
        private ComboBox raskus = new ComboBox { Location = new Point(20, 10), Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
        private Label aegSilt = new Label { Text = "aeg: 30", Location = new Point(180, 13), AutoSize = true };
        private Label l1 = new Label { Location = new Point(20, 45), AutoSize = true }, l2 = new Label { Location = new Point(20, 75), AutoSize = true };
        private Label l3 = new Label { Location = new Point(20, 105), AutoSize = true }, l4 = new Label { Location = new Point(20, 135), AutoSize = true };
        private NumericUpDown n1 = new NumericUpDown { Location = new Point(180, 43), Width = 80, Maximum = 1000, Enabled = false };
        private NumericUpDown n2 = new NumericUpDown { Location = new Point(180, 73), Width = 80, Maximum = 1000, Enabled = false };
        private NumericUpDown n3 = new NumericUpDown { Location = new Point(180, 103), Width = 80, Maximum = 1000, Enabled = false };
        private NumericUpDown n4 = new NumericUpDown { Location = new Point(180, 133), Width = 80, Maximum = 1000, Enabled = false };
        private Button alusta = new Button { Text = "alusta", Location = new Point(80, 175), Size = new Size(120, 30) };
        private Timer taimer = new Timer { Interval = 1000 };

        private int aeg, v1, v2, v3, v4;
        private Random rand = new Random();

        public MängForm()
        {
            Text = "matemaatika";
            Size = new Size(300, 260);

            raskus.Items.AddRange(new object[] { "lihtne", "raske" });
            raskus.SelectedIndex = 0;

            alusta.Click += (s, e) => AlustaMängu();
            taimer.Tick += TaimerTiksub;

            Controls.AddRange(new Control[] { raskus, aegSilt, l1, n1, l2, n2, l3, n3, l4, n4, alusta });
        }

        private void AlustaMängu()
        {
            bool onRaske = raskus.SelectedIndex == 1;
            int max = onRaske ? 50 : 20;
            int maxM = onRaske ? 15 : 10;

            int a = rand.Next(1, max), b = rand.Next(1, max);
            v1 = a + b; l1.Text = $"{a} + {b} ="; n1.Value = 0; n1.Enabled = true;

            a = rand.Next(max / 2, max); b = rand.Next(1, a);
            v2 = a - b; l2.Text = $"{a} - {b} ="; n2.Value = 0; n2.Enabled = true;

            a = rand.Next(2, maxM); b = rand.Next(2, maxM);
            v3 = a * b; l3.Text = $"{a} * {b} ="; n3.Value = 0; n3.Enabled = true;

            b = rand.Next(2, maxM); a = b * rand.Next(2, maxM);
            v4 = a / b; l4.Text = $"{a} / {b} ="; n4.Value = 0; n4.Enabled = true;

            aeg = onRaske ? 20 : 30;
            aegSilt.Text = $"aeg: {aeg}";
            alusta.Enabled = false;
            raskus.Enabled = false;
            taimer.Start();
        }

        private void TaimerTiksub(object sender, EventArgs e)
        {
            if (n1.Value == v1 && n2.Value == v2 && n3.Value == v3 && n4.Value == v4)
            {
                taimer.Stop();
                MessageBox.Show("võit!");
                LõpetaMäng();
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
                LõpetaMäng();
            }
        }

        private void LõpetaMäng()
        {
            alusta.Enabled = true;
            raskus.Enabled = true;
            n1.Enabled = false;
            n2.Enabled = false;
            n3.Enabled = false;
            n4.Enabled = false;
        }
    }
}