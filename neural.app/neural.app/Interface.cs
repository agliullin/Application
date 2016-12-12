using AForge.Video.FFMPEG;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace neural.app
{
    public partial class Interface : Form
    {
        private NeuralNetwork NN;
        private int[,] arr0;
        private int[,] arr1;
        private int[,] arr2;
        private int[,] arr3;
        Bitmap img0, img1, img2, img3;

        private void button5_Click(object sender, EventArgs e)
        {
            Form resultForm = new AdditionForm(NN);
            resultForm.Show();
        }

        public Interface()
        {
            InitializeComponent();
            NN = new NeuralNetwork();
        }
        

        private void recognize()
        {
            string get = "";
            int[,] image0 = ImageTools.CutBitmapAndGetArray(img0, new Point(img0.Width, img0.Height));
            arr0 = ImageTools.Standardizing(image0, new int[NeuralNetwork.ArrayWidth, NeuralNetwork.ArrayHeight]);
            int[,] image1 = ImageTools.CutBitmapAndGetArray(img1, new Point(img1.Width, img1.Height));
            arr1 = ImageTools.Standardizing(image1, new int[NeuralNetwork.ArrayWidth, NeuralNetwork.ArrayHeight]);
            int[,] image2 = ImageTools.CutBitmapAndGetArray(img2, new Point(img2.Width, img2.Height));
            arr2 = ImageTools.Standardizing(image2, new int[NeuralNetwork.ArrayWidth, NeuralNetwork.ArrayHeight]);
            int[,] image3 = ImageTools.CutBitmapAndGetArray(img3, new Point(img3.Width, img3.Height));
            arr3 = ImageTools.Standardizing(image3, new int[NeuralNetwork.ArrayWidth, NeuralNetwork.ArrayHeight]);
            get += NN.Recognition(arr0);
            get += NN.Recognition(arr1);
            get += NN.Recognition(arr2);
            get += NN.Recognition(arr3);
            textBox1.Text = get;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            recognize();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            VideoFileReader reader = new VideoFileReader();
            OpenFileDialog open_dialog = new OpenFileDialog(); // Создание диалогового окна для выбора файла
            open_dialog.Filter = "Video Files(*.WMV;*.AVI)|*.WMV;*.AVI|All files (*.*)|*.*"; // Формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) // Если в окне была нажата кнопка "ОК"
            {
                try
                {
                    reader.Open(open_dialog.FileName);
                    int index = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        Bitmap videoFrame = reader.ReadVideoFrame();
                        if (i % 25 == 0)
                        {
                            videoFrame.Save(index + ".bmp");
                            index++;
                        }

                        videoFrame.Dispose();
                    }
                    reader.Close();
                    DialogResult res = MessageBox.Show("Видео успешно обработано.", "", MessageBoxButtons.OK);

                }
                catch
                {
                    DialogResult res = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = (Image)new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox3.Image = (Image)new Bitmap(pictureBox3.Width, pictureBox3.Height);
            pictureBox4.Image = (Image)new Bitmap(pictureBox4.Width, pictureBox4.Height);

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            img0 = new Bitmap("0.bmp");
            pictureBox1.Image = img0;

            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            img1 = new Bitmap("1.bmp");
            pictureBox2.Image = img1;

            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            img2 = new Bitmap("2.bmp");
            pictureBox3.Image = img2;

            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            img3 = new Bitmap("3.bmp");
            pictureBox4.Image = img3;
        }
    }
}
