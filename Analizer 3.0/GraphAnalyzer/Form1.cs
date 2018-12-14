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
using System.Collections;

namespace GraphAnalyzer
{
    public partial class Form1 : Form
    {
        double[][] MAT;
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.InitialDirectory = Application.ExecutablePath;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.Multiselect = true;
            openFileDialog1.RestoreDirectory = true;
        }
        #region comparer
        public class ReverseComparer : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(Object x, Object y)
            {
                return (new CaseInsensitiveComparer()).Compare(((int[])y)[1], ((int[])x)[1]);
            }
        }
        public class ReverseComparerP : IComparer
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(Object x, Object y)
            {
                return (new CaseInsensitiveComparer()).Compare(y, x);
            }
        }
        #endregion
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            int c = dataGridView3.RowCount;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dataGridView3.RowHeadersWidth = 50;

                foreach (String file in openFileDialog1.FileNames)
                {
                    dataGridView3.Rows.Add(file);
                    dataGridView3.Rows[dataGridView3.RowCount - 1].HeaderCell.Value = c.ToString();

                    MAT = FromFile(file);
                    Graph gr = new Graph(MAT, c);
                    c++;
                }
            }
        }

        List<int> genDist(double beta, int N, int E, double xmin, double xmax)
        {
            double summ;
            double[] P;
            List<int> W;
            Random rn1 = new Random();
            do
            {
                P = new double[N];
                W = new List<int>();
                summ = 0;

                for (int i = 0; i < N; i++)
                {
                    P[i] = Math.Pow(((Math.Pow(xmax, 1 - beta) - Math.Pow(xmin, 1 - beta)) * rn1.NextDouble()) + Math.Pow(xmin, 1 - beta), (1 / 1 - beta));
                    summ = summ + P[i];
                }
                double c = 2 * E / summ;
                summ = 0;

                for (int i = 0; i < N; i++)
                {
                    W.Add((int)(Math.Round(P[i] * c)));
                    summ = summ + W[i];
                }
            } while (summ != 2 * E);
            return W;
        }
        double[][] FromFile(string path)
        {
            StreamReader myStream = null;
            if ((myStream = new StreamReader(path)) != null)
            {
                using (myStream)
                {
                    int c = 0;
                    int N = 0;
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
        #region pokaz
        private void displayGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 a = new Form3(dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString());
            a.Show();
        }

        private void incedenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 a = new Form2(dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString());
            a.Show();
        }

        private void minPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 a = new Form2(dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString(), true);
            a.Show();
        }
        #endregion

        private string[] ER(int N, int E)
        {
            double[] res = new double[9];
            string[] f;
            for (int ka = 0; ka < 100; ka++)
            {

                Random rn1 = new Random();
                Random rn2 = new Random();
                int a, b;
                //

                double[][] MIe = new double[N][];
                for (int i = 0; i < N; i++)
                {
                    MIe[i] = new double[N];
                }

                for (int i = 0; i < E; i++)
                {
                    do
                    {
                        a = rn1.Next(0, N);
                        b = rn1.Next(0, N);
                    }
                    while ((a == b) || (MIe[a][b] == 1));

                    MIe[a][b] = 1;
                    MIe[b][a] = 1;
                }
            }
            f = new string[9];

            return f;
        }
        private string[] CL(double beta, int N, int E, double xmin, double xmax)
        {
            double[] res = new double[9];
            string[] f;
            for (int ka = 0; ka < 100; ka++)
            {
                List<int> W;
                int a, b;
                Random rn1 = new Random();
                double[][] MIs;
                W = genDist(beta, N, E, xmin, xmax);
                int popitki = 0;
                int p = 1;
                do
                {
                    popitki++;
                    if (popitki >= 50000)
                    {
                        W = genDist(beta, N, E, xmin, xmax);
                    }
                    MIs = new double[N][];
                    for (int i = 0; i < N; i++)
                    {
                        MIs[i] = new double[N];
                    }
                    //

                    List<int> NN = new List<int>();
                    List<int> W1 = new List<int>();
                    for (int i = 0; i < N; i++)
                    {
                        W1.Add(W[i]);
                        NN.Add(i);
                    }

                    int an, bn;
                    int ch = 0;
                    for (int i = 0; i < E; i++)
                    {
                        do
                        {
                            an = 0;
                            bn = rn1.Next(0, NN.Count);

                            a = NN[an];
                            b = NN[bn];

                            p = 1;
                            if (MIs[a][b] == 1)
                            {
                                ch++;

                            }
                            else
                            {
                                if (a == b)
                                {
                                    ch++;
                                }
                                else
                                {
                                    ch = 0;
                                }

                            }
                            if (ch >= 4)
                            {
                                p = 0;
                                i = E;
                                break;
                            }
                        }
                        while ((a == b) || (W[a] == 0) || (W[b] == 0) || (MIs[a][b] == 1));

                        MIs[a][b] = 1;
                        MIs[b][a] = 1;

                        if (p == 1)
                        {
                            W[a]--;
                            W[b]--;
                        }

                        if (a > b)
                        {

                            if (W[a] == 0)
                            {
                                NN.RemoveAt(an);
                            }
                            if (W[b] == 0)
                            {
                                NN.RemoveAt(bn);
                            }
                        }
                        else
                        {
                            if (W[b] == 0)
                            {
                                NN.RemoveAt(bn);
                            }
                            if (W[a] == 0)
                            {
                                NN.RemoveAt(an);
                            }
                        }
                    }
                    if (p == 0)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            W[j] = W1[j];
                        }
                    }
                }
                while (p == 0);
            }
            f = new string[9];

            return f;
        }
    }
}
