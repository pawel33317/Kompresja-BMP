using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication4
{
    class spam
    {

        /*
        int nzl = 0;
        bool zn = false;
        byte niewystepujacyZnak = 0;

        //wyszukuje niewystepujacy znak ktory bedzie przecinkiem (zaklada, ze jest taki zank jak nie to sie wysypie)
        while (zn == false)
        {
            niewystepujacyZnak = (byte)nzl;
            zn = true;
            for (int i = 0; i < obrazek.Width; i++)
            {
                for (int j = 0; j < obrazek.Height; j++)
                {
                    if (macierzR[i,j] == niewystepujacyZnak)
                        zn = false;
                    break;
                }
                if(!zn)
                    break;
            }
            nzl++;
        }
        label2.Text = niewystepujacyZnak.ToString();
        */



        /*
//wyświetla tablice koloru R bez przekształcenia
for (int i = 0; i < obrazek.Width; i++)
{
    for (int j = 0; j < obrazek.Height; j++)
    {
        textBox1.Text += macierzR[i,j].ToString() + " ";
    }
    textBox1.Text += "--------------------------------------------------\n";
}
*/

        /*
        //wyświetla tablice koloru R po przekształceniu
        for (int i = 0; i < rr + 1; i++)
        {
            textBox1.Text += macr[i].ToString() + " ";
        }
         
        */






        //odczyt z tablic z programu nie z pliku

        /*
         * 
            //potrzebne : 
            //wysokość obrazu,  obrazek.Height
            //szerokość obrazu, obrazek.Width
            //rozmiar tabeli R  rr+1
            //tabela R          macr


            //p - przeskakuje na kolejna wartosc [ilosc,kolor]
            //g - iterator ilosci
            //k - szerokosc
            //l - wysokosc
            //w - iterator każdego piksela
            int p=0, k = 0, l = 0, g=0, w=0;
            byte[,] dcmr = new byte[obrazek.Width, obrazek.Height];
            while (p < rr+1)
            {
                g = 0;
                while (g < macr[p])
                {
                    dcmr[l, k] = macr[p + 1];
                    w++;
                    g++;
                    if ((w % obrazek.Width == 0) && w!=0)
                    {
                        w = 0;
                        k = 0;
                        l++;
                    }
                    else
                    {
                        k++;
                    }
                }
                p += 2;
            }
            
            p = 0; k = 0; l = 0; g = 0; w = 0;
            byte[,] dcmg = new byte[obrazek.Width, obrazek.Height];
            while (p < rg + 1)
            {
                g = 0;
                while (g < macg[p])
                {
                    dcmg[l, k] = macg[p + 1];
                    w++;
                    g++;
                    if ((w % obrazek.Width == 0) && w != 0)
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

            p = 0; k = 0; l = 0; g = 0; w = 0;
            byte[,] dcmb = new byte[obrazek.Width, obrazek.Height];
            while (p < rb + 1)
            {
                g = 0;
                while (g < macb[p])
                {
                    dcmb[l,k] = macb[p + 1];
                    w++;
                    g++;
                    if ((w % obrazek.Width == 0) && w != 0)
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
         * 
         * 
         *           Color tmp;
            Image obrazekN = new Bitmap(macierzR.GetLength(0), macierzR.GetLength(1), PixelFormat.Format24bppRgb);
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
         * */
    }
}
