﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StorkShipping
{
    public partial class AnaSayfa : Form
    {
        // Global Değişkenler 

        readonly string[] SehirAd = { " ","Adana", "Adıyaman", "Afyon", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın",
            "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli",
            "Diyarbakır", "Edirne", "Elazığ", "Ezincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari",
            "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli",
            "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize",
            "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat",
            "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük",
            "Kilis", "Osmaniye", "Düzce" };
        readonly Bitmap cizimAlani;
        readonly int[] HedefAdres = new int[10];
        readonly int[] KaynakAdres = new int[11];
        readonly bool[] hedefKontrol = new bool[10];
        int toplamYol = 0;
        int AnlikHedef = 0;
        int Tur;
        
        int Round = 0;
      
        //------------------------------ 

        public AnaSayfa()
        {
            InitializeComponent();
            cizimAlani = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = cizimAlani;

        }
        public void CizimYap(int[] adresler)
        {

         

            Pen Kalem = new Pen(System.Drawing.Color.Red, 5);
            Graphics grafik;
            grafik = Graphics.FromImage(cizimAlani);
      
            for (int i = 0; i < adresler.Length - 1; i++)
            {
                
             grafik.DrawLine(Kalem, (Controls["button" + (adresler[i]+1)] as Button).Location.X+15, (Controls["button" + (adresler[i]+1)] as Button).Location.Y + 15,
                  (Controls["button" + (adresler[i+1]+1)] as Button).Location.X + 15, (Controls["button" + (adresler[i+1]+1)] as Button).Location.Y + 15);
        
            }

            pictureBox1.Image = cizimAlani;
            grafik.Dispose();
        }

        public void FormSifirla()
        {
            Graphics grafik;
            grafik = Graphics.FromImage(cizimAlani);
            grafik.Clear(Color.Transparent);
            // this.Controls.Clear();
            // this.InitializeComponent();
            Refresh();
           
        }

        public void EnkisaYoluBul(int baslangic, int hedef, int matrisBoyut, int[,] graf,int islemTipi)
        {
            int[] uzaklik = new int[matrisBoyut];

            for (int i = 0; i < matrisBoyut; i++)
            {

                uzaklik[i] = int.MaxValue;

            }

            bool[] gezmeKontrol = new bool[matrisBoyut];
            int?[] oncekiDugum = new int?[matrisBoyut];
            int? dugum = hedef;
            uzaklik[baslangic] = 0;
            LinkedList<int> yol = new LinkedList<int>();

            while (true)
            {
                int enYakinDugum = 0;
                int enAzUzaklik = int.MaxValue;
                int enYakinDugumeOlanUzaklik;
                int sonrakiDugumeOlanUzaklik;
                int toplamUzaklik;


                for (int i = 0; i < matrisBoyut; i++)
                {
                    if (gezmeKontrol[i] == false && enAzUzaklik > uzaklik[i])
                    {
                        enYakinDugum = i;
                        enAzUzaklik = uzaklik[i];
                    }
                }

                if (enAzUzaklik == int.MaxValue)
                {
                    break;
                }

                gezmeKontrol[enYakinDugum] = true;

                for (int i = 0; i < matrisBoyut; i++)
                {
                    if (graf[enYakinDugum, i] > 0)
                    {
                        enYakinDugumeOlanUzaklik = uzaklik[enYakinDugum];
                        sonrakiDugumeOlanUzaklik = graf[enYakinDugum, i];

                        toplamUzaklik = enYakinDugumeOlanUzaklik + sonrakiDugumeOlanUzaklik;

                        if (toplamUzaklik < uzaklik[i])
                        {
                            uzaklik[i] = toplamUzaklik;
                            oncekiDugum[i] = enYakinDugum;
                        }
                    }
                }
            }
            if (islemTipi == 0)
            {
                while (dugum != null)
            {
                yol.AddFirst(dugum.Value);
                dugum = oncekiDugum[dugum.Value];
            }
                KaynakAdres[Tur] = AnlikHedef;
                Tur++;
                YazdirVeEkle(yol.ToList(), graf);
              
            }
            else 
            {
                int Tempkontrol = AnlikHedef;
                int kontrolTut=0;
              
                
              
                     
                for (int i = 0; i < Round; i++)
                {
                    if (uzaklik[HedefAdres[i]-1] < uzaklik[AnlikHedef-1] && hedefKontrol[i]==false)
                    {
                        AnlikHedef = HedefAdres[i];
                        kontrolTut = i;
                        MessageBox.Show("GİRDİ");
                    }       
                }

                if(Tempkontrol==AnlikHedef)
                {
                    for (int j = 0; j < Round; j++)
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
        }
        public void YazdirVeEkle(List<int> yol, int[,] graf)
        {
            string gidilenYollar;
            string gidilenYollar2="";
         
            int[] indisDizi = new int[yol.Count];
            int[] yazdirilcakDizi = new int[yol.Count];

            for (int i = 0; i < yol.Count; i++)
            {
                indisDizi[i] = yol[i];
                yazdirilcakDizi[i] = yol[i] + 1;
               
            }

            for (int i = 0; i < yol.Count - 1; i++)
            {
                toplamYol += graf[yol[i], yol[i + 1]];
            }
           

           gidilenYollar = string.Join("->", yazdirilcakDizi);
           
            for(int i=1; i<yazdirilcakDizi.Length;i++)
            {
                gidilenYollar2 +="->"+ Convert.ToString(yazdirilcakDizi[i]);
            }
             
            textBox2.Text = Convert.ToString(toplamYol)+" Kilometre";
            if(Tur>2)
            { 
            textBox1.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) + indisDizi.Length-1);
            richTextBox1.Text = String.Concat(richTextBox1.Text, gidilenYollar2);
            }
            else
            {
            richTextBox1.Text = gidilenYollar;
            textBox1.Text = Convert.ToString(indisDizi.Length);
            }

        CizimYap(indisDizi);
                     
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (clickedButton.BackColor == System.Drawing.SystemColors.ButtonHighlight)
            {
                if (Round > 9)
                {
                    MessageBox.Show("En Fazla 10 Şehir Seçebilirsiniz");
                }
                else
                { 
                clickedButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
                clickedButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;             
                HedefAdres[Round] = Convert.ToInt32(clickedButton.Text);                                                       
                listBox1.Items.Add(HedefAdres[Round] + " - " + SehirAd[HedefAdres[Round]]);
                Round++;
                }
            }
            else
            {
                clickedButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
                clickedButton.ForeColor = System.Drawing.SystemColors.ControlText;
                Round--;
              
                for (int i = 0; i < HedefAdres.Length; i++)
                {
                    if(HedefAdres[i]== Convert.ToInt32(clickedButton.Text))
                    {
                        listBox1.Items.Remove(Convert.ToInt32(clickedButton.Text) + " - " + SehirAd[Convert.ToInt32(clickedButton.Text)]);
                        Array.Clear(HedefAdres, i, 1);
               
                    }
                    
                }

                for(int i =0; i<HedefAdres.Length-1;i++)
                {
                    if (HedefAdres[i] == 0)
                    {
                        for (int j = i; j < HedefAdres.Length - 1; j++)
                        {
                            HedefAdres[j] = HedefAdres[j + 1];
                        }
                    }
                }             
            }                                
        }
        private void SehirEkle_Click(object sender, EventArgs e)
        {
            int sehirplaka = 0;

            if (radioButton2.Checked == true)
            {
                for (int i = 0; i < SehirAd.Length; i++)
                {
                    if (SehirAd[i].ToUpper() == textBox4.Text.ToUpper())
                    {
                        sehirplaka = i;
                    }
                }
            }
            else
            {
                if (textBox4.Text != "")
                {
                    sehirplaka = Convert.ToInt32(textBox4.Text);
                }
            }
            if (sehirplaka == 0)
            {
                MessageBox.Show("Yanlış Yada Eksik Bir Şehir Adı Girdiniz");
            }
            else
            {
                if (sehirplaka < 82 && sehirplaka > 0 && (Controls["button" + sehirplaka].BackColor == System.Drawing.SystemColors.ButtonHighlight))
                {
                    (Controls["button" + sehirplaka] as Button).BackColor = System.Drawing.SystemColors.MenuHighlight;
                    (Controls["button" + sehirplaka] as Button).ForeColor = System.Drawing.SystemColors.ControlLightLight;
                    HedefAdres[Round] = sehirplaka;
                    listBox1.Items.Add(HedefAdres[Round] + " - " + SehirAd[HedefAdres[Round]]);
                    Round++;
                }
                else
                {
                    MessageBox.Show("Yanlış veya zaten seçilmiş bir şehir girdiniz");
                }
            }

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
                string deneme;
                deneme = Convert.ToString(Convert.ToInt32(listBox1.SelectedItem.ToString().Substring(0, 2)));
                (Controls["button" + deneme] as Button).BackColor = System.Drawing.SystemColors.ButtonHighlight;
                (Controls["button" + deneme] as Button).ForeColor = System.Drawing.SystemColors.ControlText;
                Round--;

                for (int i = 0; i < HedefAdres.Length; i++)
                {
                    if (HedefAdres[i] == Convert.ToInt32(deneme))
                    {
                        listBox1.Items.Remove(Convert.ToInt32(deneme) + " - " + SehirAd[Convert.ToInt32(deneme)]);
                        Array.Clear(HedefAdres, i, 1);

                    }

                }

                for (int i = 0; i < HedefAdres.Length - 1; i++)
                {
                    if (HedefAdres[i] == 0)
                    {
                        for (int j = i; j < HedefAdres.Length - 1; j++)
                        {
                            HedefAdres[j] = HedefAdres[j + 1];
                        }
                    }
                }

            }
        }
        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(20, Color.White);
            panel2.BackColor = Color.FromArgb(20, Color.White);
            radioButton1.Checked=true;
        }
        private void Yazdir(object sender, EventArgs e)
        {
            FormSifirla();
        }
        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }              
            }       
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox4.MaxLength = 2;
        }
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox4.MaxLength = 20;
        }
 
        private void Hesapla(object sender, EventArgs e)
{
            AnlikHedef=HedefAdres[0];
            toplamYol = 0;
            int[,] graf =
    {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,191,0,69,0,0,0,0,335,0,0,0,0,0,0,0,192,0,0,0,0,207,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,87,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,207,0,0,0,0,0,150,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,187,0,163,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,112,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,169,0,0,0,0,222,0,0,0,0,0,144,0,0,0,0,0,168,0,0,0,0,0,0,0,0,0,223,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,115,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,234,0,0,0,0,0,0,0,0,0,0,0,183,0,0,0,0,0,0,0,0,0,0,214,0,0,0,0,0,0,0,0,0,0,0,0,245,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,228,0,0,0,0,0,0,0,0,0,0,143,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,92,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,131,0,0,0,0,114,0,0,0,0,0,200,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,191,0,0,0,130,0,0,0,0,0,0,0,233,0,0,0,0,0,0,0,0,0,0,0,0,0,184,0,258,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,225,0,0,75,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,122,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,130,466,0,0,0,0,0,0,0,0,322,0,0,0,0,0,311,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,374,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,226,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,161,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,116,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,126,0,0,0,0,0,0,0,0,0,0,0,0,0,0,126,0,0,0,0,0,0,0,0,0,155,0,0,99,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,151,198,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,176,0,0,0,0,0,0,0,228,0,141,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,213,0,94,0,0,0,0,0,0,0,0,0,83,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,112,0,0,0,0,0,0,0,0,0,0,99,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,141,0,144,275,177,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,111,0,0,0,0,0,0,0,0,0,0,0,0,144,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,234,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,83,0,0,0,0,0,0,96,0,0,0,0,0,0,0,0,161,0,0,0,0,0,0,135,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,191,0,0,0,0,213,0,0,0,0,0,0,233,0,0,0,0,0,0,0,291,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,114,0,0,0,0,0,0,0,0,0,0,0,0,157,0,0,0,0,0,0,0,0,0,0,134,0,0,45},
{0,0,169,0,0,0,122,0,0,0,0,0,0,0,0,0,0,0,0,150,0,0,0,0,0,0,0,0,0,0,0,51,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,241,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,151,94,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,132,0,177,0,0,0,0,0,0,0,0,0,0,157,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,69,0,0,0,0},
{0,0,0,0,0,0,0,0,0,198,0,0,0,0,0,0,0,0,0,0,0,216,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,187,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,130,0,0,0,0,0,0,0,233,0,0,0,0,156,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,106,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,104,0,0,0,0,0,0,193,0,0,0},
{0,0,0,0,92,0,0,0,0,0,0,0,0,0,0,0,0,156,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,197,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,172,0,266,0,0,0,0,0,0,0,0,108,0,0,0,0,166,0,0,0,0,0,0,0,0,0,0},
{0,0,222,0,0,0,0,0,126,0,0,0,0,0,150,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,208,0,0,145,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,150,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,207,0,0,0,0,0,0,0,0,0,141,0,0,0,0,0,0,0,0,0,0,153,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,249,0,0,96,0,252,0,0,0,0,0,0,0,0,0,0,0,0,0,176,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,216,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,62,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,140,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,144,0,0,0,0,0,0,0,0,153,0,0,267,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,98,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,136,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,275,0,0,0,0,0,0,0,0,0,0,267,0,190,0,0,295,133,0,0,0,0,0,0,0,0,0,0,0,0,0,0,363,0,0,0,0,0,0,0,0,0,0,0,0,0,248,0,0,0,243,0,0,0,0,0,0,155,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,183,0,0,0,226,0,0,0,177,0,0,0,0,0,0,0,0,0,0,0,190,0,0,0,0,0,0,0,0,0,0,0,203,0,0,0,0,0,0,0,0,0,0,0,0,266,0,0,0,259,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,233,0,0,0,0,0,233,0,0,0,0,0,0},
{0,0,144,0,0,233,0,0,0,0,83,0,0,291,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,233,78,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,150,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,193,0,0,0,0,0,0,0,0,0,0,0,0,0,0,76,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,137,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,64,125,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,295,0,0,0,0,162,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,44,0,0,0,0,0,298,0,0,136,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,133,0,0,0,162,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,78,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,197,0,0,0,0,0,0,0,189,0,0,0,0,0,0,0,0},
{191,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,193,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,129,0},
{0,0,168,0,0,0,130,0,0,0,0,0,0,0,51,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,263,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{69,0,0,0,0,0,466,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,348,0,0,0,0,0,0,0,0,200,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,235,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,111,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,131,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,126,176,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,35,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,214,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,203,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,94,140,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,106,197,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,183,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,183,0,0,0,114,0,0,0},
{335,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,261,0,0,0,81,128,0,0,0,0,0,0,194,0,0,0,0,0,0,0,197,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,62,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,121,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,184,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,91,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,112,0,110,0,0,113,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,132,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,111,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,37,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,65,0,0,0,0},
{0,0,223,0,0,258,322,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,233,0,0,0,0,0,263,348,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,242,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,148,0,119,0,0,0,0,0,0,0,0,0,0,0},
{0,0,100,0,0,0,0,0,0,228,112,0,0,0,0,177,0,0,0,0,0,0,0,0,0,78,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,316,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,139,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,187,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,249,0,98,363,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,224,0,0,0,0,0,0,0,0,0,0,0,243,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,155,141,0,0,0,0,0,0,0,0,0,208,0,0,0,0,0,0,0,0,0,0,0,0,0,0,35,0,0,0,0,0,0,0,316,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,195,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{192,163,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,76,0,0,0,0,0,0,0,0,0,0,261,0,0,0,0,0,224,0,0,0,0,0,0,0,0,0,0,0,0,0,327,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,104,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,96,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,227,0,0,0,0,0,0,188,0,0,0,0,0,0,0,0,150,197,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,311,0,99,0,0,0,0,0,241,0,0,0,0,145,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,245,0,0,0,0,0,0,0,111,83,0,0,0,0,0,0,0,252,0,0,0,266,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,218,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,81,0,91,0,0,0,0,0,0,0,0,0,0,82,0,0,0,0,0,0,0,0,0,0,0,0,0,0,203,0,75,0,0,0,0,0,0,0,0,0,0,0,0,0},
{207,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,200,0,0,0,0,128,0,0,0,242,0,0,0,0,0,0,0,82,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,122,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,44,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,150,0,0,314,0,216,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,161,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,259,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,75,0,0,0,0,0,0,0,253,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,99,0,0,114,0,157,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,37,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,69},
{0,0,0,0,131,0,0,0,0,0,0,0,0,0,0,0,0,0,172,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,150,0,0,0,0,155,0,0,229,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,96,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,227,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,257,0,0,0,0,0,0,86,100,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,266,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,183,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,155,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,248,0,0,0,298,0,0,0,0,0,0,0,0,0,194,0,0,0,0,0,243,0,327,0,0,0,0,0,314,0,0,0,0,0,0,0,108,0,0,0,0,0,224,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,187,0,0,0,0,140,0,0,0,0,0,0,0,0,0,0,0,131,0,0,0,0,121,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,114,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,216,0,0,229,0,0,108,0,0,0,0,0,0,0,205,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,136,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,75,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,178,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,144,0,0,0,0,0,0,0,0,0,0,136,243,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,112,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,176,0,0,0,0,0,137,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,188,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,115,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,150,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,139,0,195,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,228,0,0,0,0,0,0,0,0,161,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,197,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,257,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,357,0,0,0,0,0,0,0,0},
{0,0,0,0,200,0,0,0,0,0,0,0,0,0,0,0,0,0,108,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,197,0,112,0,0,0,0,0,0,0,0,0,203,0,0,0,0,0,0,0,224,0,205,0,0,0,0,0,0,0,0,0,0,140,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,157,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,87,0,0,0,100,0,0,113},
{0,0,0,0,0,225,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,110,0,148,0,0,0,0,0,0,0,75,122,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,155,233,0,0,0,78,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,253,0,0,0,0,0,0,0,178,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,374,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,235,0,0,0,0,0,0,0,0,119,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,75,0,0,0,0,0,0,0,0,0,0,0,104,166,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,113,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,140,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,135,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,150,0,218,0,0,0,0,0,0,86,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,189,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,197,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,0,357,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,183,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,87,0,0,0,0,0,0,0,0,0,0,89,0,0,0},
{0,0,0,0,0,0,0,116,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,233,0,0,0,0,0,0,0,0,0,0,94,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,143,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,140,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,69,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,65,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,134,0,0,0,193,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,114,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,89,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,64,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{87,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,125,0,0,0,129,0,0,0,0,0,0,0,0,0,0,0,0,0,0,104,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,45,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,69,0,0,0,0,0,0,0,0,0,0,0,0,113,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            KaynakAdres[0] = 41;
           FormSifirla();
            Tur = 1;
            if (HedefAdres[0] != 0)
            {
             // DENEMEE               
                for (int i = 0; i < Round ; i++)
                {
                     EnkisaYoluBul(KaynakAdres[i]-1, HedefAdres[i] - 1, 81, graf, 1);
                     EnkisaYoluBul(KaynakAdres[i]-1, AnlikHedef-1 , 81, graf, 0);
                }

            }
            else
            {
             MessageBox.Show("Güzargah Oluşturulabilmesi için, en az bir hedef şehir seçilmelidir.");
            }

        }
    }
}
