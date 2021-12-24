using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppsDelegateVeEvent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SayiKontrol sayiKontroluNesnesi = new SayiKontrol();
        
        public void MerhabaDiyeSelamVer()
        {
            //MessageBox.Show("Merhaba!");
        }

        //Bir delege tanımladık.
        //Void ve parametre almayan metotları temsil edebilir.
        public delegate void CalismaHandler();
        public delegate int IslemYapHandler(int x, int y);
        private void Form1_Load(object sender, EventArgs e)
        {
            //Bir metodu onun adını yazarak çağırıyoruz.
            //MerhabaDiyeSelamVer();

            //Metodumuzu delege çağırsın diye delege tipinde bir nesne türettik o nesneye çalışmasını istediğimiz "target" hedef metodun ismini parametre olarak verildi.
            //Artık metot delege aracılığıyla çağırılacak ve metodun içinde ne işlem yapılmışsa ekranda göreceğiz.
            CalismaHandler benim_calismaHandlerim = new CalismaHandler(MerhabaDiyeSelamVer);
            //1.yol sadece delege nesnesini çağırmak.
            //benim_calismaHandlerim();
            //2.yol delege nesnesinin INVOKE metodunu çağırmak.
            //benim_calismaHandlerim.Invoke();
            //3.yol delege nesnesinin BEGININVOKE metoduna çalışmasını istediğimiz metodun ismini parametre olarak vermek.
            this.BeginInvoke(benim_calismaHandlerim);
            button3.Click += new EventHandler(button3_Click);
            button3.MouseMove += new MouseEventHandler(button3_FareUzerineGeldi);
            //Event'ların tipi delege'dir.
            //+= ile event oluşturmak istediğinizde event tanımlanırken tip olarak hangi delege verildiyse kandisine o delege geliyor.
            //Delegenin imzası yani geriye dönüş tipi parametreleri ne ise event'a atanacak metodun imzasının da aynı olması gerekir.
            //Örneğin PaintEventHandler içine object tipte bir sender ve PaintEventArgs tipinde e isimli parametreler almış.
            // Bu nedenle button3_Paint isimli metot da aynı imzaya yani aynı geri dönüş tipi ve aynı parametrelere sahip olmalıdır
            //Böylece o delege kendisine constructor'da atanan metodu temsil edebiliyor.
            button3.Paint += new PaintEventHandler(button3_Paint);
            sayiKontroluNesnesi.SayiDurumu += new SayiKontrol.SayiKontrolEtHandler(Kontrol);
        }
        private void button3_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show("Button3'nin paint event'ı çalışıyor");
        }
        private void button3_FareUzerineGeldi(object sender,EventArgs e)
        {
            button3.BackColor = Color.Pink;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button3'ye tıkladınız");
        }
        public int Topla(int s1,int s2)
        {
            return s1 + s2;
        }
        public int Carp(int s1, int s2)
        {
            return s1 * s2;
        }

        private void btnSonuc_Click(object sender, EventArgs e)
        {
            try
            {
                IslemYapHandler benimIslemDelegem = new IslemYapHandler(Topla);
                //+= ile Carp metodunu verince bu delegenin sonucunda iki sayı çarpılır.
                //Bu messageBox iki sayıyı toplayıp işlem yapar
                MessageBox.Show(benimIslemDelegem.Method.Name + " işlemi yapılacaktır.\n Sonuç: " + benimIslemDelegem(Convert.ToInt32(txtSayi1.Text), Convert.ToInt32(txtSayi2.Text)));
                // burada carp metodu delegeye atandığı için çarpma işlemi yapılır
                benimIslemDelegem += Carp;

                MessageBox.Show(benimIslemDelegem.Method.Name + " işlemi yapılacaktır.\n Sonuç: " + benimIslemDelegem(Convert.ToInt32(txtSayi1.Text), Convert.ToInt32(txtSayi2.Text)));

                //-= ile Carp metodunu delege temsilinden çıkarırsak bir önceki temsil ettiği metodun işlemini yapıp iki sayıyı toplayacaktır
                benimIslemDelegem -= Carp;
                // Carp metodu temsilden çıkarıldığı için toplama işlemi yapar
                MessageBox.Show(benimIslemDelegem.Method.Name + " işlemi yapılacaktır.\n Sonuç: " + benimIslemDelegem(Convert.ToInt32(txtSayi1.Text), Convert.ToInt32(txtSayi2.Text)));
            }
            catch (Exception ex)
            {

                MessageBox.Show("HATA:" + ex.Message);
            }
        }

        private void btnIslem_Click(object sender, EventArgs e)
        {
            IslemYapHandler benimIslemDelegem = new IslemYapHandler(Topla);
            benimIslemDelegem += Carp;
            Delegate[] delegetiminMetotları = benimIslemDelegem.GetInvocationList();
            foreach (Delegate item in delegetiminMetotları)
            {
                string metotParametreBilgileri = string.Empty;
                ParameterInfo[] itemParameters = item.Method.GetParameters();
                foreach (var theParameter in itemParameters)
                {
                    metotParametreBilgileri += $"Parametre Adı: {theParameter.Name}" + $"\nParametrenin Tipi: {theParameter.ParameterType.Name}\n";
                }
                MessageBox.Show($"Çalışan metodun adı: {item.Method.Name}" + $"\nÇalışan metdoun geriye dönüş tipi: {item.Method.ReturnType}" +$"\nÇalışan metodun parametre bilgileri:" + $"\n{metotParametreBilgileri}" +$"\n Sonuç: {item.DynamicInvoke(Convert.ToInt32(txtSayi1.Text), Convert.ToInt32(txtSayi2.Text))}");

            }
        }
        public void Kontrol()
        {
            MessageBox.Show(sayiKontroluNesnesi.SayiDurumMesaji);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SayiKontrol sayiKontrolNesnem = new SayiKontrol();
            sayiKontrolNesnem.SayiDurumu += new SayiKontrol.SayiKontrolEtHandler(Kontrol);
            sayiKontrolNesnem.Sayi = Convert.ToInt32(txtSayi1.Text);
            
        }

        private void btnSonucPozitif_Click(object sender, EventArgs e)
        {
            sayiKontroluNesnesi.Sayi = Convert.ToInt32(txtSayi1.Text);
            if (sayiKontroluNesnesi.SayiKontroluGectiMi==false)
            {
                return;
            }
            sayiKontroluNesnesi.Sayi = Convert.ToInt32(txtSayi2.Text);
            if (!sayiKontroluNesnesi.SayiKontroluGectiMi)
            {
                return;
            }
            //hem sayi1 hem desayi2 >=0 ise aşağıdaki işlemler gerçekleşsin.
            IslemYapHandler islemim = new IslemYapHandler(Topla);
            MessageBox.Show("Toplama sonuç:" + islemim.Invoke(Convert.ToInt32(txtSayi1.Text), Convert.ToInt32(txtSayi2.Text)));

        }
    }
}
