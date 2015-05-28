using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
namespace WindowsFormsApplication4
{
    //Autor Paweł Czubak 171912 pawel33317@gmail.com

    //STRUKTURA PLIKU MPA
    //32 bity  wysokość pliku     H
    //32 bity  szerokość pliku    W
    //32 bity  rozmiar tablicy R  R
    //32 bity  rozmiar tablicy G  G
    //32 bity  rozmiar tablicy B  B
    //R bajtów tablica R (1 bajt jeden element tablicy)
    //G bajtów tablica G (1 bajt jeden element tablicy)
    //B bajtów tablica B (1 bajt jeden element tablicy)
    //na końcu 1 bit == true w celu sprawdzenia poprawności
    //załadowanie obrazka z Resources w projekcie

    //w klasie spam są różne funkcje którymi bawiłem się jednak się nie przydały

    public partial class Form1 : Form
    {
        Bitmap obrazek;
        String nazwaPliku ="";
        public Form1()
        {
            InitializeComponent();
        }

        //po załadowaniu plików konwersji wyświetleniu itp pobiera ich rozmiar i wyświetla
        public void rozmiary()
        {
            FileInfo info = new FileInfo(@"Resources/" + nazwaPliku + ".bmp");
            label6.Text = info.Length.ToString();
            info = new FileInfo(@"Resources/" + nazwaPliku + ".mpa");
            label7.Text = info.Length.ToString();
        }

        //funkcja główna odpowiadająca za załadowanie przetworzenie wyswietlenie skompresowanie itp
        public void load(String nazwa)
        {
            nazwaPliku = nazwa;

            //ładowanie pliku bmp jest nawet obsługa wyjątku xD
            try
            {
                obrazek = new Bitmap(@"Resources/" + nazwaPliku + ".bmp", true);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("brak pliku");
            }


            //macierze o rozmiarach obrazka na palety kolorów R G B
            byte[,] macierzR = new byte[obrazek.Width, obrazek.Height];
            byte[,] macierzG = new byte[obrazek.Width, obrazek.Height];
            byte[,] macierzB = new byte[obrazek.Width, obrazek.Height];


            //wypełnienie macierzy kolorami z obrazka
            //pobiera kolory i wpisuje macierzy
            for (int i = 0; i < obrazek.Width; i++)
            {
                for (int j = 0; j < obrazek.Height; j++)
                {
                    macierzR[i, j] = obrazek.GetPixel(i, j).R;
                    macierzG[i, j] = obrazek.GetPixel(i, j).G;
                    macierzB[i, j] = obrazek.GetPixel(i, j).B;
                }
            }
            pictureBox1.Image = obrazek;


            //zmienne na długość tablicy po skompresowaniu
            int rr = 0, rg = 0, rb = 0;

            //tablice na skompresowane kolory na razie będą duże gdyż nasza kompresja dla niektórych plików może okazać się antykompresją 
            //w najgorszym wypadku 2x większy rozmiar pliku
            byte[] macr = new byte[obrazek.Width * obrazek.Height * 2];
            byte[] macg = new byte[obrazek.Width * obrazek.Height * 2];
            byte[] macb = new byte[obrazek.Width * obrazek.Height * 2];

            //pierwsze przypisanie nie zrobione w pętli, żeby pominąć dodatkowy if w petli
            //podaje tylko pierwszy kolor i jego ilość wystąpień ustawia na 0 ale pętla oczywiście to poprawi
            macr[0] = 0;
            macr[1] = macierzR[0, 0];
            macg[0] = 0;
            macg[1] = macierzR[0, 0];
            macb[0] = 0;
            macb[1] = macierzR[0, 0];

            //kompresowanie 
            //budowa [ilośćWystąpieńKoloru,kolor,ilośćWystąpieńKoloru,kolor,ilośćWystąpieńKoloru,kolor,ilośćWystąpieńKoloru,kolor,....]
            for (int i = 0; i < obrazek.Width; i++)
            {
                for (int j = 0; j < obrazek.Height; j++)
                {
                    //jeżeli kolejny piksel ma taki sam kolor to nie dopisuje go do nowej tablicy tylko zwiększa ilość wystąpień poprzedniego
                    //jeżeli przekraczamy wartość max byte to mimo, że kolejny kolor jest taki sam ustawi go jako nowy
                    //Dla Palety R
                    if (macierzR[i, j] == macr[rr + 1] && macr[rr] < 255)
                    {
                        //zwiększenie licznika
                        macr[rr]++;
                    }
                    //Jeżeli nowy kolor jest inny niż poprzedni dodaje go i ustawia jego licznik na 1
                    else
                    {
                        //+2 bo prezesuwamy o kolor i jego licznik
                        rr += 2;
                        //licznik nowego koloru na 1
                        macr[rr] = 1;
                        //przypisanie nowego koloru
                        macr[rr + 1] = macierzR[i, j];
                    }
                    //analogicznie dla palety G
                    if (macierzG[i, j] == macg[rg + 1] && macg[rg] < 255)
                    {
                        macg[rg]++;
                    }
                    else
                    {
                        rg += 2;
                        macg[rg] = 1;
                        macg[rg + 1] = macierzG[i, j];
                    }
                    //analogicznie dla palety B
                    if (macierzB[i, j] == macb[rb + 1] && macb[rb] < 255)
                    {
                        macb[rb]++;
                    }
                    else
                    {
                        rb += 2;
                        macb[rb] = 1;
                        macb[rb + 1] = macierzB[i, j];
                    }

                }

            }
            //jako żę przyjeliśmy iż nasze tablice będą miały największy możliwy rozmiar a tak oczywiście nie jest to je zmniejszamy 
            //do rozmiaru danych +2 bo trzeba przesunąć jeszcze o kolor gdyż było ustawione na licznik który jest przed ostatnim kolorem oraz dla tego że,
            //podajemy rozmiar a nie ostatni element tablicy który jest pomniejszony o 1 bo zaczyna się od zera
            Array.Resize(ref macr, rr + 2);
            Array.Resize(ref macg, rg + 2);
            Array.Resize(ref macb, rb + 2);


            //ZAPIS DO PLIKU
            //nazwa taka sam tylko inne rozszerzenie
            string fileName = "Resources/" + nazwaPliku + ".mpa";

            //Stream w trybie zapisu i writer
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter wr = new BinaryWriter(fs);

            //zapisujemy do pliku: 
            //wysokość 
            wr.Write((int)obrazek.Height);
            //szerokość
            wr.Write((int)obrazek.Width);
            //rozmiar tablicy R
            wr.Write((int)(rr + 2));
            //rozmiar tablicy G
            wr.Write((int)(rg + 2));
            //rozmiar tablicy B
            wr.Write((int)(rb + 2));

            //zapisujemy bajt po bajcie kolejne elementy tablicy R
            for (int i = 0; i < rg + 2; i++)
            {
                wr.Write((byte)(macg[i]));
            }
            //zapisujemy bajt po bajcie kolejne elementy tablicy G
            for (int i = 0; i < rb + 2; i++)
            {
                wr.Write((byte)(macb[i]));
            }
            //zapisujemy bajt po bajcie kolejne elementy tablicy B
            for (int i = 0; i < rr + 2; i++)
            {
                wr.Write((byte)(macr[i]));
            }
            //ostatni bit zapisujemy wartość true tak w celu sprawdzenia
            wr.Write((bool)(true));
            fs.Close();

            //ODCZYT Z PLIKU
            //stream w trybie odczytu oraz reader
            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            //odczytujemy szerokość i długość
            int obrazHeight = r.ReadInt32();
            int obrazWidth = r.ReadInt32();
            //odczytujemy rozmiary tablic RGB
            int rsiz = r.ReadInt32();
            int gsiz = r.ReadInt32();
            int bsiz = r.ReadInt32();

            //tworzymy tablice na macierze RGB o wcześniej pobranym rozmiarze
            byte[] ffr = new byte[rsiz];
            byte[] ffg = new byte[gsiz];
            byte[] ffb = new byte[bsiz];

            //wyciągamy bajt tablicę kolorów R 
            //jest finkcja, że można pobrać od razu całość ale nie chciało mi się bawić
            //można zrobić metodę żeby nie pisać 3 razy tego samego
            for (int i = 0; i < gsiz; i++)
            {
                ffg[i] = r.ReadByte();
            }
            for (int i = 0; i < bsiz; i++)
            {
                ffb[i] = r.ReadByte();
            } for (int i = 0; i < rsiz; i++)
            {
                ffr[i] = r.ReadByte();
            }
            //ostatni bit powinien mieć wartość true
            label9.Text = r.ReadBoolean().ToString();
            r.Close();
            fs.Close();


            //potrzebne : 
            //wysokość obrazu,  obrazek.Height  obrazHeight
            //szerokość obrazu, obrazek.Width   obrazWidth
            //rozmiar tabeli R  rr+1            rsiz
            //tabela R          macr            ffr

            //p - przeskakuje na kolejna wartosc [ilosc,kolor]
            //g - iterator ilosci
            //k - szerokosc
            //l - wysokosc
            //w - iterator każdego piksela

            //MOŻNA ZROBI METODĘ ŻEBY NIE PISA TEGO SAMEGO
            int p = 0, k = 0, l = 0, g = 0, w = 0;
            //tablica dwuwymiarowa, żeby odtworzyć BMP
            //można napisać używająć Graphics program do bezpośredniego wyświetlania tego formatu ale nie chciało mi się 
            byte[,] dcmr = new byte[obrazWidth, obrazHeight];

            //konwersja do tablicy R do formatu na bmp
            //pętla przeskakuje o 2 miejsca czyli na następny licznik kolejnego koloru
            while (p < rsiz - 1)
            {
                g = 0;
                //pętla wykonuje się tyle razy ile licznik koloru
                while (g < ffr[p])
                {
                    //przypisanie koloru
                    dcmr[l, k] = ffr[p + 1];
                    w++;
                    g++;

                    //sprawdzenie czy to już kolejny wiersz
                    if ((w % obrazWidth == 0) && w != 0)
                    {
                        w = 0;
                        k = 0;
                        l++;
                    }
                    //jeśli ten sam wiersz to kolejna kolumna
                    else
                    {
                        k++;
                    }
                }
                p += 2;
            }

            //analogicznie dla tablicy G
            p = 0; k = 0; l = 0; g = 0; w = 0;
            byte[,] dcmg = new byte[obrazWidth, obrazHeight];
            while (p < gsiz - 1)
            {
                g = 0;
                while (g < ffg[p])
                {
                    dcmg[l, k] = ffg[p + 1];
                    w++;
                    g++;
                    if ((w % obrazWidth == 0) && w != 0)
                    {
                        k = 0;
                        w = 0;
                        l++;
                    }
                    else
                    {
                        k++;
                    }
                }
                p += 2;
            }

            //analogicznie dla tablicy B
            p = 0; k = 0; l = 0; g = 0; w = 0;
            byte[,] dcmb = new byte[obrazWidth, obrazHeight];
            while (p < bsiz - 1)
            {
                g = 0;
                while (g < ffb[p])
                {
                    dcmb[l, k] = ffb[p + 1];
                    w++;
                    g++;
                    if ((w % obrazWidth == 0) && w != 0)
                    {
                        k = 0;
                        w = 0;
                        l++;
                    }
                    else
                    {
                        k++;
                    }
                }
                p += 2;
            }

            //tworzy bmp z normalnic tablic RGB
            Color tmp;
            Image obrazekN = new Bitmap(obrazWidth, obrazHeight, PixelFormat.Format24bppRgb);
            Bitmap ObrazekN = new Bitmap(obrazekN);
            for (int i = 0; i < obrazek.Width; i++)
            {
                for (int j = 0; j < obrazek.Height; j++)
                {
                    tmp = Color.FromArgb(dcmr[i, j], dcmg[i, j], dcmb[i, j]);
                    ObrazekN.SetPixel(i, j, tmp);
                }
            }
            pictureBox2.Image = ObrazekN;
            rozmiary();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load(textBox4.Text);
        }
    }
}
