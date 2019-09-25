using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IdentifyByTesseract
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string imgPath = "";

        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imgPath = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(imgPath);
                richTextBox1.Text = "";
                Thread thread = new Thread(ImageToText);
                thread.Start();
            }
        }

        public void ImageToText()
        {
            for (int i = 0; i < 13; i++)
            {
                this.Invoke(new Action(() =>
                {
                    richTextBox1.Text += "PSM" + i + ":";
                    richTextBox1.Text += Environment.NewLine;
                }));
                string result = ExcuteImageToText(i, imgPath);
                this.Invoke(new Action(() =>
                {
                    richTextBox1.Text += @result;
                    richTextBox1.Text += "---------------------";
                    richTextBox1.Text += Environment.NewLine;
                }));
            }
        }

        public string ExcuteImageToText(int mode, string imgPath)
        {
            string filePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\out.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string cmd = "";
            cmd += "tesseract ";
            cmd += imgPath + " ";
            cmd += System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\out "; ;
            cmd += "-l chi_sim ";
            cmd += "--psm ";
            cmd += mode;
            //cmd = @"tesseract C:\Users\ZR644\Desktop\tesseract-ocr\myscan.png C:\Users\ZR644\Desktop\tesseract-ocr\out -l chi_sim --psm 7";
            var result = CmdHelper.Cmd(new string[1] { cmd });
            return CmdHelper.ReadTextFile(filePath);
        }
    }

}
