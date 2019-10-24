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

        public static int MaxXYZ = 250;

        public static int max = 30;

        public static int qBorn = 3;

        public static int qDead = 5;

        public static int procent = 97; // процент свободного места в стартовом заполнении

         public struct Cell

        { 
            public bool currentStatus;

            public bool newStatus;

        }


        public static Cell [,,] Matrix = new Cell [max,max,max]; // трехмерный массив структур Cell размерностями max

        public Random rnd = new Random();

       

        public Form1() // конструктор класса Form1
        {
            InitializeComponent();  // оставляем
            redBrush = new SolidBrush(Color.Blue);
            neutralBrush = new SolidBrush(BackColor);
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

                            Matrix[x, y, z].currentStatus = false;

                            if (r > procent)
                             {
                                 Matrix[x, y, z].newStatus = true;
                                 CreateDot(x, y, z, true);
                             }
                                                         
                         }
                     }
                 }
             }
             


            generation = 1;

            if (timer1.Enabled == true) timer1.Enabled = false;
            else  
            timer1.Enabled = true;

            
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

        static void CreateDot(int x, int y, int z, bool onoff)

        {

            if (onoff) g.FillEllipse(redBrush, X1(x,y,z), Y1(x,y,z), 4, 4);

            else g.FillEllipse(neutralBrush, X1(x, y, z), Y1(x, y, z), 4, 4);                      
        
        }

        static void Life(int gen)

        {
            int quant; // количество в окрестности Мура
            
            bool changeFlag; // признак изменения статуса ячейки 


            for (int z = 0; z < max; z++)  
            {
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)

                    {

                        if (Matrix[x, y, z].currentStatus != Matrix[x, y, z].newStatus) changeFlag = true; // статус менялся

                        else changeFlag = false;

                        Matrix[x, y, z].currentStatus = Matrix[x, y, z].newStatus;
                        
                        if (changeFlag) CreateDot(x, y, z, Matrix[x, y, z].currentStatus); // если статус менялся - прорисовываем/затираем точку

                        // вычисление и присваивание статуса на следующий проход

                        quant = NeighborQuantity(x, y, z);

                        if ((quant == qBorn) && (Matrix[x, y, z].currentStatus == false)) Matrix[x, y, z].newStatus = true; // если количество начальное для рождения, и поле пустое, то размножение
                        
                        if ((quant < qBorn - 1) || (quant >= qDead)) Matrix[x, y, z].newStatus = false; // одиночество или перенаселение с учетом в количестве самой центральной ячейки

                        
                        // если не подпало ни под одно из условий - статус  не меняется

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

                        
                    }
                }
            }

            return quantity;
        }

       
    }
}
