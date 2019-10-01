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

        public static int x = 0;
        //public static int y = 0;
        //public static int z = 0;

        public static int MaxXYZ = 250;


        public Form1()
        {
            InitializeComponent();
            redBrush = new SolidBrush(Color.Red);
            neutralBrush = new SolidBrush(this.BackColor);
            axesBrush = new Pen(Color.Black, 3);

            g = this.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            g.DrawLine(axesBrush, 250, 250, 500, 250); // ось Х
            g.DrawLine(axesBrush, 250, 250, 250, 1); // ось Y
            g.DrawLine(axesBrush, 250, 250, 215, 250); // ось Z    



            if (timer1.Enabled == true) timer1.Enabled = false;
            else timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MatrixTest();
            

        }

        static int X1 (int x, int y, int z) 

        {return (int) (MaxXYZ + 5 * (x - 0.7 * z)); }

        static int Y1(int x, int y, int z)

        {return (int) (MaxXYZ - 5 * (y - 0.7 * z));}

        static void CreateDot(int x, int y, int z, Boolean onoff)

        {

            if (onoff) g.FillEllipse(redBrush, X1(x,y,z), Y1(x,y,z), 5, 5);

            else g.FillEllipse(neutralBrush, X1(x, y, z), Y1(x, y, z), 5, 5);


        }

        static void MatrixTest()

        {
            x++;
            int y = (int)(Math.Cos(Math.PI / 180 * x) * 200);
            int z = (int)(Math.Sin(Math.PI / 180 * x) * 200);
            



            if ((x > 250) || (y > 250) || (z > 250)) return;

            CreateDot(x, y, z, true);

            
            




        }




    }



}
