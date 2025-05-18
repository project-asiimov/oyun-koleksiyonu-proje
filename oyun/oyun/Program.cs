using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOyunKoleksiyonuYonetimi
{
    public class Oyun
    {
        public string Ad { get; set; }
        public string Tur { get; set; }
        public string Platform { get; set; }
        private List<double> degerlendirmeler;

        public Oyun(string ad, string tur, string platform)
        {
            Ad = ad;
            Tur = tur;
            Platform = platform;
            degerlendirmeler = new List<double>();
        }
        public void Degerlendir(double puan)
        {
            if (puan < 0 || puan > 100)
            {
                Console.WriteLine("Lütfen 0 ile 100 arasında bir puan giriniz.");
                return;
            }
            degerlendirmeler.Add(puan);
            Console.WriteLine($"{Ad} için {puan} puan eklendi.");
        }
        public double OrtalamaPuani()
        {
            if (degerlendirmeler.Count == 0)
                return 0;
            return degerlendirmeler.Average();
        }
        public string OneriAl()
        {
            double ort = OrtalamaPuani();
            if (ort >= 80)
                return $"{Ad} Kesinlikle oynamalısın!";
            else if (ort >= 50)
                return $"{Ad} Denemeye değer.";
            else
                return $"{Ad} Kötü bu oyun oynama kral.";
        }

        public override string ToString()
        {
            return $"Oyun: {Ad}, Tür: {Tur}, Platform: {Platform}, Ortalama Puan: {OrtalamaPuani():F2}";
        }
    }
    public class Koleksiyon
    {
        public List<Oyun> Oyunlar { get; private set; }

        public Koleksiyon()
        {
            Oyunlar = new List<Oyun>();
        }
        public void OyunEkle(Oyun oyun)
        {
            if (oyun == null)
            {
                Console.WriteLine("Geçersiz oyun eklendi.");
                return;
            }
            Oyunlar.Add(oyun);
            Console.WriteLine($"{oyun.Ad} koleksiyona eklendi.");
        }
        public void OyunSil(string oyunAdi)
        {
            Oyun oyun = Oyunlar.FirstOrDefault(o => o.Ad.Equals(oyunAdi, StringComparison.OrdinalIgnoreCase));
            if (oyun != null)
            {
                Oyunlar.Remove(oyun);
                Console.WriteLine($"{oyunAdi} koleksiyondan çıkarıldı.");
            }
            else
            {
                Console.WriteLine($"{oyunAdi} koleksiyonda bulunamadı.");
            }
        }
        public void KoleksiyonuGoster()
        {
            Console.WriteLine("Koleksiyonunuzdaki Oyunlar:");
            if (Oyunlar.Count == 0)
            {
                Console.WriteLine("Koleksiyonunuz boş.");
                return;
            }
            foreach (var oyun in Oyunlar)
            {
                Console.WriteLine(oyun.ToString());
            }
        }

        public void OneriYap()
        {
            Console.WriteLine("Oyun Önerileri:");
            foreach (var oyun in Oyunlar)
            {
                Console.WriteLine(oyun.OneriAl());
            }
        }
    }

    public class Oyuncu
    {
        public string Ad { get; set; }
        public Koleksiyon Koleksiyon { get; private set; }
        public List<Oyun> FavoriOyunlar { get; private set; }

        public Oyuncu(string ad)
        {
            Ad = ad;
            Koleksiyon = new Koleksiyon();
            FavoriOyunlar = new List<Oyun>();
        }

        public void FavoriEkle(Oyun oyun)
        {
            if (oyun == null)
            {
                Console.WriteLine("Geçersiz oyun.");
                return;
            }
            if (!Koleksiyon.Oyunlar.Contains(oyun))
            {
                Console.WriteLine("Oyun, koleksiyonunuzda bulunmadığından favorilere eklenemiyor.");
                return;
            }
            if (!FavoriOyunlar.Contains(oyun))
            {
                FavoriOyunlar.Add(oyun);
                Console.WriteLine($"{oyun.Ad} favori oyunlarınıza eklendi.");
            }
            else
            {
                Console.WriteLine($"{oyun.Ad} zaten favorilerinizde.");
            }
        }
        public void FavorileriGoster()
        {
            Console.WriteLine($"{Ad}'nin Favori Oyunları:");
            if (FavoriOyunlar.Count == 0)
            {
                Console.WriteLine("Favori listeniz boş.");
                return;
            }
            foreach (var oyun in FavoriOyunlar)
            {
                Console.WriteLine(oyun.ToString());
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Nick sal: ");
            string oyuncuAdi = Console.ReadLine();
            DisplaySplashScreen(oyuncuAdi);

            Oyuncu oyuncu = new Oyuncu(oyuncuAdi);

            bool cikis = false;
            while (!cikis)
            {
                DisplayMainMenu();
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        Console.Write("Oyun Adını Giriniz: ");
                        string ad = Console.ReadLine();
                        Console.Write("Oyun Türünü Giriniz: ");
                        string tur = Console.ReadLine();
                        Console.Write("Oyun Platformunu Giriniz: ");
                        string platform = Console.ReadLine();

                        Oyun yeniOyun = new Oyun(ad, tur, platform);
                        oyuncu.Koleksiyon.OyunEkle(yeniOyun);
                        break;

                    case "2":
                        Console.Write("Silmek istediğiniz oyunun adını giriniz: ");
                        string silAd = Console.ReadLine();
                        oyuncu.Koleksiyon.OyunSil(silAd);
                        break;

                    case "3":
                        oyuncu.Koleksiyon.KoleksiyonuGoster();
                        break;

                    case "4":
                        Console.Write("Değerlendirme yapmak istediğiniz oyunun adını giriniz: ");
                        string degerlendirilecekOyun = Console.ReadLine();
                        Oyun oyunToRate = oyuncu.Koleksiyon.Oyunlar.FirstOrDefault(o => o.Ad.Equals(degerlendirilecekOyun, StringComparison.OrdinalIgnoreCase));
                        if (oyunToRate != null)
                        {
                            Console.Write("Oyuna vereceğiniz puanı giriniz (0-10): ");
                            if (double.TryParse(Console.ReadLine(), out double puan))
                            {
                                oyunToRate.Degerlendir(puan);
                            }
                            else
                            {
                                Console.WriteLine("Geçerli bir puan giriniz.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Belirtilen oyun bulunamadı.");
                        }
                        break;

                    case "5":
                        Console.Write("Favorilere eklemek istediğiniz oyunun adını giriniz: ");
                        string favOyunAd = Console.ReadLine();
                        Oyun favOyun = oyuncu.Koleksiyon.Oyunlar.FirstOrDefault(o => o.Ad.Equals(favOyunAd, StringComparison.OrdinalIgnoreCase));
                        if (favOyun != null)
                            oyuncu.FavoriEkle(favOyun);
                        else
                            Console.WriteLine("Belirtilen oyun bulunamadı.");
                        break;

                    case "6":
                        oyuncu.FavorileriGoster();
                        break;

                    case "7":
                        oyuncu.Koleksiyon.OneriYap();
                        break;

                    case "0":
                        cikis = true;
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyiniz.");
                        break;
                }
            }

            Console.WriteLine("Program sonlandırılıyor...");
        }
        static void DisplaySplashScreen(string oyuncuAdi)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("**************************************************");
            Console.WriteLine("*                                                *");
            Console.WriteLine("*        VIDEO OYUN KOLEKSİYONU YÖNETİMİ         *");
            Console.WriteLine("*                                                *");
            Console.WriteLine("**************************************************");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hoş geldiniz, {oyuncuAdi}!");
            Console.ResetColor();
            Console.WriteLine();
        }
        static void DisplayMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("| 1. Oyun Ekle                                  |");
            Console.WriteLine("| 2. Oyun Sil                                   |");
            Console.WriteLine("| 3. Koleksiyonu Görüntüle                      |");
            Console.WriteLine("| 4. Oyuna Değerlendirme Ekle                   |");
            Console.WriteLine("| 5. Favorilere Oyun Ekle                       |");
            Console.WriteLine("| 6. Favorileri Göster                          |");
            Console.WriteLine("| 7. Oyun Önerilerini Göster                    |");
            Console.WriteLine("| 0. Çıkış                                      |");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Seçiminizi yapınız: ");
            Console.ResetColor();
        }
    }
}