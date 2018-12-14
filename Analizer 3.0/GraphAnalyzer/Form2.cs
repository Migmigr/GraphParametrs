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
    public partial class Form2 : Form
    {
        double[][] MAT;
        int N;
        string path;
        bool minpath = false;
        public Form2(string path)
        {
            this.path = path;
            InitializeComponent();
            this.Text ="Matrix for "+ path;
        }
        public Form2(string path, bool s)
        {
            minpath = s;
            this.path = path;
            InitializeComponent();
            this.Text = "Matrix of min path for " + path;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            MAT = FromFile(path);
            for (int i = 0; i < N; i++)
            {
                dataGridView1.Columns.Add((i + 1).ToString(), (i + 1).ToString());
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].HeaderCell.Value = (i + 1).ToString();
                dataGridView1.RowHeadersWidth = 50;
            }
            if(minpath==false)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = MAT[i][j];
                    }
                }
            }
            else
            {
                double[][] L = new double[N][];
                for (int i = 0; i < N; i++)
                {
                    L[i] = new double[N];

                    for (int j = 0; j < N; j++)
                    {
                        
                        L[i][j] = MAT[i][j];
                    }
                    L[i][i] = 0;
                }
                for (int k = 0; k < N; k++)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (L[i][k] != 0 && L[k][j] != 0 && i != j)
                            {
                                if (L[i][k] + L[k][j] < L[i][j] || L[i][j] == 0)
                                {
                                    L[i][j] = L[i][k] + L[k][j];
                                }
                            }

                        }
                    }
                   
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = L[i][j];
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
