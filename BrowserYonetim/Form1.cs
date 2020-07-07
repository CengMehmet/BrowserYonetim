using License;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrowserYonetim
{
    public partial class Form1 : Form
    {
        public string baglanti = "", key = "";

        public Form1()
        {
            InitializeComponent();
            RegistryIslemleri();
        }

        private void RegistryIslemleri()
        {
            string sistemkey = license.CPUSeriNo() + license.HDDserino();
            Registry.CurrentUser.CreateSubKey("PWR");
            RegistryKey PtsReg = Registry.CurrentUser.OpenSubKey("PWR", true);
            try { key = PtsReg.GetValue("key").ToString(); } catch (Exception e) { }
            try { baglanti = PtsReg.GetValue("sql").ToString(); } catch (Exception e) { }
            if (string.IsNullOrEmpty(key))
            {
                try { PtsReg.SetValue("key", "1"); } catch (Exception e) { }
                DialogResult dr = MessageBox.Show("Lisans Hatası", "Lisans", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dr == DialogResult.OK) Environment.Exit(0);
            }
            else
            {
                if (sistemkey != key)
                {
                    DialogResult dr = MessageBox.Show("Lisans Hatası", "Lisans", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (dr == DialogResult.OK) Environment.Exit(0);
                }
            }
            if (string.IsNullOrEmpty(baglanti))
            {
                try { PtsReg.SetValue("sql", "Data Source=.; Initial Catalog=BrowserTakip; User Id=sa; Password=Recep123"); } catch (Exception e) { }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AnasayfaOlustur();
        }

        private List<Panel> UserPanels;

        public void AnasayfaOlustur()
        {
            timer1.Stop();
            while (flowLayoutPanel1.Controls.Count > 0)
            {
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    flowLayoutPanel1.Controls.Remove(control);
                    control.Dispose();
                }
            }

            using (var bdb = new BrowserContext(baglanti))
            {
                if (textBox10.Text == "")
                {
                    var users = bdb.UserSet.Where(u => u.Sinif.Contains(textBox9.Text)).ToList();
                    UserPanels = new List<Panel>();
                    foreach (var user in users)
                    {
                        if (user.Durum == 1 || textBox9.Text != "" || textBox10.Text != "")
                        {
                            Panel puser = new Panel();
                            puser.Size = new Size(flowLayoutPanel1.Size.Width, 100);
                            puser.BackColor = Color.Transparent;
                            puser.Name = user.username;

                            PictureBox pc = new PictureBox();
                            pc.Size = new Size(120, 100);
                            pc.Location = new Point(0, 0);
                            pc.Tag = user.username;
                            pc.MouseDown += detaygoster;
                            pc.Tag = user.UserId;
                            pc.SizeMode = PictureBoxSizeMode.StretchImage;

                            Image image1 = null;
                            using (FileStream stream = new FileStream(user.Durum + ".jpg", FileMode.Open))
                            { image1 = Image.FromStream(stream); }
                            pc.Image = image1;

                            if (user.Izin == 0)
                            {
                                using (FileStream stream = new FileStream("kirmizi.jpg", FileMode.Open))
                                { image1 = Image.FromStream(stream); }
                                pc.Image = image1;
                            }

                            TextBox adtxt = new TextBox();
                            adtxt.ReadOnly = true;
                            adtxt.Location = new Point(130, 0);
                            try { adtxt.Text = user.AdSoyad; } catch (Exception) { }
                            adtxt.Tag = user.username;
                            adtxt.Font = new Font("Arial", 8.0f);
                            adtxt.Size = new Size(puser.Size.Width - 180, 20);

                            RichTextBox urltxt = new RichTextBox();
                            urltxt.ReadOnly = true;
                            urltxt.Location = new Point(130, 22);
                            try { urltxt.Text = bdb.BrowserLogSet.OrderByDescending(o => o.Tarih).FirstOrDefault(l => l.UserId == user.UserId).Adres; } catch (Exception) { }
                            urltxt.Tag = user.username;
                            urltxt.Font = new Font("Arial", 8.0f);
                            urltxt.Size = new Size(puser.Size.Width - 180, 78);
                            UserPanels.Add(puser);

                            puser.Controls.Add(pc);
                            puser.Controls.Add(adtxt);
                            puser.Controls.Add(urltxt);
                            flowLayoutPanel1.Controls.Add(puser);
                        }
                    }
                }
                else
                {
                    var users = bdb.UserSet.Where(u => u.OgrenciNo == textBox10.Text).ToList();
                    UserPanels = new List<Panel>();
                    foreach (var user in users)
                    {
                        if (user.Durum == 1 || textBox9.Text != "" || textBox10.Text != "")
                        {
                            Panel puser = new Panel();
                            puser.Size = new Size(flowLayoutPanel1.Size.Width, 100);
                            puser.BackColor = Color.Transparent;
                            puser.Name = user.username;

                            PictureBox pc = new PictureBox();
                            pc.Size = new Size(120, 100);
                            pc.Location = new Point(0, 0);
                            pc.Tag = user.username;
                            pc.MouseDown += detaygoster;
                            pc.Tag = user.UserId;
                            pc.SizeMode = PictureBoxSizeMode.StretchImage;

                            Image image1 = null;
                            using (FileStream stream = new FileStream(user.Durum + ".jpg", FileMode.Open))
                            { image1 = Image.FromStream(stream); }
                            pc.Image = image1;

                            if (user.Izin == 0)
                            {
                                using (FileStream stream = new FileStream("kirmizi.jpg", FileMode.Open))
                                { image1 = Image.FromStream(stream); }
                                pc.Image = image1;
                            }

                            TextBox adtxt = new TextBox();
                            adtxt.ReadOnly = true;
                            adtxt.Location = new Point(130, 0);
                            try { adtxt.Text = user.AdSoyad; } catch (Exception) { }
                            adtxt.Tag = user.username;
                            adtxt.Font = new Font("Arial", 8.0f);
                            adtxt.Size = new Size(puser.Size.Width - 180, 20);

                            RichTextBox urltxt = new RichTextBox();
                            urltxt.ReadOnly = true;
                            urltxt.Location = new Point(130, 22);
                            try { urltxt.Text = bdb.BrowserLogSet.OrderByDescending(o => o.Tarih).FirstOrDefault(l => l.UserId == user.UserId).Adres; } catch (Exception) { }
                            urltxt.Tag = user.username;
                            urltxt.Font = new Font("Arial", 8.0f);
                            urltxt.Size = new Size(puser.Size.Width - 180, 78);
                            UserPanels.Add(puser);

                            puser.Controls.Add(pc);
                            puser.Controls.Add(adtxt);
                            puser.Controls.Add(urltxt);
                            flowLayoutPanel1.Controls.Add(puser);
                        }
                    }
                }
            }
            timer1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            while (flowLayoutPanel1.Controls.Count > 0)
            {
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    flowLayoutPanel1.Controls.Remove(control);
                    control.Dispose();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //KULLANICI EKLE
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                User ogrenci = bdb.UserSet.FirstOrDefault(o => o.username == textBox4.Text);
                if (ogrenci == null)
                {
                    var user = bdb.UserSet.Add(new User()
                    {
                        username = textBox4.Text,
                        pass = textBox5.Text,
                        AdSoyad = textBox3.Text,
                        Sinif = textBox1.Text,
                        OgrenciNo = textBox2.Text,
                        SonGuncelleme = DateTime.Now,
                        Izin = Convert.ToInt32(radioButton1.Checked),
                        Durum = 0
                    });
                }
                else
                {
                    ogrenci.username = textBox4.Text;
                    ogrenci.pass = textBox5.Text;
                    ogrenci.AdSoyad = textBox3.Text;
                    ogrenci.Sinif = textBox1.Text;
                    ogrenci.OgrenciNo = textBox2.Text;
                    ogrenci.Izin = Convert.ToInt32(radioButton1.Checked);
                }
                bdb.SaveChanges();
                var list = bdb.UserSet.Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                dataGridView2.DataSource = list;
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    string username = row.Cells[1].Value.ToString();
                    using (var bdb = new BrowserContext(baglanti))
                    {
                        User ogrenci = bdb.UserSet.FirstOrDefault(o => o.username == username);
                        if (ogrenci == null) return;
                        labelid.Text = ogrenci.UserId.ToString();
                        textBox4.Text = ogrenci.username;
                        textBox5.Text = ogrenci.pass;
                        textBox3.Text = ogrenci.AdSoyad;
                        textBox1.Text = ogrenci.Sinif;
                        textBox2.Text = ogrenci.OgrenciNo;
                        radioButton1.Checked = Convert.ToBoolean(ogrenci.Izin);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e) //KULLANICI GÜNCELLE
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                int id = Convert.ToInt32(labelid.Text);
                var ogrenci = bdb.UserSet.FirstOrDefault(u => u.UserId == id);
                if (ogrenci == null) return;
                ogrenci.username = textBox4.Text;
                ogrenci.pass = textBox5.Text;
                ogrenci.AdSoyad = textBox3.Text;
                ogrenci.Sinif = textBox1.Text;
                ogrenci.OgrenciNo = textBox2.Text;
                ogrenci.Izin = Convert.ToInt32(radioButton1.Checked);
                bdb.SaveChanges();
                var list = bdb.UserSet.Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                dataGridView2.DataSource = list;
            }
        }

        private void pictureBoxaboneler_Click(object sender, EventArgs e) //KULLANICILAR SEKMESİ
        {
            kullaniciGetir();
        }

        private void kullaniciGetir()
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                var list = bdb.UserSet.Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                dataGridView2.DataSource = list;
                tabControl1.SelectedIndex = 1;
            }
        }

        private void button4_Click(object sender, EventArgs e) //TARİHE GÖRE FİLTRELE
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                var list = bdb.BrowserLogSet.Where(w => w.Tarih > dateTimePicker1.Value.Date && w.Tarih < dateTimePicker2.Value.Date).Select(s => new { s.User.AdSoyad, s.Adres, s.Tarih }).ToList();
                dataGridView3.DataSource = list;
            }
        }

        private void pictureBoxkayitlar_Click(object sender, EventArgs e) //KAYITLAR BUTONU
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                var list = bdb.BrowserLogSet.Where(w => w.Tarih > dateTimePicker1.Value.Date && w.Tarih < dateTimePicker2.Value.Date).Select(s => new { s.User.AdSoyad, s.Adres, s.Tarih }).ToList();
                dataGridView3.DataSource = list;
                tabControl1.SelectedIndex = 2;
            }
        }

        private void button2_Click(object sender, EventArgs e) //OGRENCI NUMARASINA GORE FILTRELE
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                User ogrenci = bdb.UserSet.FirstOrDefault(o => o.OgrenciNo == textBox6.Text);
                if (ogrenci == null) return;
                var list = bdb.BrowserLogSet.Where(w => w.UserId == ogrenci.UserId && w.Tarih > dateTimePicker1.Value.Date && w.Tarih < dateTimePicker2.Value.Date).Select(s => new { s.User.AdSoyad, s.Adres, s.Tarih }).ToList();
                dataGridView3.DataSource = list;
                tabControl1.SelectedIndex = 2;
            }
        }

        private void button3_Click(object sender, EventArgs e) //KULLANICI ADINA GÖRE FİLTRELE
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                User ogrenci = bdb.UserSet.FirstOrDefault(o => o.username == textBox7.Text);
                if (ogrenci == null) return;
                var list = bdb.BrowserLogSet.Where(w => w.UserId == ogrenci.UserId && w.Tarih > dateTimePicker1.Value.Date && w.Tarih < dateTimePicker2.Value.Date).Select(s => new { s.User.AdSoyad, s.Adres, s.Tarih }).ToList();
                dataGridView3.DataSource = list;
                tabControl1.SelectedIndex = 2;
            }
        }

        private void pictureBoxcikis_Click(object sender, EventArgs e) //ÇIKIŞ BUTONU
        {
            Environment.Exit(1);
        }

        private void pictureBoxkaraliste_Click(object sender, EventArgs e)// KARALİSTE BUTONU
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                var list = bdb.UserSet.Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                dataGridView4.DataSource = list;
                tabControl1.SelectedIndex = 3;
            }
        }

        private void dataGridView4_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    string username = row.Cells[1].Value.ToString();
                    using (var bdb = new BrowserContext(baglanti))
                    {
                        User ogrenci = bdb.UserSet.FirstOrDefault(o => o.username == username);
                        if (ogrenci == null) return;
                        if (ogrenci.Izin == 0) ogrenci.Izin = 1; else ogrenci.Izin = 0;
                        bdb.SaveChanges();
                        var list = bdb.UserSet.Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                        dataGridView4.DataSource = list;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) //Karalistede öğrenci numarasına göre arama
        {
            using (var bdb = new BrowserContext(baglanti))
            {
                var list = bdb.UserSet.Where(o => o.OgrenciNo == textBox8.Text).Select(s => new { s.OgrenciNo, s.username, s.AdSoyad, s.Sinif, s.Izin }).ToList();
                dataGridView4.DataSource = list;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelsaat.Text = DateTime.Now.ToString("HH:mm");
            labeltarih.Text = DateTime.Now.ToString("dd-MM-yyyy");

            using (var bdb = new BrowserContext(baglanti))
            {
                var users = bdb.UserSet.Where(u => u.Sinif.Contains(textBox9.Text)).ToList();
                if (users == null) return;
                foreach (var user in users)
                {
                    TimeSpan ts = DateTime.Now.Subtract(user.SonGuncelleme);

                    if (ts.TotalSeconds > 6) { user.Durum = 0; }
                    else
                    {
                        Panel panel = null;
                        try { panel = UserPanels.FirstOrDefault(p => p.Name == user.username); } catch (Exception) { }
                        if (panel == null) return;
                        foreach (var item in panel.Controls)
                        {
                            if (item is RichTextBox)
                            {
                                try { ((RichTextBox)item).Text = bdb.BrowserLogSet.OrderByDescending(o => o.Tarih).FirstOrDefault(l => l.UserId == user.UserId).Adres; } catch (Exception) { }
                            }
                            else if (item is PictureBox)
                            {
                                Image image1 = null;
                                using (FileStream stream = new FileStream(user.Durum + ".jpg", FileMode.Open))
                                { image1 = Image.FromStream(stream); }
                                ((PictureBox)item).Image = image1;

                                if (user.Izin == 0)
                                {
                                    using (FileStream stream = new FileStream("kirmizi.jpg", FileMode.Open))
                                    { image1 = Image.FromStream(stream); }
                                    ((PictureBox)item).Image = image1;
                                }
                            }
                        }
                    }
                    bdb.SaveChanges();
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void pictureBoxurunExcelSenk_Click(object sender, EventArgs e)
        {
            excelsenkronizasyon excel = new excelsenkronizasyon();
            excel.ShowDialog();
        }

        private void buttonKullaniciSil_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows[0].Cells[0].Value != null)
            {
                string seciliogrencino = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                DialogResult res = MessageBox.Show(seciliogrencino + " numaralı öğrenci bilgileri silinecektir.Onaylıyor musunuz?", "Kullanıcı Silme", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    using (var bdb = new BrowserContext(baglanti))
                    {
                        var user = bdb.UserSet.FirstOrDefault(u => u.OgrenciNo == seciliogrencino);
                        if (user != null) { bdb.UserSet.Remove(user); bdb.SaveChanges(); }
                    }
                }
                else
                {
                    MessageBox.Show("İşlem İptal Edildi.", "İşlem İptal");
                }
                kullaniciGetir();
            }
            else
            {
                string seciliusername = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                DialogResult res = MessageBox.Show(seciliusername + " kullanıcı adına sahip öğrenci bilgileri silinecektir.Onaylıyor musunuz?", "Kullanıcı Silme", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    using (var bdb = new BrowserContext(baglanti))
                    {
                        var user = bdb.UserSet.FirstOrDefault(u => u.username == seciliusername);
                        if (user != null) { bdb.UserSet.Remove(user); bdb.SaveChanges(); }
                    }
                }
                else
                {
                    MessageBox.Show("İşlem İptal Edildi.", "İşlem İptal");
                }
                kullaniciGetir();
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }

        public void detaygoster(object sender, EventArgs e)
        {
            int userid = Convert.ToInt32(((PictureBox)sender).Tag.ToString());
            using (var bdb = new BrowserContext(baglanti))
            {
                var user = bdb.UserSet.FirstOrDefault(u => u.UserId == userid);
                if (user == null) return;
                textBoxadsoyad.Text = user.AdSoyad;
                textBoxkullaniciadi.Text = user.username;
                textBoxogrencino.Text = user.OgrenciNo;
                textBoxsinifi.Text = user.Sinif;
                var list = bdb.BrowserLogSet.Where(b => b.UserId == user.UserId).OrderByDescending(o => o.Tarih).Select(s => new { s.Adres, s.Tarih }).ToList();
                dataGridView1.DataSource = list;
            }
        }
    }
}