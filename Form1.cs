using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

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

    public class PildidForm : Form
    {
        private PictureBox pilt = new PictureBox { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
        private CheckBox venita = new CheckBox { Text = "venita", AutoSize = true };
        private Timer taimer = new Timer { Interval = 2000 };
        private string[] failid;
        private int indeks = 0;

        public PildidForm()
        {
            Text = "pildid";
            Size = new Size(600, 450);

            var tabel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 2 };
            tabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tabel.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tabel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));

            venita.CheckedChanged += (s, e) => pilt.SizeMode = venita.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;

            var paneel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };

            var btnAva = new Button { Text = "ava", AutoSize = true };
            var btnPuhasta = new Button { Text = "puhasta", AutoSize = true };
            var btnVärv = new Button { Text = "värv", AutoSize = true };
            var btnSalvesta = new Button { Text = "salvesta", AutoSize = true };
            var btnSlaidid = new Button { Text = "slaidid", AutoSize = true };
            var btnSulge = new Button { Text = "sulge", AutoSize = true };

            btnAva.Click += AvaPilt;
            btnPuhasta.Click += (s, e) => pilt.Image = null;
            btnVärv.Click += MuudaVärv;
            btnSalvesta.Click += SalvestaPilt;
            btnSlaidid.Click += AlustaSlaidid;
            btnSulge.Click += (s, e) => Close();
            taimer.Tick += SlaidTiksub;

            paneel.Controls.AddRange(new Control[] { btnSulge, btnSlaidid, btnSalvesta, btnVärv, btnPuhasta, btnAva });
            tabel.Controls.Add(pilt, 0, 0);
            tabel.SetColumnSpan(pilt, 2);
            tabel.Controls.Add(venita, 0, 1);
            tabel.Controls.Add(paneel, 1, 1);

            Controls.Add(tabel);
        }

        private void AvaPilt(object sender, EventArgs e)
        {
            using (var dialoog = new OpenFileDialog { Filter = "pildid|*.jpg;*.png" })
                if (dialoog.ShowDialog() == DialogResult.OK) pilt.Load(dialoog.FileName);
        }

        private void MuudaVärv(object sender, EventArgs e)
        {
            using (var dialoog = new ColorDialog())
                if (dialoog.ShowDialog() == DialogResult.OK) pilt.BackColor = dialoog.Color;
        }

        private void SalvestaPilt(object sender, EventArgs e)
        {
            if (pilt.Image == null) return;
            using (var dialoog = new SaveFileDialog { Filter = "png|*.png|jpg|*.jpg" })
                if (dialoog.ShowDialog() == DialogResult.OK) pilt.Image.Save(dialoog.FileName);
        }

        private void AlustaSlaidid(object sender, EventArgs e)
        {
            using (var dialoog = new FolderBrowserDialog())
            {
                if (dialoog.ShowDialog() == DialogResult.OK)
                {
                    failid = Directory.GetFiles(dialoog.SelectedPath, "*.jpg");
                    if (failid.Length > 0) { indeks = 0; taimer.Start(); }
                }
            }
        }

        private void SlaidTiksub(object sender, EventArgs e)
        {
            if (failid == null || failid.Length == 0) return;
            pilt.Load(failid[indeks]);
            indeks = (indeks + 1) % failid.Length;
        }
    }

    public class MängForm : Form
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

    public class PaaridForm : Form
    {
        private TableLayoutPanel tabel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, BackColor = Color.LightBlue };
        private Label esimene = null, teine = null;
        private Timer taimer = new Timer { Interval = 500 };
        private Random rand = new Random();
        private List<string> märgid = new List<string> { "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z" };

        public PaaridForm()
        {
            Text = "paarid";
            Size = new Size(400, 400);

            for (int i = 0; i < 4; i++)
            {
                tabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                tabel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            for (int i = 0; i < 16; i++)
            {
                var sümbol = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Webdings", 30) };
                sümbol.Click += RuutKlõps;
                tabel.Controls.Add(sümbol);
            }

            taimer.Tick += TaimerTiksub;
            SeaSümbolid();
            Controls.Add(tabel);
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