using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neural.app
{
    public partial class AdditionForm : Form
    {
        private Point StartPoint;
        private NeuralNetwork NN;
        private int[,] arr;
        public AdditionForm(NeuralNetwork NN)
        {
            InitializeComponent();
            this.NN = NN;
            pictureBox1.Image = (Image)new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point EndPoint = new Point(e.X, e.Y);
                Bitmap image = (Bitmap)pictureBox1.Image;
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawLine(new Pen(Color.Black, 4), StartPoint, EndPoint);
                }
                pictureBox1.Image = image;
                StartPoint = EndPoint;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            StartPoint = new Point(e.X, e.Y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[,] getArr = ImageTools.CutBitmapAndGetArrayPaint((Bitmap)pictureBox1.Image, new Point(pictureBox1.Width, pictureBox1.Height));
            if (getArr == null) return;
            arr = ImageTools.Standardizing(getArr, new int[20, 20]);
            if (textBox1.Text != null)
            {
                string get = textBox1.Text;
                NN.SetTraining(get, arr);
                NN.Save();
                Close();
            }
            else MessageBox.Show("Пустое текстовое поле."); 
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
