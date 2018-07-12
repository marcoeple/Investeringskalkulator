using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double laan;
        private double laanTilDisposisjon;
        private double egenkapital;
        private double konto;
        private int investeringshorisont;
        private int nedbetalingsaar;
        private double forventetAvkastningProsent;
        private double laanRenteProsent;
        private int sparebelop;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Calculate(object sender, RoutedEventArgs e)
        {
            try
            {
                laan = Convert.ToDouble(txtBoxLaan.Text);
                laanTilDisposisjon = Convert.ToDouble(txtBoxLaanTilDisposisjon.Text);
                egenkapital = Convert.ToDouble(txtBoxEgenkapital.Text);
                konto = laanTilDisposisjon + egenkapital;
                investeringshorisont = Convert.ToInt32(txtBoxInvesteringshorisont.Text);
                nedbetalingsaar = Convert.ToInt32(txtBoxNedbetalingsaar.Text);
                forventetAvkastningProsent = Convert.ToDouble(txtBoxForventetAvkastning.Text);
                laanRenteProsent = Convert.ToDouble(txtBoxLaanRente.Text);
                sparebelop = Convert.ToInt32(txtBoxSparebelop.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Må være tall, separert med ',' ");
                return;
            }

            resultat();
        }


        //Finner summen å betale tilbake
        private double laanBetalingsbelop()
        {
            
            double betalingssum = 0;
            double gjenstaendeLaan = laan;
            for(int i = 0; i < nedbetalingsaar; i++)
            {
                double renter = gjenstaendeLaan * (laanRenteProsent / 100);
                double avdrag = gjenstaendeLaan / (nedbetalingsaar - i);
                gjenstaendeLaan -= avdrag;
                betalingssum += (avdrag + renter);
                //Console.WriteLine("År nr " + (i+1) + ":   Renter: " + renter + "  Avdrag: " + avdrag + " Gjenstående lån: " + gjenstaendeLaan);
            }
            //Console.WriteLine("Betalingssum: " + betalingssum);
            return betalingssum;
        }


        //Finner beholdning for hvert år
        private double resultat()
        {
            double terminbelop = laanBetalingsbelop()/(nedbetalingsaar*12); //beløp per måned

            double[] maanedligBeholdning = new double[investeringshorisont * 12];

            maanedligBeholdning[0] = konto + konto * (forventetAvkastningProsent / (100 * 12)) + sparebelop - terminbelop;

            Console.WriteLine("Måned 1: ");
            Console.WriteLine("Beholdning: " + konto + " Avkastning som skal legges til: " + (int)(konto*(forventetAvkastningProsent/(100*12))) + " Terminbeløp som skal trekkes fra: " + (int)terminbelop + " Sparebeløp: " + sparebelop + " Beholdning slutten av måneden: " + (int)maanedligBeholdning[0]);
            Console.WriteLine("");

            txtBlockResultat.Text = "";
            
            for (int i = 1; i < (investeringshorisont*12); i++)
            {
                maanedligBeholdning[i] = maanedligBeholdning[i - 1] + maanedligBeholdning[i - 1] * (forventetAvkastningProsent / (100 * 12)) + sparebelop - terminbelop;

                Console.WriteLine("Måned " + (i+1) + ": ");
                Console.WriteLine("Beholdning: " + (int)maanedligBeholdning[i-1] + " Avkastning som skal legges til: " + (int)(maanedligBeholdning[i - 1] * (forventetAvkastningProsent / (100 * 12))) + " Terminbeløp som skal trekkes fra: " + (int)terminbelop + " Sparebeløp: " + sparebelop + " Beholdning slutten av måneden: " + (int)maanedligBeholdning[i]);
                Console.WriteLine("");

                if((i+1) % 12 == 0)
                {
                    Console.WriteLine("ETT ÅR");
                    txtBlockResultat.Text += ("Beholdning år " + ((i+1)/12) + ": " + (int)maanedligBeholdning[i] + "\n");
                }

            }

            

            return 0;
        }
    }
}
