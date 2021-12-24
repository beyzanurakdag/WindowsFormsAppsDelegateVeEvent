using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppsDelegateVeEvent
{
    public class SayiKontrol
    {
        //Bir class'taki event'a başka bir class'taki metodu bağlayacağız.
        public delegate void SayiKontrolEtHandler();
        //event'lara tip olarak delege veriliyor.
        public event SayiKontrolEtHandler SayiDurumu;
        public string SayiDurumMesaji { get; set; } = "Negatif sayı girdiniz! Lütfen sıfır veya pozitif sayı giriniz";
        private int _sayi;
        public int Sayi
        {
            get
            {
                return _sayi;
            }
            set
            {
                _sayi = value;
                if (_sayi<0)
                {
                    //Olay(event) çalışsın.
                    if (SayiDurumu!=null)
                    {
                        SayiKontroluGectiMi = false;
                        SayiDurumu();
                    }
                }
                else
                {
                    SayiKontroluGectiMi = true;
                }
            }
        }
        public bool SayiKontroluGectiMi { get; set; }
    }
}
