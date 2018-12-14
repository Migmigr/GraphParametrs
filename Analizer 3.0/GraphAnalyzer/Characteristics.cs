using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
namespace GraphAnalyzer
{
    public class Graph
    {
        int c;
        bool or;
        public double[][] MAT;
        public int N, E;
        public double D;//density
        public double[] gi;
        double[][] L;//matrix min path
        double[][] VL;//matrix count of vertex min path
        int[] ki, kin, kout; //degree of nodes
        double[] si; // strength
        double[] knni,knnwi;
        double[] Y2;//Y2  For example, the most heterogeneous situation is obtained when one weight dominates over all the others.
        double[] f;
        double[] Ci; //clust. koef
        double[] Cwi;//w. clust. koef
        double Dg;// diametr
        double radg;// radius
        double[] Cb;   //Betweenness centrality
        double[] ec;//eccentricity
        int [][] X;
        int[] ksi;
        Double[] rcc;
        double r; //Assortativity
        public Graph(double[][] M,int ou)
        {
            c = ou;
            or = false;
            MAT = M;
            N = MAT.Length;
            X = new int[N][];
            ksi=new int[N];
            findX();
            L = new Double[N][];
            VL = new Double[N][];
            ki = new int[N];
            kin = new int[N];
            kout = new int[N];
            si = new Double[N];
            Y2 = new Double[N];
            gi = new Double[N];
            ec = new Double[N];
            f = new Double[N];
            knni = new Double[N];
            knnwi = new Double[N];
            rcc = new Double[N];
            find_E_ki_si();
           
            if(or==true)
            {
                D = (double)(E) / (N * (N - 1));
            }
            else
            {
                D = (double)(2 * E) / (N * (N - 1));
            }
         

            //findMinMat();         
            Ci = new double[N];
            Cwi = new double[N];

            findMinMat_clustering_koef();
            findY2_gi_Dg_ec();
          
            Cb = new double[N];
            findBetweenness();
            findAssortativity();
            findrichclubcoef();
            Otchet();
        }
        public void Otchet()
        {
            StreamWriter myStream = null;
            if ((myStream = new StreamWriter(Application.StartupPath+"\\Отчеты\\"+c+".csv")) != null)
            {
                using (myStream)
                {
                    myStream.WriteLine("N;E;Density;Diametr;Radius;Assortativity");
                    myStream.WriteLine(N + ";" + E + ";" + D + ";" + Dg + ";" + radg + ";" + r);

                }
            }
        }
        public void  findrichclubcoef()
        {
            int nodcount = 0;
            for(int k=0;k<N;k++)
            {
                List<int> ri = new List<int>();
                for(int i=0;i<N;i++)
                {
                    if(ki[i]<=k)
                    {
                        ri.Add(i);
                    }
                }
                int kol = 0;
                foreach(int i in ri)
                {
                    foreach (int j in ri)
                    {
                       if(MAT[j][i]!=0)
                       {
                           kol++;
                       }
                    }
                }
               if(or==false)
               {
                   kol = kol / 2;
               }
                if(kol!=0)
                {
                    rcc[k] = kol / ri.Count;
                }
             
            }
        }
        public void findX()
        {
            for(int i=0;i<N;i++)
            {
                X[i] = new int[N];
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (MAT[i][j] != 0)
                    {
                        X[i][j] = 1;
                        if(MAT[j][i]==0)
                        {
                            or = true;
                        }
                       
                    }
                }
            }  
        }       
        public void findY2_gi_Dg_ec()
        {
            double summ = 0, sum = 0; 
            for (int i = 0; i < N; i++)
            {
                summ = 0; sum = 0;
                for (int j = 0; j < N; j++)
                {
                    if(i!=j)
                    {
                        ec[i] = Math.Max(ec[i], VL[i][j]);
                        summ = summ + VL[i][j];
                    }
                    if (MAT[i][j]!=0)
                    {
                        sum = sum + ((MAT[i][j] / si[i]) * Math.Log(MAT[i][j] / si[i]));
                        Y2[i] = Y2[i] + (MAT[i][j] / si[i]) * (MAT[i][j] / si[i]);
                    }
                  
                    Dg = Math.Max(Dg, VL[i][j]);  
                  
                }
                gi[i] = 1 / summ;
                f[i] = -sum/Math.Log(ki[i]);
            }
            radg = ec[0];

            for (int i = 0; i < N; i++)
            {
                radg = Math.Min(radg, ec[i]);
            }
        }
        public void find_E_ki_si()
        {
            E = 0;         
            for (int i = 0; i < N; i++)
            {
                if (MAT[i][i] == 1)
                {
                    E++;
                }
                for (int j = 0; j < N; j++)
                {
                    if (MAT[i][j] != 0)
                    {
                        si[i] = si[i] + MAT[i][j];
                        kout[i]++;
                        E++;
                    }
                    if (MAT[j][i] != 0)
                    {
                        kin[i]++;
                    }
                   
                }
            }
            for (int i = 0; i < N; i++)
            {
                if(or==true)
                {
                     ki[i]=kin[i]+kout[i];
                }
                else
                {
                    ki[i] = kin[i];
                }

                ksi[i] =Math.Max(kin[i],kout[i]);
            }
            if (or == false)
            {
                E = E / 2;
            }
           
            double sum, summ; 
            for (int i = 0; i < N; i++)
            {
                sum=0;
                summ = 0;
              
                for (int j = 0; j < N; j++)
                {
                    if (MAT[i][j] != 0 )
                    {
                        sum = sum + ki[j];
                        summ = summ + MAT[i][j] * ki[j];
                       
                    }
                    else
                    {
                        if (or == true)
                        {
                            if (MAT[j][i] != 0)
                            {
                                sum = sum + ki[j];
                                summ = summ + MAT[j][i] * ki[j];
                             
                            }

                        }
                    }
                    
                }
                knni[i] = sum / ksi[i];
                knnwi[i] = summ / si[i];
               
            }
        }     
        public void findMinMat_clustering_koef()
        {
            for (int i = 0; i < N; i++)
            {
                L[i] = new double[N];
                VL[i] = new double[N];
                for (int j = 0; j < N; j++)
                {
                    if (MAT[i][j] != 0)
                    {
                        VL[i][j] = 1;
                        L[i][j] = MAT[i][j];
                    }
                    else
                    {
                        L[i][j] = double.PositiveInfinity;
                        VL[i][j] = double.PositiveInfinity;
                    }
                }
                L[i][i] = 0;
                VL[i][i] = 0;
            }

            double sum = 0, sum1 = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (MAT[i][j]!=0 || MAT[j][i]!=0)
                    {

                    }
                    for (int  l= 0; l < N; l++)
                    {

                        if (X[i][j] * X[j][l] * X[l][i] == 1)
                        {

                            if (X[j][i] * X[i][j] * X[i][l] == 0)
                            {
                                Ci[i]++;
                                Cwi[i] = Cwi[i] + (MAT[j][i] + MAT[l][i]);
                            }
                        }

                        Ci[i] = Ci[i] + (X[i][j] * X[j][l] * X[l][i]);
                        Cwi[i] = Cwi[i] + (((MAT[i][j] + MAT[i][l]) / 2) * (X[i][j] * X[j][l] * X[l][i]));
                    
                        if (L[j][i] != 0 && L[i][l] != 0 && j != l)
                        {
                            if (L[j][i] + L[i][l] < L[j][l] || L[j][l] == 0)
                            {
                                L[j][l] = L[j][i] + L[i][l];
                                VL[j][l] = VL[j][i] + VL[i][l];
                            }
                        }

                    }
                }

                if (ksi[i] == 0 || (ksi[i] - 1 == 0))
                {
                    Ci[i] = 0;
                }
                else
                {
                    Ci[i] = Ci[i] / (ksi[i] * (ksi[i] - 1));
                }
                if (si[i] == 0 || (ksi[i] - 1 == 0))
                {
                    Cwi[i] = 0;
                }
                else
                {
                    Cwi[i] = Cwi[i] / (si[i] * (ksi[i] - 1));
                }
                sum1 = sum1 + Cwi[i];
                sum = sum + Ci[i];
            }
           
        }
        public void findBetweenness()
        {
            double[] dze = new double[N];
            double[] d = new double[N];

            List<int>[] P = new List<int>[N];
            Queue<int> Q = new Queue<int>();

            for (int s = 0; s < N; s++)
            {
                Stack<int> S = new Stack<int>();
                for (int t = 0; t < N; t++)
                {
                    d[t] = Double.NegativeInfinity;
                    P[t] = new List<int>();
                    dze[t] = 0;
                }

                d[s] = 0;
                dze[s] = 1;
                Q = new Queue<int>();              
                Q.Enqueue(s);
                while (Q.Count != 0)
                {
                    int v = Q.Dequeue();                                    
                    S.Push(v);                  
                    for (int w = 0; w < N; w++)
                    {
                        if (MAT[w][v] != 0)
                        {                          
                            if (d[w] < 0)
                            {
                                Q.Enqueue(w);
                                d[w] = d[v] + 1;
                            }
                            if (d[w] == d[v] + 1)
                            {
                                dze[w] = dze[w] + dze[v];
                                P[w].Add(v);
                            }
                        }
                    }
                }
                double[] dat = new double[N];
                while (S.Count != 0)
                {
                    int w = S.Pop();
                    foreach (int v in P[w])
                    {                      
                        dat[v] = dat[v] + (dze[v] / (double)dze[w]) * (1 + (double)dat[w]);
                    }
                    if (w != s)
                    {
                        Cb[w] = Cb[w] + dat[w];
                    }
                    if (or == false)
                    {
                        Cb[w] = Cb[w] / 2;
                    }
                }
               
            }
        }
        public void findAssortativity()
        {
            int[] jr = new int[E];
            int[] ir = new int[E];
            int n0 = 0;
            if(or==false)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if ((MAT[i][j] == MAT[j][i]) && MAT[i][j] != 0)
                        {
                            jr[n0] = -1;
                            ir[n0] = -1;
                            for (int k = 0; k < N; k++)
                            {
                                if (MAT[i][k] != 0)
                                {
                                    jr[n0]++;
                                    if (MAT[k][i] == 0)
                                    {
                                        ir[n0]++;
                                    }
                                }
                                if (MAT[k][j] != 0)
                                {
                                    ir[n0]++;
                                    if (MAT[k][j] == 0)
                                    {
                                        jr[n0]++;
                                    }
                                }
                            }
                            n0++;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (MAT[i][j] != 0)
                        {
                            jr[n0] = -1;
                            ir[n0] = -1;
                            for (int k = 0; k < N; k++)
                            {
                                if (MAT[i][k] != 0)
                                {
                                   ir[n0]++;
                                }
                                if (MAT[k][i] != 0)
                                {
                                    ir[n0]++;
                                }
                                if (MAT[k][j] != 0)
                                {
                                    jr[n0]++;
                                }
                                if (MAT[j][k] != 0)
                                {
                                    jr[n0]++;
                                }
                            }
                            n0++;
                        }
                    }
                }
            }
            double s1 = 0, s2 = 0, s3 = 0;
            for (int i = 0; i < E; i++)
            {
                s1 = s1 + jr[i] * ir[i];
                s2 = s2 + jr[i] + ir[i];
                s3 = s3 + (jr[i] * jr[i]) + (ir[i] * ir[i]);
            }
            r = ((s1 / E) - (s2 / (2 * E)) * (s2 / (2 * E))) / ((s3 / (2 * E)) - (s2 / (2 * E)) * (s2 / (2 * E)));          
        }      
    }
     
}
