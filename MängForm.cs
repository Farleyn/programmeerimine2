using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class MängForm : Form
    {
        private readonly ComboBox raskus = new ComboBox();
        private readonly Label aegSilt = new Label();
        private readonly Label skoorSilt = new Label();

        private readonly Label[] küsimused = new Label[4];
        private readonly NumericUpDown[] vastused = new NumericUpDown[4];

        private readonly Button kontrolli = new Button();
        private readonly Button alusta = new Button();

        private readonly Timer taimer = new Timer();
        private readonly Random rand = new Random();

        private readonly int[] õiged = new int[4];

        private int aeg;
        private int skoor;

        public MängForm()
        {
            InitializeComponent();

            Text = "Matemaatika";
            ClientSize = new Size(430, 340);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var seaded = new GroupBox
            {
                Text = "Seaded",
                Location = new Point(15, 15),
                Size = new Size(400, 70)
            };

            var raskusSilt = new Label
            {
                Text = "Raskus:",
                Location = new Point(20, 27),
                AutoSize = true
            };

            raskus.Location = new Point(75, 23);
            raskus.Size = new Size(100, 25);
            raskus.DropDownStyle = ComboBoxStyle.DropDownList;

            skoorSilt.Text = "Tulemus: 0";
            skoorSilt.Location = new Point(200, 27);
            skoorSilt.Size = new Size(90, 20);

            aegSilt.Text = "Aeg: 30";
            aegSilt.Location = new Point(300, 27);
            aegSilt.Size = new Size(70, 20);

            seaded.Controls.Add(raskusSilt);
            seaded.Controls.Add(raskus);
            seaded.Controls.Add(skoorSilt);
            seaded.Controls.Add(aegSilt);

            var ülesanded = new GroupBox
            {
                Text = "Ülesanded",
                Location = new Point(15, 95),
                Size = new Size(400, 175)
            };

            for (int i = 0; i < 4; i++)
            {
                küsimused[i] = new Label
                {
                    Location = new Point(25, 30 + i * 32),
                    Size = new Size(150, 25)
                };

                vastused[i] = new NumericUpDown
                {
                    Location = new Point(220, 27 + i * 32),
                    Size = new Size(100, 25),
                    Maximum = 1000,
                    Enabled = false
                };

                ülesanded.Controls.Add(küsimused[i]);
                ülesanded.Controls.Add(vastused[i]);
            }

            kontrolli.Text = "Kontrolli";
            kontrolli.Location = new Point(120, 285);
            kontrolli.Size = new Size(100, 35);
            kontrolli.Enabled = false;

            alusta.Text = "Uus mäng";
            alusta.Location = new Point(230, 285);
            alusta.Size = new Size(100, 35);

            raskus.Items.Add("Lihtne");
            raskus.Items.Add("Raske");
            raskus.SelectedIndex = 0;

            alusta.Click += (s, e) => AlustaMängu();
            kontrolli.Click += (s, e) => KontrolliVastused();

            taimer.Interval = 1000;
            taimer.Tick += TaimerTiksub;

            Controls.Add(seaded);
            Controls.Add(ülesanded);
            Controls.Add(kontrolli);
            Controls.Add(alusta);
        }

        private void AlustaMängu()
        {
            bool raske = raskus.SelectedIndex == 1;

            int max = raske ? 50 : 20;
            int maxM = raske ? 15 : 10;

            int a = rand.Next(1, max);
            int b = rand.Next(1, max);

            õiged[0] = a + b;
            küsimused[0].Text = $"{a} + {b} =";

            a = rand.Next(Math.Max(2, max / 2), max);
            b = rand.Next(1, a);

            õiged[1] = a - b;
            küsimused[1].Text = $"{a} - {b} =";

            a = rand.Next(2, maxM);
            b = rand.Next(2, maxM);

            õiged[2] = a * b;
            küsimused[2].Text = $"{a} * {b} =";

            b = rand.Next(2, maxM);
            a = b * rand.Next(2, maxM);

            õiged[3] = a / b;
            küsimused[3].Text = $"{a} / {b} =";

            for (int i = 0; i < 4; i++)
            {
                vastused[i].Value = 0;
                vastused[i].Enabled = true;
            }

            aeg = raske ? 20 : 30;
            aegSilt.Text = $"Aeg: {aeg}";

            kontrolli.Enabled = true;
            alusta.Enabled = false;
            raskus.Enabled = false;

            taimer.Start();

            vastused[0].Focus();
        }

        private void KontrolliVastused()
        {
            for (int i = 0; i < 4; i++)
            {
                if (vastused[i].Value != õiged[i])
                {
                    MessageBox.Show("Mõni vastus on vale.", "Matemaatika");
                    vastused[i].Focus();
                    return;
                }
            }

            skoor++;
            skoorSilt.Text = $"Tulemus: {skoor}";

            taimer.Stop();

            MessageBox.Show("Võit!", "Matemaatika");

            LõpetaMäng();
        }

        private void TaimerTiksub(object sender, EventArgs e)
        {
            aeg--;

            aegSilt.Text = $"Aeg: {aeg}";

            if (aeg <= 0)
            {
                taimer.Stop();

                MessageBox.Show("Aeg läbi!", "Matemaatika");

                LõpetaMäng();
            }
        }

        private void LõpetaMäng()
        {
            alusta.Enabled = true;
            raskus.Enabled = true;
            kontrolli.Enabled = false;

            for (int i = 0; i < 4; i++)
                vastused[i].Enabled = false;
        }
    }
}