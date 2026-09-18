using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class PildidForm : Form
    {
        private PictureBox pilt = new PictureBox { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
        private CheckBox venita = new CheckBox { Text = "venita", AutoSize = true };
        private Timer taimer = new Timer { Interval = 2000 };
        private string[] failid;
        private int indeks = 0;

        public PildidForm()
        {
            Text = "pildid";
            Size = new Size(800, 600);

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
}