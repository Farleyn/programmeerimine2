using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class PaaridForm : Form
    {
        private readonly Label käigudSilt = new Label();
        private readonly Label paaridSilt = new Label();

        private readonly Button vihje = new Button();
        private readonly Button nuppUus = new Button();

        private readonly Timer taimer = new Timer();
        private readonly Timer vihjeTaimer = new Timer();

        private readonly Random rand = new Random();

        private readonly string[] märgid =
        {
            "🍎", "🍎",
            "🍌", "🍌",
            "🍒", "🍒",
            "🍋", "🍋",
            "🍉", "🍉",
            "⭐", "⭐",
            "❤️", "❤️",
            "🐱", "🐱"
        };

        private readonly Label[] ruudud = new Label[16];

        private Label esimene;
        private Label teine;
        private Label vihjeÜks;
        private Label vihjeKaks;

        private int käigud;
        private int leitudPaarid;

        public PaaridForm()
        {
            InitializeComponent();

            Text = "Paarid";
            ClientSize = new Size(440, 485);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            käigudSilt.Text = "Käigud: 0";
            käigudSilt.Location = new Point(15, 15);
            käigudSilt.Size = new Size(90, 25);

            paaridSilt.Text = "Paarid: 0/8";
            paaridSilt.Location = new Point(110, 15);
            paaridSilt.Size = new Size(100, 25);

            vihje.Text = "Vihje";
            vihje.Location = new Point(255, 10);
            vihje.Size = new Size(80, 30);

            nuppUus.Text = "Uus mäng";
            nuppUus.Location = new Point(340, 10);
            nuppUus.Size = new Size(100, 30);

            for (int i = 0; i < 16; i++)
            {
                int rida = i / 4;
                int veerg = i % 4;

                var ruut = new Label
                {
                    Location = new Point(15 + veerg * 105, 55 + rida * 105),
                    Size = new Size(95, 95),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Emoji", 28, FontStyle.Regular),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = SystemColors.Control,
                    ForeColor = SystemColors.Control,
                    Cursor = Cursors.Hand
                };

                ruut.Click += RuutKlõps;

                ruudud[i] = ruut;

                Controls.Add(ruut);
            }

            Controls.Add(käigudSilt);
            Controls.Add(paaridSilt);
            Controls.Add(vihje);
            Controls.Add(nuppUus);

            taimer.Interval = 750;
            taimer.Tick += TaimerTiksub;

            vihjeTaimer.Interval = 1000;
            vihjeTaimer.Tick += PeidaVihje;

            nuppUus.Click += (s, e) => UusMäng();
            vihje.Click += (s, e) => NäitaVihjet();

            UusMäng();
        }

        private void UusMäng()
        {
            taimer.Stop();
            vihjeTaimer.Stop();

            esimene = null;
            teine = null;
            vihjeÜks = null;
            vihjeKaks = null;

            käigud = 0;
            leitudPaarid = 0;

            käigudSilt.Text = "Käigud: 0";
            paaridSilt.Text = "Paarid: 0/8";

            var segatud = new List<string>(märgid);

            foreach (Label ruut in ruudud)
            {
                int indeks = rand.Next(segatud.Count);

                ruut.Text = segatud[indeks];
                ruut.ForeColor = ruut.BackColor;
                ruut.Enabled = true;

                segatud.RemoveAt(indeks);
            }
        }

        private void RuutKlõps(object sender, EventArgs e)
        {
            if (taimer.Enabled || vihjeTaimer.Enabled)
                return;

            if (sender is not Label ruut)
                return;

            if (!ruut.Enabled)
                return;

            if (esimene == ruut)
                return;

            ruut.ForeColor = Color.Black;

            if (esimene == null)
            {
                esimene = ruut;
                return;
            }

            teine = ruut;

            käigud++;
            käigudSilt.Text = $"Käigud: {käigud}";

            if (esimene.Text == teine.Text)
            {
                esimene.Enabled = false;
                teine.Enabled = false;

                leitudPaarid++;
                paaridSilt.Text = $"Paarid: {leitudPaarid}/8";

                esimene = null;
                teine = null;

                if (leitudPaarid == 8)
                    MessageBox.Show($"Võit! Käike: {käigud}", "Paarid");

                return;
            }

            taimer.Start();
        }

        private void TaimerTiksub(object sender, EventArgs e)
        {
            taimer.Stop();

            if (esimene != null)
                esimene.ForeColor = esimene.BackColor;

            if (teine != null)
                teine.ForeColor = teine.BackColor;

            esimene = null;
            teine = null;
        }

        private void NäitaVihjet()
        {
            if (taimer.Enabled || vihjeTaimer.Enabled)
                return;

            Label leitud1 = null;
            Label leitud2 = null;

            for (int i = 0; i < ruudud.Length; i++)
            {
                if (!ruudud[i].Enabled)
                    continue;

                for (int j = i + 1; j < ruudud.Length; j++)
                {
                    if (!ruudud[j].Enabled)
                        continue;

                    if (ruudud[i].Text == ruudud[j].Text)
                    {
                        leitud1 = ruudud[i];
                        leitud2 = ruudud[j];
                        break;
                    }
                }

                if (leitud1 != null)
                    break;
            }

            if (leitud1 == null)
                return;

            vihjeÜks = leitud1;
            vihjeKaks = leitud2;

            vihjeÜks.ForeColor = Color.Black;
            vihjeKaks.ForeColor = Color.Black;

            vihjeTaimer.Start();
        }

        private void PeidaVihje(object sender, EventArgs e)
        {
            vihjeTaimer.Stop();

            if (vihjeÜks != null && vihjeÜks.Enabled)
                vihjeÜks.ForeColor = vihjeÜks.BackColor;

            if (vihjeKaks != null && vihjeKaks.Enabled)
                vihjeKaks.ForeColor = vihjeKaks.BackColor;

            vihjeÜks = null;
            vihjeKaks = null;
        }
    }
}