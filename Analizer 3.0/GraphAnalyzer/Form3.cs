using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace GraphAnalyzer
{
    public partial class Form3 : Form
    {
        Bitmap bm;
        Graphics g;
        double[][] MAT;
        int N;
        string path;
        public Form3(string path)
        {
            InitializeComponent();
            this.path = path;
            this.Text = "Picture for " + path;
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            pre(pictureBox1.Width / 2, pictureBox1.Height / 2);
            g.ScaleTransform(1, -1);
            pictureBox1.Image = bm;
        }
        public void pre(int x, int y)
        {
            g.Clear(Color.White);
            g.TranslateTransform(x, y);
            g.DrawLine(new Pen(Color.Red), -1000, 0, 1000, 0);
            g.DrawLine(new Pen(Color.Blue), 0, -1000, 0, 1000);
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            MAT = FromFile(path);
            int R = 200;
            g.DrawEllipse(new Pen(Color.OldLace), -R, -R, 2 * R, 2 * R);
            double p = 2 * Math.PI / MAT.Length;
            int[][] Kt = new int[MAT.Length][];
            for (int i = 0; i < MAT.Length; i++)
            {
                Kt[i] = new int[2];
                Kt[i][0] = Convert.ToInt32(R * Math.Cos(i * p));
                Kt[i][1] = Convert.ToInt32(R * Math.Sin(i * p));
                Pen f = new Pen(Color.Orange, 5);
                g.DrawRectangle(f, Kt[i][0] - 2, Kt[i][1] - 2, 4, 4);
            }
            for (int i = 0; i < MAT.Length; i++)
            {
                for (int j = 0; j < MAT.Length; j++)
                {
                    if (MAT[i][j] == 1)
                    {
                        if (i != j)
                        {
                            g.DrawLine(new Pen(Color.Black), Kt[i][0], Kt[i][1], Kt[j][0], Kt[j][1]);
                        }
                        else
                        {
                            int d = 10;
                            if (Kt[i][0] >= 0 && Kt[i][1] >= 0)
                            {
                                g.DrawEllipse(new Pen(Color.Black), Kt[i][0] - 2, Kt[i][1] - 2, 2 * 20, 2 * 20);
                            }
                            else
                            {
                                if (Kt[i][0] <= 0 && Kt[i][1] >= 0)
                                {
                                    g.DrawEllipse(new Pen(Color.Black), Kt[i][0] - d * 4 + 2, Kt[i][1] - 2, 2 * 20, 2 * 20);
                                }
                                else
                                {
                                    if (Kt[i][0] <= 0 && Kt[i][1] <= 0)
                                    {
                                        g.DrawEllipse(new Pen(Color.Black), Kt[i][0] - d * 4 + 2, Kt[i][1] - d * 4 + 2, 2 * 20, 2 * 20);
                                    }
                                    else
                                    {
                                        if (Kt[i][0] >= 0 && Kt[i][1] <= 0)
                                        {
                                            g.DrawEllipse(new Pen(Color.Black), Kt[i][0] - 2, Kt[i][1] - d * 4 + 2, 2 * 20, 2 * 20);
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
            }
        }
        
        double[][] FromFile(string path)
        {
            StreamReader myStream = null;
            if ((myStream = new StreamReader(path)) != null)
            {
                using (myStream)
                {
                    int c = 0;
                    N = 0;
                    String line;
                    while (((line = myStream.ReadLine()) != null))
                    {
                        string[] word = line.Split(' ');
                        if (c == 0)
                        {
                            N = word.Length;
                            MAT = new double[N][];
                        }
                        MAT[c] = new double[N];
                        for (int i = 0; i < N; i++)
                        {
                            MAT[c][i] = Convert.ToDouble(word[i]);
                        }
                        c++;
                    }
                }
            }
            return MAT;
        }

    }
}
