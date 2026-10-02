using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace programmeerimine2
{
    public partial class PildidForm : Form
    {
        private readonly PictureBox pilt = new PictureBox();
        private readonly CheckBox venita = new CheckBox();
        private readonly Label loendur = new Label();

        private readonly Timer taimer = new Timer();

        private string[] failid = Array.Empty<string>();
        private int indeks;

        public PildidForm()
        {
            InitializeComponent();

            Text = "Pildid";
            ClientSize = new Size(885, 595);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            venita.Text = "Venita";
            venita.Location = new Point(15, 15);
            venita.Size = new Size(70, 25);

            loendur.Text = "0 / 0";
            loendur.Location = new Point(95, 16);
            loendur.Size = new Size(70, 20);

            var btnAva = new Button
            {
                Text = "Ava",
                Location = new Point(175, 10),
                Size = new Size(75, 30)
            };

            var btnEelmine = new Button
            {
                Text = "Eelmine",
                Location = new Point(255, 10),
                Size = new Size(85, 30)
            };

            var btnJärgmine = new Button
            {
                Text = "Järgmine",
                Location = new Point(345, 10),
                Size = new Size(90, 30)
            };

            var btnSlaidid = new Button
            {
                Text = "Slaidid",
                Location = new Point(440, 10),
                Size = new Size(80, 30)
            };

            var btnSalvesta = new Button
            {
                Text = "Salvesta",
                Location = new Point(525, 10),
                Size = new Size(85, 30)
            };

            var btnTaust = new Button
            {
                Text = "Taust",
                Location = new Point(615, 10),
                Size = new Size(75, 30)
            };

            var btnPuhasta = new Button
            {
                Text = "Puhasta",
                Location = new Point(695, 10),
                Size = new Size(80, 30)
            };

            var btnSulge = new Button
            {
                Text = "Sulge",
                Location = new Point(780, 10),
                Size = new Size(90, 30)
            };

            pilt.Location = new Point(15, 55);
            pilt.Size = new Size(855, 520);
            pilt.BorderStyle = BorderStyle.FixedSingle;
            pilt.BackColor = Color.White;
            pilt.SizeMode = PictureBoxSizeMode.Zoom;

            btnAva.Click += AvaPilt;
            btnEelmine.Click += (s, e) => NäitaEelmist();
            btnJärgmine.Click += (s, e) => NäitaJärgmist();
            btnSlaidid.Click += AlustaSlaidid;
            btnSalvesta.Click += SalvestaPilt;
            btnTaust.Click += MuudaVärv;
            btnPuhasta.Click += (s, e) => PuhastaPilt();
            btnSulge.Click += (s, e) => Close();

            venita.CheckedChanged += (s, e) =>
            {
                pilt.SizeMode = venita.Checked
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.Zoom;
            };

            taimer.Interval = 2000;
            taimer.Tick += SlaidTiksub;

            Controls.Add(venita);
            Controls.Add(loendur);

            Controls.Add(btnAva);
            Controls.Add(btnEelmine);
            Controls.Add(btnJärgmine);
            Controls.Add(btnSlaidid);
            Controls.Add(btnSalvesta);
            Controls.Add(btnTaust);
            Controls.Add(btnPuhasta);
            Controls.Add(btnSulge);

            Controls.Add(pilt);
        }

        private void AvaPilt(object sender, EventArgs e)
        {
            using var dialoog = new OpenFileDialog
            {
                Filter = "Pildid|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (dialoog.ShowDialog() != DialogResult.OK)
                return;

            taimer.Stop();

            LaePilt(dialoog.FileName);

            failid = new[] { dialoog.FileName };
            indeks = 0;

            loendur.Text = "1 / 1";
        }

        private void LaePilt(string fail)
        {
            try
            {
                using var temp = Image.FromFile(fail);
                var uus = new Bitmap(temp);

                pilt.Image?.Dispose();
                pilt.Image = uus;
            }
            catch
            {
                MessageBox.Show("Pildi avamine ebaõnnestus.", "Pildid");
            }
        }

        private void PuhastaPilt()
        {
            taimer.Stop();

            pilt.Image?.Dispose();
            pilt.Image = null;

            failid = Array.Empty<string>();
            indeks = 0;

            loendur.Text = "0 / 0";
        }

        private void MuudaVärv(object sender, EventArgs e)
        {
            using var dialoog = new ColorDialog();

            if (dialoog.ShowDialog() == DialogResult.OK)
                pilt.BackColor = dialoog.Color;
        }

        private void SalvestaPilt(object sender, EventArgs e)
        {
            if (pilt.Image == null)
                return;

            using var dialoog = new SaveFileDialog
            {
                Filter = "PNG|*.png|JPG|*.jpg"
            };

            if (dialoog.ShowDialog() != DialogResult.OK)
                return;

            pilt.Image.Save(dialoog.FileName);
        }

        private void AlustaSlaidid(object sender, EventArgs e)
        {
            using var dialoog = new FolderBrowserDialog();

            if (dialoog.ShowDialog() != DialogResult.OK)
                return;

            failid = Directory.GetFiles(dialoog.SelectedPath);

            Array.Sort(failid, StringComparer.OrdinalIgnoreCase);

            failid = Array.FindAll(
                failid,
                fail =>
                    fail.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    fail.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    fail.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    fail.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    fail.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
            );

            if (failid.Length == 0)
            {
                MessageBox.Show("Kaustas pole sobivaid pilte.", "Pildid");
                return;
            }

            indeks = 0;

            NäitaJärgmist();

            taimer.Start();
        }

        private void SlaidTiksub(object sender, EventArgs e)
        {
            NäitaJärgmist();
        }

        private void NäitaJärgmist()
        {
            if (failid.Length == 0)
                return;

            LaePilt(failid[indeks]);

            loendur.Text = $"{indeks + 1} / {failid.Length}";

            indeks++;

            if (indeks >= failid.Length)
                indeks = 0;
        }

        private void NäitaEelmist()
        {
            if (failid.Length == 0)
                return;

            indeks--;

            if (indeks < 0)
                indeks = failid.Length - 1;

            LaePilt(failid[indeks]);

            loendur.Text = $"{indeks + 1} / {failid.Length}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            taimer.Stop();
            pilt.Image?.Dispose();

            base.OnFormClosed(e);
        }
    }
}