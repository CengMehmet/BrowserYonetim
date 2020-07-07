using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BrowserYonetim
{
    public partial class excelsenkronizasyon : Form
    {
        private OpenFileDialog openfileDialog1 = new OpenFileDialog();
        private SqlConnection sqlbag;
        private SqlCommand k;
        private SqlDataReader rd;
        private string sqlbagstring;
        private string baglanti = "", key = "";

        public excelsenkronizasyon()
        {
            InitializeComponent();
            RegistryIslemleri();
        }

        private void RegistryIslemleri()
        {
            baglanti = ((Form1)Application.OpenForms["Form1"]).baglanti;
        }

        private void buttonExcelGoster_Click(object sender, EventArgs e)
        {
            excelgoster();
        }

        private void excelgoster()
        {
            string dosya_yolu = "ogrencitablo.xls";

            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + dosya_yolu + "; Extended Properties = Excel 12.0");
            baglanti.Open();
            string sorgu = "select * from [Sayfa1$] ";
            OleDbDataAdapter data_adaptor = new OleDbDataAdapter(sorgu, baglanti);
            baglanti.Close();

            System.Data.DataTable dt = new System.Data.DataTable();
            data_adaptor.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        public void exceldenguncelle()
        {
            for (int l = 0; l < dataGridView1.Rows.Count - 1; l++)
            {
                using (var bdb = new BrowserContext(baglanti))
                {
                    string ogrencino = dataGridView1.Rows[l].Cells[0].Value.ToString();
                    User ogrenci = bdb.UserSet.FirstOrDefault(o => o.OgrenciNo == ogrencino);
                    if (ogrenci == null)
                    {
                        var user = bdb.UserSet.Add(new User()
                        {
                            username = dataGridView1.Rows[l].Cells[3].Value.ToString(),
                            pass = dataGridView1.Rows[l].Cells[4].Value.ToString(),
                            AdSoyad = dataGridView1.Rows[l].Cells[1].Value.ToString(),
                            Sinif = dataGridView1.Rows[l].Cells[2].Value.ToString(),
                            OgrenciNo = dataGridView1.Rows[l].Cells[0].Value.ToString(),
                            SonGuncelleme = DateTime.Now,
                            Izin = Convert.ToInt32(dataGridView1.Rows[l].Cells[5].Value.ToString()),
                            Durum = 0
                        });
                    }
                    else
                    {
                        ogrenci.username = dataGridView1.Rows[l].Cells[3].Value.ToString();
                        ogrenci.pass = dataGridView1.Rows[l].Cells[4].Value.ToString();
                        ogrenci.AdSoyad = dataGridView1.Rows[l].Cells[1].Value.ToString();
                        ogrenci.OgrenciNo = dataGridView1.Rows[l].Cells[0].Value.ToString();
                        ogrenci.Sinif = dataGridView1.Rows[l].Cells[2].Value.ToString();
                        ogrenci.Izin = Convert.ToInt32(dataGridView1.Rows[l].Cells[5].Value.ToString());
                    }
                    bdb.SaveChanges();
                }
            }
            string[] sutunAdi = { "username", "pass", "AdSoyad", "Sinif", "Durum", "Izin" };
            for (int m = 0; m < sutunAdi.Length; m++)
            {
                if (sutunAdi[m] == "Durum" || sutunAdi[m] == "Izin")
                {
                    using (var bdb = new BrowserContext(baglanti))
                    {
                        List<User> ogrenci = bdb.UserSet.ToList();
                        foreach (var ogr in ogrenci)
                        {
                            if (ogr.Durum == null) { ogr.Durum = 0; bdb.SaveChanges(); }
                            if (ogr.Izin == null) { ogr.Izin = 1; bdb.SaveChanges(); }
                        }
                    }
                }
            }
            excelgoster();
            MessageBox.Show("Tablo Doldurma İşlemi Tamamlanmıştır.", "Tablo Doldurma");
        }

        private void buttonExcelGuncelle_Click(object sender, EventArgs e)
        {
            exceldenguncelle();
        }

        private void excelsenkronizasyon_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Equals("EXCEL"))
                {
                    clsProcess.Kill();
                    break;
                }
            }
        }
    }
}