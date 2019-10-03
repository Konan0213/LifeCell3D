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

        public struct Cell

        { 
            public Boolean currentStatus;

            public Boolean nextStatus;

        }

        public static int generation; // Отсчет поколений

        public static int MaxXYZ = 250;

        public static int max = 40;

        public static int qBorn = 2;

        public static int qDead = 5;

        public static int procent = 97;


        public static Cell [,,] Matrix = new Cell [max,max,max]; // трехмерный массив структур Cell размерностями max

        public Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
            redBrush = new SolidBrush(Color.Blue);
            neutralBrush = new SolidBrush(this.BackColor);
            axesBrush = new Pen(Color.Black, 3);

            

            

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
            generation = 0;
        }

        private void Form1_Click(object sender, EventArgs e)
        {

           
                g.DrawLine(axesBrush, 250, 250, 500, 250); // ось Х
                g.DrawLine(axesBrush, 250, 250, 250, 1); // ось Y
                g.DrawLine(axesBrush, 250, 250, X1(0, 0, 50), Y1(0, 0, 50)); // ось Z  

            if (generation == 0)
            {

                for (int z = 0; z < max; z++)  // инициализация массива заполнением полей
                {
                    for (int y = 0; y < max; y++)
                    {
                        for (int x = 0; x < max; x++)
                        {
                            Single r = rnd.Next(0, 100);

                            if (r > procent)
                            {
                                Matrix[x, y, z].currentStatus = true;
                                CreateDot(x, y, z, true);
                            }

                            // else Matrix[x, y, z].currentStatus = false;  // надо, не надо - хз
                        }


                    }

                }
                
            }

            generation = 1;

            if (timer1.Enabled == true) timer1.Enabled = false;
            else timer1.Enabled = true;

            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            

           

            Life(generation);
            generation++;
            Gen1.Text = Convert.ToString(generation);


        }

        static int X1 (int x, int y, int z) 

        {return (int) (MaxXYZ + 5 * (x -  0.6* z)); }

        static int Y1(int x, int y, int z)

        {return (int) (MaxXYZ - 5 * (y - 0.6 * z));}

        static void CreateDot(int x, int y, int z, Boolean onoff)

        {

            if (onoff) g.FillEllipse(redBrush, X1(x,y,z), Y1(x,y,z), 3, 3);

            else g.FillEllipse(neutralBrush, X1(x, y, z), Y1(x, y, z), 3, 3);


        }

        static void Life(int gen)

        {
            int quant; // количество в окрестности Мура
            Boolean whoThere; // статус текущей ячейки
            

            for (int z = 0; z < max; z++)  // первый обход массива, оценка ситуации, перекладка значений
            {
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)
                    {                  
                        whoThere = Matrix[x, y, z].currentStatus;

                        quant = NeighborQuantity(x, y, z);                                                  

                        if ((quant < qBorn - 1) || (quant >= qDead)) whoThere = false; // одиночество или перенаселение с учетом в количестве самой центральной ячейки

                        if ((quant >= qBorn) && (quant < qDead)) whoThere = true; // если количество больше , то размножение

                        // если не подпало ни под одно из условий - новый статус приравнивается к старому, whoThere не меняется
                        
                        Matrix[x, y, z].nextStatus = whoThere; // в следующий проход новоназначенное состояние ячейки станет текущим и должно быть отображено
                    }
                }
            }

            for (int z = 0; z < max; z++)  // второй обход массива
            {
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)
                    {

                       

                        whoThere = Matrix[x, y, z].nextStatus;

                        CreateDot(x, y, z, whoThere);

                        Matrix[x, y, z].currentStatus = whoThere; // новый статус закрепляется как действующий
                    }
                }

            }

        }
                                         

        static int NeighborQuantity(int x, int y, int z)

        {
            int quantity = 0;

            if (Matrix[x, y, z].currentStatus) quantity = -1; // саму себя не считаем соседом

            for (int zz = z - 1; zz < z + 2; zz++)
            {
                for (int yy = y - 1; yy < y + 2; yy++)
                {
                    for (int xx = x - 1; xx < x + 2; xx++)
                    {

                        if ((xx >= 0) && (xx < max) && (yy >= 0) && (yy < max) && (zz >= 0) && (zz < max)) // координаты в диапазоне
                        {
                            if (Matrix[xx, yy, zz].currentStatus) quantity++;
                        }     // ячейка заполнена

                        //  if (Matrix[xx, yy, zz].currentStatus) quantity++; // ячейка заполнена
                    }
                }
            }

            return quantity;
        }
    }
}
