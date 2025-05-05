using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace OP_Stachlewski_Krzysztof_App_Filter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        double k = 30000; //Konštanta

        class Butter
        {
            public double a1 = 1.8478;
            public double a2 = 0.7654;
            public double b1 = 1;
            public double b2 = 1;
            public double q1 = 0.54;
            public double q2 = 1.31;

        }

        class Bessel
        {
            public double a1 = 1.3397;
            public double a2 = 0.7743;
            public double b1 = 0.4889;
            public double b2 = 0.3890;
            public double q1 = 0.52;
            public double q2 = 0.81;

        }

        class Chebyshev
        {
            public double a1 = 2.1853;
            public double a2 = 0.1964;
            public double b1 = 5.5339;
            public double b2 = 1.2009;
            public double q1 = 1.08;
            public double q2 = 5.58;

        }

        public double vypocet_c1(double f,double k) 
        {
            double  c1 = k / f;

            return c1;

        }

        public double vypocet_c2(double a1, double b1, double c1) 
        {
            double c2 = ((4 * b1) / a1) * c1;
            return c2;

        }

        public double vypocet_c4(double a2, double b2, double c1)
        {
            double c4 = ((4 * b2) / a2) * c1;
            return c4;
        }

        public double vypocet_r1(double a1, double c1, double f) 
        {
            c1 = c1 / Math.Pow(10, 9);
            double r1 = (1 / (Math.PI * f * c1 * a1));
            return r1;
        }

        public double vypocet_r2(double a1, double b1, double c2, double c1, double f)
        { 
            if (radioButton1.Checked)
            {
                c1 = c1 / Math.Pow(10, 9);
                double r2 = (a1 / (4 * Math.PI * f * c1 * b1));
                return r2;
            }
            else
            {
                c2 = c2 / Math.Pow(10, 9);
                double r2 = (a1 / (4 * Math.PI * f * c2 * b1));
                return r2;
            }
            

        }

        public double vypocet_r3(double a2, double c1, double f)
        {
            c1 = c1 / Math.Pow(10, 9);
            double r3 = (1 / (Math.PI * f * c1 * a2));
            return r3;
        }

        public double vypocet_r4(double a2, double b2, double c4, double c1, double f)
        {
            if (radioButton3.Checked)
            {
                c1 = c1 / Math.Pow(10, 9);
                double r4 = (a2 / (4 * Math.PI * f * c1 * b2));
                return r4;
            }
            else
            {
                c4 = c4 / Math.Pow(10, 9);
                double r4 = (a2 / (4 * Math.PI * f * c4 * b2));
                return r4;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            double f;
            double a1;
            double b1;
            double a2;
            double b2;

            if (double.TryParse(textBox1.Text, out f))
            {    

                double c1 = vypocet_c1(f,k); //Vypocet C1

                
                if (comboBox1.SelectedIndex == 0)   //Butterworth
                {
                    Butter butter = new Butter();
                    a1 = butter.a1;
                    b1 = butter.b1;
                    a2 = butter.a2;
                    b2 = butter.b2;
                }
                else if (comboBox1.SelectedIndex == 1)  //Bessel
                {
                    Bessel bessel = new Bessel();
                    a1 = bessel.a1;
                    b1 = bessel.b1;
                    a2 = bessel.a2;
                    b2 = bessel.b2;
                }
                else if (comboBox1.SelectedIndex == 2)  //Čebyšev
                {
                    Chebyshev chebyshev = new Chebyshev();
                    a1 = chebyshev.a1;
                    b1 = chebyshev.b1;
                    a2 = chebyshev.a2;
                    b2 = chebyshev.b2;
                }
                else
                {
                    MessageBox.Show("Zabudli ste vybrať filter!");
                    return;
                }

                if ((radioButton1.Checked || radioButton2.Checked) && (radioButton3.Checked || radioButton4.Checked))
                {
                    double c2 = vypocet_c2(a1, b1, c1);         //Vypocet C2
                    double c4 = vypocet_c4(a2, b2, c1);         //Vypocet C3

                    double r1 = vypocet_r1(a1, c1, f);          //vypocet R1
                    double r2 = vypocet_r2(a1, b1, c2, c1, f);  //vypocet R2
                    double r3 = vypocet_r3(a2, c1, f);          //Vypocet R3
                    double r4 = vypocet_r4(a2, b2, c4, c1, f);  //Vypocet R4

                    DialogResult prepocet = MessageBox.Show("Chcete výsledné hodnoty prepočítať na KΩ?", "Prepočet", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (prepocet == DialogResult.Yes) 
                    {
                        double[] hodnoty = { r1, r2, r3, r4 };
                        for (int i = 0; i < hodnoty.Length; i++)
                        {
                            hodnoty[i] = hodnoty[i] / 1000;
                            (r1, r2, r3, r4) = (hodnoty[0], hodnoty[1], hodnoty[2], hodnoty[3]);
                        }

                    textBox8.Text = $"C1 = {c1:F2} nF{Environment.NewLine}" +
                                    $"C2 = {c2:F2} nF{Environment.NewLine}" +
                                    $"C3 = {c1:F2} nF{Environment.NewLine}" +
                                    $"C4 = {c4:F2} nF{Environment.NewLine}" +
                                    $"_______________{Environment.NewLine}" +
                                    $"R1 = {r1:F2} kΩ {Environment.NewLine}" +
                                    $"R2 = {r2:F2} kΩ {Environment.NewLine}" +
                                    $"R3 = {r3:F2} kΩ {Environment.NewLine}" +
                                    $"R4 = {r4:F2} kΩ {Environment.NewLine}" ;

                    label19.Text = $"{c1:F2} nF";
                    label16.Text = $"{c2:F2} nF";
                    label12.Text = $"{r1:F2} kΩ";
                    label13.Text = $"{r2:F2} kΩ";
                    label18.Text = $"{c1:F2} nF";
                    label17.Text = $"{c4:F2} nF";
                    label14.Text = $"{r3:F2} kΩ";
                    label15.Text = $"{r4:F2} kΩ";
                    }
                    else if (prepocet == DialogResult.No)
                    {
                    textBox8.Text = $"C1 = {c1:F2} nF{Environment.NewLine}" +
                                    $"C2 = {c2:F2} nF{Environment.NewLine}" +
                                    $"C3 = {c1:F2} nF{Environment.NewLine}" +
                                    $"C4 = {c4:F2} nF{Environment.NewLine}" +
                                    $"_______________{Environment.NewLine}" +
                                    $"R1 = {r1:F2} Ω {Environment.NewLine}" +
                                    $"R2 = {r2:F2} Ω {Environment.NewLine}" +
                                    $"R3 = {r3:F2} Ω {Environment.NewLine}" +
                                    $"R4 = {r4:F2} Ω {Environment.NewLine}";

                    label19.Text = $"{c1:F2} nF";
                    label16.Text = $"{c2:F2} nF";
                    label12.Text = $"{r1:F2} Ω";
                    label13.Text = $"{r2:F2} Ω";
                    label18.Text = $"{c1:F2} nF";
                    label17.Text = $"{c4:F2} nF";
                    label14.Text = $"{r3:F2} Ω";
                    label15.Text = $"{r4:F2} Ω";

                    }


                }
                else
                {
                    MessageBox.Show("Vyberte či sa majú kondenzátory rovnať!");
                }
            }   
            else
            {
                MessageBox.Show($"Pozor zabudli ste zadať hodnotu frekvencie!");
            }            

        }

    }
}
