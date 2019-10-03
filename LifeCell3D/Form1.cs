using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeCell3D
{
    public partial class Form1 : Form

    {
        public static Graphics g;
        public static SolidBrush redBrush;
        public static SolidBrush neutralBrush;
        public static Pen axesBrush;

        public static int generation; // Отсчет поколений

       // public static int x = 0;
        //public static int y = 0;
        //public static int z = 0;

        public static int MaxXYZ = 250;

        public static int max = 20;

        public static Boolean [,,] Matrix = new Boolean [max,max,max]; // трехмерный массив размерностями max

        public Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
            redBrush = new SolidBrush(Color.Blue);
            neutralBrush = new SolidBrush(this.BackColor);
            axesBrush = new Pen(Color.Black, 3);

            g = this.CreateGraphics();

            generation = 0;

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                  

        }

        private void Form1_Click(object sender, EventArgs e)
        {

            if ( generation == 0)
            {
                g.DrawLine(axesBrush, 250, 250, 500, 250); // ось Х
                g.DrawLine(axesBrush, 250, 250, 250, 1); // ось Y
                g.DrawLine(axesBrush, 250, 250, X1(0, 0, 50), Y1(0, 0, 50)); // ось Z  

                for (int z = 0; z < max; z++)  // инициализация массива заполнением 0,01 полей

                {

                    for (int y = 0; y < max; y++)

                    {

                        for (int x = 0; x < max; x++)

                        {

                            double r = rnd.Next(0, 100);

                            if (r > 90)

                            {
                                Matrix[x, y, z] = true;

                                CreateDot(x, y, z, true);

                            }





                        }

                    }


                }
                generation = 1;
            }

            

            if (timer1.Enabled == true) timer1.Enabled = false;
            else timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Gen1.Text = Convert.ToString(generation);

            Life(generation);
            generation++;
            


        }

        static int X1 (int x, int y, int z) 

        {return (int) (MaxXYZ + 5 * (x - 0.7 * z)); }

        static int Y1(int x, int y, int z)

        {return (int) (MaxXYZ - 5 * (y - 0.7 * z));}

        static void CreateDot(int x, int y, int z, Boolean onoff)

        {

            if (onoff) g.FillEllipse(redBrush, X1(x,y,z), Y1(x,y,z), 3, 3);

            else g.FillEllipse(neutralBrush, X1(x, y, z), Y1(x, y, z), 3, 3);


        }

        static void Life(int gen)

        {
            int quant;
            Boolean whoThere;

            for (int z = 0; z < max; z++)  // обход массива
            {
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)
                    {
                       
                            whoThere = Matrix[x, y, z];

                            quant = NeighborQuantity(x, y, z);
                        

                            if (whoThere)
                            {
                                quant--; // вычитаем из пространства Мура саму центральную ячейку
                                if ((quant < 6) || (quant > 9)) // одиночество или перенаселение с учетом в количестве самой центральной ячейки
                                {
                                    Matrix[x, y, z] = false;
                                    CreateDot(x, y, z, false);

                                }
                            }

                            else
                            {
                                if (quant > 3)  // если количество больше единицы, то размножение

                                {
                                    Matrix[x, y, z] = true;
                                    CreateDot(x, y, z, true);

                                }
                            }

                    }

                }

            }






        }

        static int NeighborQuantity(int x, int y, int z)

        {
            int quantity = 0;

            for (int zz = z - 1; zz < z + 2; zz++)

            {
                for (int yy = y - 1; yy < y + 2; yy++)

                {
                    for (int xx = x - 1; xx < x +2; xx++)

                    {

                        if ((xx >= 0) && (xx < max) && (yy >= 0) && (yy < max) && (zz >= 0) && (zz < max)) // координаты в диапазоне
                        { if (Matrix[xx, yy, zz]) quantity++; } // ячейка заполнена
                          

                    }


                }


            }

            return quantity;



        }

        
    }



}
