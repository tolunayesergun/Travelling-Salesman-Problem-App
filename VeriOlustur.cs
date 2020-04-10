using System;
using System.IO;

namespace StorkShipping
{
    internal class VeriOlustur
    {
        public static void VeriDosyasiOlustur()
        {
            string dizinYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Stork Shipping";
            if (Directory.Exists(dizinYolu) == false) Directory.CreateDirectory(dizinYolu);
            StreamWriter Yaz = new StreamWriter(dizinYolu + "\\Şehir Verileri.txt");

            Yaz.Write(
                "1,Adana,Hatay-191,Osmaniye-87,Kahramanmaras-192,Kayseri-335,Nigde-207,Mersin-69\n" +
                "2,Adiyaman,Sanliurfa-112,Diyarbakir-207,Malatya-187,Kahramanmaras-163,Gaziantep-150\n" +
                "3,Afyonkarahisar,Isparta-168,Konya-223,Eskisehir-144,Kutahya-100,Usak-115,Denizli-222,Burdur-169\n" +
                "4,Agri,Van-228,Igdir-143,Kars-214,Erzurum-183,Mus-245,Bitlis-234\n" +
                "5,Amasya,Yozgat-200,Tokat-114,Samsun-131,Corum-92\n" +
                "6,Ankara,Konya-258,Aksaray-225,Kirsehir-184,Kirikkale-75,Cankiri-130,Bolu-191,Eskisehir-233\n" +
                "7,Antalya,Mersin-466,Karaman-374,Konya-322,Isparta-130,Burdur-122,Mugla-311\n" +
                "8,Artvin,Rize-161,Erzurum-226,Ardahan-116\n" +
                "9,Aydin,Mugla-99,Denizli-126,Manisa-155,Izmir-126\n" +
                "10,Balikesir,Izmir-176,Manisa-141,Kutahya-228,Bursa-151,Canakkale-198\n" +
                "11,Bilecik,Kutahya-112,Eskisehir-83,Bolu-213,Sakarya-99,Bursa-94\n" +
                "12,Bingol,Diyarbakir-141,Mus-111,Erzurum-177,Erzincan-275,Tunceli-144,Elazig-144\n" +
                "13,Bitlis,Siirt-96,Van-161,Agri-234,Mus-83,Batman-135\n" +
                "14,Bolu,Eskisehir-291,Ankara-191,Cankiri-233,Zonguldak-157,Duzce-45,Sakarya-114,Bilecik-213,Karabuk-134\n" +
                "15,Burdur,Mugla-241,Antalya-122,Isparta-51,Afyonkarahisar-169,Denizli-150\n" +
                "16,Bursa,Balikesir-151,Kutahya-177,Bilecik-94,Sakarya-157,Kocaeli-132,Yalova-69\n" +
                "17,Canakkale,Balikesir-198,Tekirdag-187,Edirne-216\n" +
                "18,Cankiri,Ankara-130,Kirikkale-104,Corum-156,Kastamonu-106,Bolu-233,Karabuk-193\n" +
                "19,Corum,Yozgat-108,Amasya-92,Samsun-172,Sinop-266,Kastamonu-197,Cankiri-156,Kirikkale-166\n" +
                "20,Denizli,Mugla-145,Burdur-150,Afyonkarahisar-222,Usak-150,Manisa-208,Aydin-126\n" +
                "21,Diyarbakir,Sanliurfa-176,Mardin-96,Batman-100,Mus-252,Bingol-141,Elazig-153,Malatya-249,Adiyaman-207\n" +
                "22,Edirne,Canakkale-216,Tekirdag-140,Kirklareli-62\n" +
                "23,Elazig,Diyarbakir-153,Bingol-144,Tunceli-136,Erzincan-267,Malatya-98\n" +
                "24,Erzincan,Elazig-267,Tunceli-243,Bingol-275,Erzurum-190,Bayburt-155,Gumushane-133,Giresun-295,Sivas-248,Malatya-363\n" +
                "25,Erzurum,Bingol-177,Mus-266,Agri-183,Kars-203,Ardahan-233,Artvin-226,Rize-259,Bayburt-233,Erzincan-190\n" +
                "26,Eskisehir,Afyonkarahisar-144,Konya-338,Ankara-233,Bolu-291,Bilecik-83,Kutahya-78\n" +
                "27,Gaziantep,Kilis-64,Sanliurfa-137,Adiyaman-150,Kahramanmaras-76,Osmaniye-125,Hatay-193\n" +
                "28,Giresun,Gumushane-162,Trabzon-136,Erzincan-295,Sivas-298,Ordu-44\n" +
                "29,Gumushane,Erzincan-133,Bayburt-78,Trabzon-100,Giresun-162\n" +
                "30,Hakkari,Van-197,Sirnak-189\n" +
                "31,Hatay,Gaziantep-193,Osmaniye-129,Adana-191\n" +
                "32,Isparta,Antalya-130,Konya-263,Afyonkarahisar-168,Burdur-51\n" +
                "33,Mersin,Adana-69,Nigde-200,Konya-348,Karaman-235,Antalya-466\n" +
                "34,Istanbul,Kocaeli-111,Tekirdag-131\n" +
                "35,Izmir,Aydin-126,Manisa-35,Balikesir-176\n" +
                "36,Kars,Agri-214,Igdir-140,Ardahan-94,Erzurum-203\n" +
                "37,Kastamonu,Corum-197,Sinop-183,Cankiri-106,Bartin-183,Karabuk-114\n" +
                "38,Kayseri,Adana-335,Kahramanmaras-261,Sivas-194,Yozgat-197,Nevsehir-81,Nigde-128\n" +
                "39,Kirklareli,Edirne-62,Tekirdag-121\n" +
                "40,Kirsehir,Nevsehir-91,Yozgat-112,Kirikkale-113,Ankara-184,Aksaray-110\n" +
                "41,Kocaeli,Yalova-65,Istanbul-111,Bursa-132,Sakarya-37\n" +
                "42,Konya,Antalya-322,Karaman-119,Mersin-348,Nigde-242,Aksaray-148,Ankara-258,Eskisehir-338,Afyonkarahisar-223,Isparta-263\n" +
                "43,Kutahya,Manisa-316,Usak-139,Afyonkarahisar-100,Eskisehir-78,Bilecik-112,Bursa-177,Balikesir-228\n" +
                "44,Malatya,Kahramanmaras-224,Adiyaman-187,Diyarbakir-249,Elazig-98,Erzincan-363,Sivas-243\n" +
                "45,Manisa,Izmir-35,Aydin-155,Denizli-208,Usak-195,Kutahya-316,Balikesir-141\n" +
                "46,Kahramanmaras,Gaziantep-76,Adiyaman-163,Malatya-224,Sivas-327,Kayseri-261,Adana-192,Osmaniye-104\n" +
                "47,Mardin,Sanliurfa-188,Diyarbakir-96,Batman-150,Siirt-227,Sirnak-197\n" +
                "48,Mugla,Antalya-311,Burdur-241,Denizli-145,Aydin-99\n" +
                "49,Mus,Diyarbakir-252,Batman-218,Bitlis-83,Agri-245,Erzurum-266,Bingol-111\n" +
                "50,Nevsehir,Nigde-82,Kayseri-81,Yozgat-203,Kirsehir-91,Aksaray-75\n" +
                "51,Nigde,Nevsehir-82,Kayseri-128,Adana-207,Mersin-200,Konya-242,Aksaray-122\n" +
                "52,Ordu,Samsun-150,Tokat-216,Giresun-44,Sivas-314\n" +
                "53,Rize,Artvin-161,Erzurum-259,Bayburt-253,Trabzon-75\n" +
                "54,Sakarya,Duzce-69,Bolu-114,Bilecik-99,Bursa-157,Kocaeli-37\n" +
                "55,Samsun,Ordu-150,Tokat-229,Amasya-131,Corum-172,Sinop-155\n" +
                "56,Siirt,Van-257,Bitlis-96,Batman-86,Mardin-227,Sirnak-100\n" +
                "57,Sinop,Samsun-155,Corum-266,Kastamonu-183\n" +
                "58,Sivas,Kayseri-194,Kahramanmaras-327,Malatya-243,Erzincan-248,Giresun-298,Ordu-314,Tokat-108,Yozgat-224\n" +
                "59,Tekirdag,Istanbul-131,Kirklareli-121,Edirne-140,Canakkale-187\n" +
                "60,Tokat,Sivas-108,Ordu-216,Samsun-229,Amasya-114,Yozgat-205\n" +
                "61,Trabzon,Rize-75,Bayburt-178,Gumushane-100,Giresun-136\n" +
                "62,Tunceli,Elazig-136,Bingol-144,Erzincan-131\n" +
                "63,Sanliurfa,Gaziantep-137,Adiyaman-112,Diyarbakir-176,Mardin-188\n" +
                "64,Usak,Manisa-195,Denizli-150,Afyonkarahisar-115,Kutahya-139\n" +
                "65,Van,Hakkari-197,Sirnak-357,Siirt-257,Bitlis-161,Agri-228\n" +
                "66,Yozgat,Kayseri-197,Sivas-224,Tokat-205,Amasya-200,Corum-108,Kirikkale-140,Kirsehir-112,Nevsehir-203\n" +
                "67,Zonguldak,Bartin-87,Bolu-157,Duzce-113,Karabuk-100\n" +
                "68,Aksaray,Nigde-122,Nevsehir-75,Kirsehir-110,Ankara-225,Konya-148\n" +
                "69,Bayburt,Erzincan-155,Erzurum-125,Rize-253,Trabzon-178,Gumushane-78\n" +
                "70,Karaman,Mersin-235,Konya-119,Antalya-374\n" +
                "71,Kirikkale,Kirsehir-113,Yozgat-140,Corum-166,Cankiri-104,Ankara-75\n" +
                "72,Batman,Mardin-150,Siirt-86,Bitlis-135,Mus-218,Diyarbakir-100\n" +
                "73,Sirnak,Mardin-197,Siirt-100,Van-357,Hakkari-189\n" +
                "74,Bartin,Kastamonu-183,Zonguldak-87,Karabuk-89\n" +
                "75,Ardahan,Kars-94,Erzurum-233,Artvin-116\n" +
                "76,Igdir,Agri-143,Kars-140\n" +
                "77,Yalova,Kocaeli-65,Bursa-69\n" +
                "78,Karabuk,Zonguldak-100,Bartin-89,Kastamonu-114,Cankiri-193,Bolu-134\n" +
                "79,Kilis,Gaziantep-64\n" +
                "80,Osmaniye,Gaziantep-125,Kahramanmaras-105,Adana-87,Hatay-129\n" +
                "81,Duzce,Zonguldak-113,Bolu-45,Sakarya-69");

            Yaz.Flush();
            Yaz.Close();
            Yaz.Dispose();
        }
    }
}