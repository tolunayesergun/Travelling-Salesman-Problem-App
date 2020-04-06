using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StorkShipping
{
    public partial class AnaSayfa : Form
    {
        private readonly Bitmap cizimAlani;  // Harita görselini çizim alanı olarak belirlememiz için gerekli global değişken

        public AnaSayfa()
        {
            InitializeComponent();
            cizimAlani = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = cizimAlani;
        }

        public void CizimYap(int[] adresler,int adet)
        {
            adet = adet + 1;
            var pens = new Dictionary<string, Pen>();
            Pen Kalem1 = new Pen(System.Drawing.Color.Red, 5);
            Pen Kalem2 = new Pen(System.Drawing.Color.Blue, 5);
            Pen Kalem3 = new Pen(System.Drawing.Color.Green, 5);
            Pen Kalem4 = new Pen(System.Drawing.Color.Yellow, 5);
            pens.Add("Kalem1", Kalem1);
            pens.Add("Kalem2", Kalem2);
            pens.Add("Kalem3", Kalem3);
            pens.Add("Kalem4", Kalem4);


            Graphics grafik;
            grafik = Graphics.FromImage(cizimAlani);

            for (int i = 0; i < adresler.Length - 1; i++)
            {
                grafik.DrawLine(pens.FirstOrDefault(x => x.Key == "Kalem"+adet.ToString()).Value, (Controls["button" + (adresler[i] + 1)] as Button).Location.X + 15, (Controls["button" + (adresler[i] + 1)] as Button).Location.Y + 15,
                (Controls["button" + (adresler[i + 1] + 1)] as Button).Location.X + 15, (Controls["button" + (adresler[i + 1] + 1)] as Button).Location.Y + 15);
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
            label6.Visible = true;
            label7.Visible = true;
            label3.Visible = true;
            listBox2.Items.Clear();
        }

        public void EnkisaYoluBul()
        {
            int listbox2count = listBox2.Items.Count;

            for (int rpt = 0; rpt < 3; rpt++)
            {
                int[,] graf = Sehirler.graf;             //Sehirler classında ki oluşturduğumuz grafı, fonksiyonumuza aktarıyoruz
                int[] HedefAdres = new int[11];          //Kullanıcının seçtiği şehirleri listboxtan çekip bu dizide saklıyorz
                int[] KaynakAdres = new int[12];         //Seçilen adresler arasından kaynak seçimini tutuyor
                int Tur = 1;                             //Yazdırma fonksiyonunun çalışma sayısını tutuyor
                int matrisBoyut = 81;                    //Komşuluk matrisinin boyutunu tutuyor
                int islemTipi = 1;                       //İşlem tipini tutuyor ( Yazdırma işlemi ve tespit işlemi için iki ayrı işlem yapılıyor)
                int hedef;                               //Son düğümün adresini tutuyor. işlem tipine göre alıcağı değer değişiyor
                int AnlikHedef = 0;                      //Per döngüsü içersindeki hedef belirleme için gerekli adresi tutuyor
                int toplamYol = 0;                       //Tüm şehirler gezildiğinde alınan mesafeyi içinde tutuyor
                int PerMax = listBox1.Items.Count;       //Gidilecek toplam adres sayısı
                int listboxNo=2;
                int limit=1;
                bool[] hedefKontrol = new bool[10];      //Kullanıcının seçtiği şehirlerin kullanılma durumunu boolean bir şekilde saklıyor
                KaynakAdres[0] = 41;                     //Program göreve Kocaeli'den başlıyacağı için default olarak 41 adresi veriliyor.
                if (checkBox1.Checked == true) PerMax++; //Eğer dönüş yolu işaretliyse döngü bir arttırlıyor

                for (int i = 0; i < listBox1.Items.Count; i++) HedefAdres[i] = Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2));

                for (int per = 0; per < PerMax; per++)
                {
                islemYap:
                    int baslangic = KaynakAdres[per] - 1;

                    if (islemTipi == 1) hedef = HedefAdres[per] - 1;
                    else hedef = AnlikHedef - 1;
                    LinkedList<int> yol = new LinkedList<int>();
                    int? dugum = hedef;
                    int?[] oncekiDugum = new int?[matrisBoyut];
                    int[] uzaklik = new int[matrisBoyut];
                    bool[] gezmeKontrol = new bool[matrisBoyut];

                    int k = limit;
                    for (k = limit; k < listbox2count - 1; k++)
                    {                  
                        gezmeKontrol[Convert.ToInt32(listBox2.Items[k].ToString().Substring(listBox2.Items[k].ToString().Length - 4, 3)) - 1] = true;
                    }
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        gezmeKontrol[Convert.ToInt32(listBox1.Items[i].ToString().Substring(0, 2)) - 1] = false;
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


                        MessageBox.Show(listbox2count.ToString());
                        if(yol.Count==1)
                        {
                            if (limit == listbox2count) break;
                            limit++;
                            goto islemYap;
                        }

                        KaynakAdres[Tur] = AnlikHedef;
                        Tur++;
                        int[] indisDizi = new int[yol.Count];
                        indisDizi[0] = KaynakAdres[Tur - 2] - 1;
                        if (Tur == 2) { listBox2.Items.Add("1) Kocaeli [ 41 ]"); listbox2count = listBox2.Items.Count; }

                        for (int i = 1; i < yol.Count; i++)
                        {
                            indisDizi[i] = yol.ToList()[i];
                            toplamYol += graf[yol.ToList()[i - 1], yol.ToList()[i]];
                            listBox2.Items.Add(listboxNo + ") " + Sehirler.SehirAd[yol.ToList()[i] + 1] + " [ " + Convert.ToString(yol.ToList()[i] + 1) + " ]");
                            listboxNo++;
                            listbox2count = listBox2.Items.Count;
                        }
                
                        label7.Text = Convert.ToString(toplamYol) + " KM";
                        label6.Text = listbox2count.ToString();
                        CizimYap(indisDizi,rpt);

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
            try
            {
                string stun;
                using (StreamReader dizin = new StreamReader(Application.StartupPath + "\\SehirVeri.txt"))

                    while ((stun = dizin.ReadLine()) != null)
                    {
                        string[] mSatir = stun.Split('-', ',');
                        for (int i = 2; i < mSatir.Length - 1; i += 2)
                        {
                            Sehirler.graf[Convert.ToInt32(mSatir[0]) - 1, (Sehirler.FindCityName(mSatir[i]) - 1)] = Convert.ToInt32(mSatir[i + 1]);
                        }
                    }
            }
            catch
            {
                MessageBox.Show("problem"); // tekrar yüklemeyi sağla.
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
            panel1.BackColor = Color.FromArgb(20, Color.White);
            panel2.BackColor = Color.FromArgb(20, Color.White);
            radioButton1.Checked = true;
            label7.Visible = false;
            label6.Visible = false;
            label3.Visible = false;
            GrafOlustur();
        }

        private void Yazdir(object sender, EventArgs e)
        {
            FormSifirla();
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

        private void Hesapla(object sender, EventArgs e)
        {
            FormSifirla();
            if (listBox1.Items.Count != 0) EnkisaYoluBul();
            else MessageBox.Show("Güzargah Oluşturulabilmesi için, en az bir hedef şehir seçilmelidir.");
        }
    }
}