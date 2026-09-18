using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class PaaridForm : Form
    {
        private TableLayoutPanel tabel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 4,
            BackColor = Color.White,
            Padding = new Padding(2)
        };
        private Label esimene = null, teine = null;
        private Timer taimer = new Timer { Interval = 750 };
        private Random rand = new Random();
        private List<string> märgid = new List<string> { "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z" };

        public PaaridForm()
        {
            Text = "matching game";
            Size = new Size(450, 510);

            var nuppUus = new Button
            {
                Text = "uus mäng",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            nuppUus.Click += (s, e) => UusMäng();

            for (int i = 0; i < 4; i++)
            {
                tabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                tabel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            for (int i = 0; i < 16; i++)
            {
                var sümbol = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Webdings", 36, FontStyle.Bold),
                    BackColor = Color.CornflowerBlue,
                    Margin = new Padding(1)
                };
                sümbol.Click += RuutKlõps;
                tabel.Controls.Add(sümbol);
            }

            taimer.Tick += TaimerTiksub;
            SeaSümbolid();

            Controls.Add(tabel);
            Controls.Add(nuppUus);
        }

        private void UusMäng()
        {
            taimer.Stop();
            esimene = null;
            teine = null;
            SeaSümbolid();
        }

        private void SeaSümbolid()
        {
            var kopeeritud = new List<string>(märgid);
            foreach (Control c in tabel.Controls)
            {
                if (c is Label l && kopeeritud.Count > 0)
                {
                    int idx = rand.Next(kopeeritud.Count);
                    l.Text = kopeeritud[idx];
                    l.ForeColor = l.BackColor;
                    kopeeritud.RemoveAt(idx);
                }
            }
        }

        private void RuutKlõps(object sender, EventArgs e)
        {
            if (taimer.Enabled) return;
            if (sender is Label l && l.ForeColor == Color.Black) return;

            if (sender is Label klõpsatud)
            {
                if (esimene == null)
                {
                    esimene = klõpsatud;
                    esimene.ForeColor = Color.Black;
                    return;
                }

                teine = klõpsatud;
                teine.ForeColor = Color.Black;

                KontrolliVõitu();

                if (esimene.Text == teine.Text)
                {
                    esimene = null;
                    teine = null;
                    return;
                }

                taimer.Start();
            }
        }

        private void TaimerTiksub(object sender, EventArgs e)
        {
            taimer.Stop();
            esimene.ForeColor = esimene.BackColor;
            teine.ForeColor = teine.BackColor;
            esimene = null;
            teine = null;
        }

        private void KontrolliVõitu()
        {
            foreach (Control c in tabel.Controls)
                if (c is Label l && l.ForeColor == l.BackColor) return;

            MessageBox.Show("võit!");
        }
    }
}