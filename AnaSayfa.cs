using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StorkShipping
{
    public partial class AnaSayfa : Form
    {
        private readonly Bitmap cizimAlani;  // Harita görselini çizim alanı olarak belirlememiz için gerekli global değişken
        private readonly int[,] listAdresleri = new int[5, 200];
        private readonly int[] toplamYol = new int[5];

        public AnaSayfa()
        {
            InitializeComponent();
            cizimAlani = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = cizimAlani;
        }

        public void CizimYap(int yazilacakYol, int adet)
        {
            int diziUzunlugu = 0;
            var pens = new Dictionary<string, Pen>();
            Pen Kalem1 = new Pen(System.Drawing.Color.Red, 5);
            Pen Kalem2 = new Pen(System.Drawing.Color.Blue, 5);
            Pen Kalem3 = new Pen(System.Drawing.Color.Green, 5);
            Pen Kalem4 = new Pen(System.Drawing.Color.Black, 5);
            Pen Kalem5 = new Pen(System.Drawing.Color.Purple, 5);
            pens.Add("Kalem1", Kalem1);
            pens.Add("Kalem2", Kalem2);
            pens.Add("Kalem3", Kalem3);
            pens.Add("Kalem4", Kalem4);
            pens.Add("Kalem5", Kalem5);

            for (int i = 0; i < 81; i++)
            {
                if (listAdresleri[yazilacakYol, i] != 0) diziUzunlugu++;
                else break;
            }

            Graphics grafik;
            grafik = Graphics.FromImage(cizimAlani);
            int kayma = 15 + (adet * 2);
            for (int i = 0; i < diziUzunlugu - 1; i++)
            {
                grafik.DrawLine(pens.FirstOrDefault(x => x.Key == "Kalem" + adet.ToString()).Value, (Controls["button" + listAdresleri[yazilacakYol, i]] as Button).Location.X + kayma, (Controls["button" + listAdresleri[yazilacakYol, i]] as Button).Location.Y + kayma,
                (Controls["button" + listAdresleri[yazilacakYol, i + 1]] as Button).Location.X + kayma, (Controls["button" + listAdresleri[yazilacakYol, i + 1]] as Button).Location.Y + kayma);
            }

            pictureBox1.Image = cizimAlani;
            grafik.Dispose();
        }

        public void FormSifirla()
        {
            Graphics grafik;
            grafik = Graphics.FromImage(cizimAlani);
            grafik.Clear(Color.Transparent);

            Refresh();
            listBox2.Items.Clear();
        }

        public void EnkisaYoluBul()
        {
            for (int rpt = 0; rpt < 5; rpt++)
            {
                int lsSay = 0;
                int[,] graf = Sehirler.graf;             //Sehirler classında ki oluşturduğumuz grafı, fonksiyonumuza aktarıyoruz
                int[] HedefAdres = new int[11];          //Kullanıcının seçtiği şehirleri listboxtan çekip bu dizide saklıyorz
                int[] KaynakAdres = new int[12];         //Seçilen adresler arasından kaynak seçimini tutuyor
                int Tur = 1;                             //Yazdırma fonksiyonunun çalışma sayısını tutuyor
                int matrisBoyut = 81;                    //Komşuluk matrisinin boyutunu tutuyor
                int islemTipi = 1;                       //İşlem tipini tutuyor ( Yazdırma işlemi ve tespit işlemi için iki ayrı işlem yapılıyor)
                int hedef;                               //Son düğümün adresini tutuyor. işlem tipine göre alıcağı değer değişiyor
                int AnlikHedef = 0;                      //Per döngüsü içersindeki hedef belirleme için gerekli adresi tutuyor
                int PerMax = listBox1.Items.Count;       //Gidilecek toplam adres sayısı
                bool[] hedefKontrol = new bool[10];      //Kullanıcının seçtiği şehirlerin kullanılma durumunu boolean bir şekilde saklıyor
                KaynakAdres[0] = 41;                     //Program göreve Kocaeli'den başlıyacağı için default olarak 41 adresi veriliyor.
                if (checkBox1.Checked == true) PerMax++; //Eğer dönüş yolu işaretliyse döngü bir arttırlıyor

                for (int i = 0; i < listBox1.Items.Count; i++) HedefAdres[i] = Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2));

                for (int per = 0; per < PerMax; per++)
                {
                    int limit = 1;
                islemYap:
                    int baslangic = KaynakAdres[per] - 1;

                    if (islemTipi == 1) hedef = HedefAdres[per] - 1;
                    else hedef = AnlikHedef - 1;
                    LinkedList<int> yol = new LinkedList<int>();
                    int? dugum = hedef;
                    int?[] oncekiDugum = new int?[matrisBoyut];
                    int[] uzaklik = new int[matrisBoyut];
                    bool[] gezmeKontrol = new bool[matrisBoyut];

                    if (rpt > 0 && per <= listBox1.Items.Count)
                    {
                        for (int k = limit; k < 80; k++)
                        {
                            if (listAdresleri[0, k] != 0) gezmeKontrol[listAdresleri[0, k] - 1] = true;
                            if (listAdresleri[1, k] != 0) gezmeKontrol[listAdresleri[1, k] - 1] = true;
                            if (listAdresleri[2, k] != 0) gezmeKontrol[listAdresleri[2, k] - 1] = true;
                            if (listAdresleri[3, k] != 0) gezmeKontrol[listAdresleri[3, k] - 1] = true;
                        }

                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            gezmeKontrol[Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2)) - 1] = false;
                        }
                    }
                    for (int i = 0; i < matrisBoyut; i++) uzaklik[i] = int.MaxValue;
                    uzaklik[baslangic] = 0;

                    for (int R = 0; R < matrisBoyut; R++)
                    {
                        int enYakinDugum = 0;
                        int enAzUzaklik = int.MaxValue;
                        int enYakinDugumeOlanUzaklik;
                        int sonrakiDugumeOlanUzaklik;
                        int toplamUzaklik;

                        for (int i = 0; i < matrisBoyut; i++)
                        {
                            if (gezmeKontrol[i] == false && uzaklik[i] < enAzUzaklik)
                            {
                                enYakinDugum = i;
                                enAzUzaklik = uzaklik[i];
                            }
                        }
                        if (enAzUzaklik == int.MaxValue) break;
                        gezmeKontrol[enYakinDugum] = true;

                        for (int i = 0; i < matrisBoyut; i++)
                        {
                            if (graf[enYakinDugum, i] > 0)
                            {
                                sonrakiDugumeOlanUzaklik = graf[enYakinDugum, i];
                                enYakinDugumeOlanUzaklik = uzaklik[enYakinDugum];
                                toplamUzaklik = enYakinDugumeOlanUzaklik + sonrakiDugumeOlanUzaklik;

                                if (toplamUzaklik < uzaklik[i])
                                {
                                    uzaklik[i] = toplamUzaklik;
                                    oncekiDugum[i] = enYakinDugum;
                                }
                            }
                        }
                    }
                    if (islemTipi == 0)  // Çizim işlemi için gerekli hesaplamalar yapılan kısım
                    {
                        while (dugum != null)
                        {
                            yol.AddFirst(dugum.Value);
                            dugum = oncekiDugum[dugum.Value];
                        }

                        if (yol.Count == 1)
                        {
                            if (limit == 81) { }
                            else
                            {
                                limit++;
                                goto islemYap;
                            }
                        }

                        KaynakAdres[Tur] = AnlikHedef;
                        Tur++;

                        if (Tur == 2)
                        {
                            listAdresleri[rpt, lsSay] = KaynakAdres[Tur - 2];
                            lsSay++;
                        }
                        for (int i = 1; i < yol.Count; i++)
                        {
                            listAdresleri[rpt, lsSay] = yol.ToList()[i] + 1;
                            lsSay++;
                            toplamYol[rpt] += graf[yol.ToList()[i - 1], yol.ToList()[i]];
                        }

                        islemTipi = 1;
                    }
                    else  // Sonraki gidilecek hedefi seçme işlemi için gerekli kısım
                    {
                        if (per == listBox1.Items.Count) AnlikHedef = KaynakAdres[0];
                        else
                        {
                            int Tempkontrol = AnlikHedef;
                            int kontrolTut = 0;

                            for (int i = 0; i < listBox1.Items.Count; i++)
                            {
                                if (hedefKontrol[i] == false)
                                {
                                    AnlikHedef = HedefAdres[i];
                                }
                            }

                            for (int i = 0; i < listBox1.Items.Count; i++)
                            {
                                if (uzaklik[HedefAdres[i] - 1] <= uzaklik[AnlikHedef - 1] && hedefKontrol[i] == false)
                                {
                                    AnlikHedef = HedefAdres[i];
                                    kontrolTut = i;
                                }
                            }

                            if (Tempkontrol == AnlikHedef)
                            {
                                for (int j = 0; j < listBox1.Items.Count; j++)
                                {
                                    if (hedefKontrol[j] == false)
                                    {
                                        AnlikHedef = HedefAdres[j];
                                        kontrolTut = j;
                                    }
                                }
                            }
                            hedefKontrol[kontrolTut] = true;
                        }
                        islemTipi = 0;
                        goto islemYap;
                    }
                }
            }
            AlternatifOlustur();
        }

        public void AlternatifOlustur()
        {
            int Sehirler;
            Sirala();
            listBox3.Items.Add("");
            for (int i = 0; i < 5; i++)
            {
                bool[] esitKontrol = new bool[5];
                for (int k = 0; k < i; k++)
                {
                    if (i != k && toplamYol[i] == toplamYol[k]) esitKontrol[i] = true;
                }
                if (esitKontrol[i] == false)
                {
                    Sehirler = 0;
                    for (int j = 0; j < 81; j++) if (listAdresleri[i, j] != 0) Sehirler++;
                    listBox3.Items.Add((listBox3.Items.Count) + ".yol " + Sehirler + " şehir, uzunluk = " + toplamYol[i] + " KM                 " + i);
                }
            }
            listBox3.Items[0] = "  Tüm Alternatif Yolları Göster" + " ( " + (listBox3.Items.Count - 1) + " )                          6";

            listBox3.SelectedIndex = 0;
        }

        public void YolYazdir(int yazilacakYol)
        {
            if (yazilacakYol == 6)
            {
                for (int adet = 1; adet < listBox3.Items.Count; adet++)
                {
                    int yaz = Convert.ToInt32(listBox3.Items[adet].ToString().Substring(listBox3.Items[adet].ToString().Length - 1, 1));
                    for (int i = 0; i < 81; i++)
                    {
                        if (listAdresleri[yaz, i] != 0)
                            listBox2.Items.Add((i + 1) + ") " + Sehirler.SehirAd[listAdresleri[yaz, i]] + " [ " + Convert.ToString(listAdresleri[yaz, i]) + " ]");
                    }
                    listBox2.Items.Add("");
                    label3.Text = "Güzargah Seçiniz";
                    CizimYap(yaz, adet);
                }
            }
            else
            {
                FormSifirla();
                int yaz = yazilacakYol;
                for (int i = 0; i < 81; i++)
                {
                    if (listAdresleri[yaz, i] != 0)
                        listBox2.Items.Add((i + 1) + ") " + Sehirler.SehirAd[listAdresleri[yaz, i]] + " [ " + Convert.ToString(listAdresleri[yaz, i]) + " ]");
                }
                listBox2.Items.Add("");
                label3.Text = (listBox2.Items.Count - 1).ToString() + " Şehir " + toplamYol[yaz] + " KM";
                CizimYap(yaz, (yaz + 1));
            }
        }

        public void ListElemanEkleSil(int btnNo, int islemTipi)
        {
            if (islemTipi == 0)
            {
                if (listBox1.Items.Count > 9)
                {
                    MessageBox.Show("En Fazla 10 Şehir Seçebilirsiniz");
                }
                else
                {
                    if (btnNo < 82 && btnNo > 0 && (Controls["button" + btnNo].BackColor == System.Drawing.SystemColors.ButtonHighlight))
                    {
                        (Controls["button" + btnNo] as Button).BackColor = System.Drawing.SystemColors.MenuHighlight;
                        (Controls["button" + btnNo] as Button).ForeColor = System.Drawing.SystemColors.ControlLightLight;
                        listBox1.Items.Add(btnNo.ToString() + " - " + Sehirler.SehirAd[btnNo]);
                    }
                    else
                    {
                        MessageBox.Show("Yanlış veya zaten seçilmiş bir şehir girdiniz");
                    }
                }
            }
            else
            {
                (Controls["button" + btnNo] as Button).BackColor = System.Drawing.SystemColors.ButtonHighlight;
                (Controls["button" + btnNo] as Button).ForeColor = System.Drawing.SystemColors.ControlText;

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2)) == btnNo)
                    {
                        listBox1.Items.Remove(btnNo.ToString() + " - " + Sehirler.SehirAd[btnNo]);
                    }
                }
            }
            label8.Text = "Seçilen Şehir Sayısı = " + listBox1.Items.Count.ToString();
        }

        public void GrafOlustur()
        {
        Tekrar:
            try
            {
                string stun;
                string dizinYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Stork Shipping";
                if ((File.Exists(dizinYolu + "\\Sehir Verileri.txt")) == false) VeriOlustur.VeriDosyasiOlustur();
                using (StreamReader dizin = new StreamReader(dizinYolu + "\\Sehir Verileri.txt"))

                    while ((stun = dizin.ReadLine()) != null)
                    {
                        string[] mSatir = stun.Split('-', ',');
                        for (int i = 2; i < mSatir.Length - 1; i += 2)
                        {
                            Sehirler.graf[Convert.ToInt32(mSatir[0]) - 1, (Sehirler.FindCityName(mSatir[i]) - 1)] = Convert.ToInt32(mSatir[i + 1]);
                        }
                    }
            }
            catch (FormatException)
            {
                VeriOlustur.VeriDosyasiOlustur();
                goto Tekrar;
            }
        }

        public void Sirala()
        {
            for (int L = 0; L < 4; L++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (toplamYol[i] > toplamYol[i + 1])
                    {
                        int[] temp = new int[81];
                        int tmp = toplamYol[i];
                        for (int j = 0; j < 81; j++)
                        {
                            temp[j] = listAdresleri[i, j];
                            listAdresleri[i, j] = listAdresleri[(i + 1), j];
                            listAdresleri[(i + 1), j] = temp[j];
                        }
                        toplamYol[i] = toplamYol[i + 1];
                        toplamYol[i + 1] = tmp;
                    }
                }
            }
        }

        public void Hesapla()
        {
            listBox3.Items.Clear();
            Array.Clear(listAdresleri, 0, listAdresleri.Length);
            Array.Clear(toplamYol, 0, toplamYol.Length);
            FormSifirla();
            if (listBox1.Items.Count != 0) EnkisaYoluBul();
            else MessageBox.Show("Güzargah Oluşturulabilmesi için, en az bir hedef şehir seçilmelidir.");
        }

        public void Yazdir(string KayitYolu)
        {
            string kayitAdresi;
            string dizinYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Stork Shipping";
            if (Directory.Exists(dizinYolu) == false) Directory.CreateDirectory(dizinYolu);
            if (KayitYolu == "") kayitAdresi = dizinYolu + "\\Arama Kayıtları.txt";
            else kayitAdresi = KayitYolu;
            bool kullanilmaDurumu = false;
            StreamWriter Yaz = new StreamWriter(kayitAdresi, true);

            {
                Yaz.WriteLine("");
                Yaz.WriteLine(" Dağıtım Merkezi = Kocaeli [41]");
                Yaz.Write(" Gidilmesi Planlanan Şehirler = ");
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    Yaz.Write(Sehirler.SehirAd[Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2))] + "[" + "{0,2}" + "]", Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2)));
                    if (i < listBox1.Items.Count - 1) Yaz.Write(" -> ");
                }
                Yaz.WriteLine("\n");

                for (int i = 1; i < (listBox3.Items.Count); i++)
                {
                    Yaz.Write("{0,26}", i + ".Güzargah");
                }
                Yaz.WriteLine("\n");
                for (int kj = 0; kj < 81; kj++)
                {
                    for (int i = 1; i < (listBox3.Items.Count); i++)
                    {
                        if (listAdresleri[Convert.ToInt32(listBox3.Items[i].ToString().Substring(listBox3.Items[i].ToString().Length - 1, 1)), kj] != 0)
                        {
                            Yaz.Write("  " + "{0,20}" + " [" + "{1,2}" + "]", Sehirler.SehirAd[listAdresleri[Convert.ToInt32(listBox3.Items[i].ToString().Substring(listBox3.Items[i].ToString().Length - 1, 1)), kj]], listAdresleri[Convert.ToInt32(listBox3.Items[i].ToString().Substring(listBox3.Items[i].ToString().Length - 1, 1)), kj]);
                        }
                        else if (kullanilmaDurumu == true)
                        {
                            Yaz.Write("                           ");
                        }
                    }
                    kullanilmaDurumu = false;
                    for (int i = 1; i < (listBox3.Items.Count); i++)
                    {
                        if ((listAdresleri[(Convert.ToInt32(listBox3.Items[i].ToString().Substring(listBox3.Items[i].ToString().Length - 1, 1))), kj]) != 0)
                        {
                            Yaz.Write("\n");
                            kullanilmaDurumu = true;
                            break;
                        }
                    }
                }
                Yaz.Write("\n");

                for (int i = 1; i < (listBox3.Items.Count); i++)
                {
                    Yaz.Write("{0,27}", (toplamYol[Convert.ToInt32(listBox3.Items[i].ToString().Substring(listBox3.Items[i].ToString().Length - 1, 1))]) + " Kilometre");
                }
                Yaz.WriteLine("\n");
                for (int i = 0; i < 150; i++) Yaz.Write("-");
                Yaz.WriteLine("");
                Yaz.Flush();
                Yaz.Close();
                Yaz.Dispose();
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.BackColor == System.Drawing.SystemColors.ButtonHighlight)
            {
                ListElemanEkleSil(Convert.ToInt32(clickedButton.Text.Substring(0, 2)), 0);
            }
            else
            {
                ListElemanEkleSil(Convert.ToInt32(clickedButton.Text.Substring(0, 2)), 1);
            }
        }

        private void SehirEkle_Click(object sender, EventArgs e)
        {
            int sehirplaka = 0;
            if (radioButton2.Checked == true) sehirplaka = Sehirler.SehirPlakaBul(textBox4.Text);
            else
            {
                if (textBox4.Text != "") sehirplaka = Convert.ToInt32(textBox4.Text);
            }
            ListElemanEkleSil(sehirplaka, 0);
            textBox4.Clear();
        }

        private void SilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Silme işlemi için öncelikle, silinecek şehir seçmelisiniz");
            }
            else
            {
                ListElemanEkleSil(Convert.ToInt32(listBox1.SelectedItem.ToString().Substring(0, 2)), 1);
            }
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            GrafOlustur();
            radioButton1.Checked = true;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox4.Clear();
                textBox4.MaxLength = 2;
            }
            else
            {
                textBox4.Clear();
                textBox4.MaxLength = 20;
            }
        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Secim;
            Secim = Convert.ToInt32(listBox3.SelectedItem.ToString().Substring(listBox3.SelectedItem.ToString().Length - 1, 1));
            YolYazdir(Secim);
        }

        private void Button82_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != 0) listBox3.SelectedIndex--;
        }

        private void Button83_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex < listBox3.Items.Count - 1) listBox3.SelectedIndex++;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0) { Hesapla(); Yazdir(""); }
        }

        private void BtnHesapla(object sender, EventArgs e)
        {
            Hesapla();
            Yazdir("");
        }

        private void BtnYazdir(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0 && listBox3.Items.Count != 0)
            {
                string KayitYolu;
                SaveFileDialog ofd = new SaveFileDialog()
                {
                    Filter = "Text Files | *.txt",
                    DefaultExt = "txt",
                    FileName = "Kocaeli-" + (listBox1.Items[listBox1.Items.Count - 1].ToString().Substring(4, (listBox1.Items[listBox1.Items.Count - 1].ToString().Length - 4))).Replace(" ", "")
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    KayitYolu = Path.GetFullPath(ofd.FileName);
                    Yazdir(KayitYolu);
                }
            }
            else
            {
                MessageBox.Show("Guzargahı kaydedebilmeniz için öncelikle hesaplanması gereklidir.");
            }
        }

        private void DyolClick(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) checkBox1.Checked = false; else checkBox1.Checked = true;
        }

        private void BtnTxtAc(object sender, EventArgs e)
        {
            string dizinYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Stork Shipping";
            if (Directory.Exists(dizinYolu) == false) Directory.CreateDirectory(dizinYolu);
            StreamWriter Yaz = new StreamWriter(dizinYolu + "\\Arama Kayıtları.txt", true);
            Yaz.Write("");
            Yaz.Flush();
            Yaz.Close();
            Yaz.Dispose();
            var process = Process.Start(dizinYolu + "\\Arama Kayıtları.txt");
            process.WaitForInputIdle();
            SendKeys.Send("^{END}");
        }
    }
}