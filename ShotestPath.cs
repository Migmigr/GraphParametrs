using System;
using System.Collections.Generic;
using System.IO;

namespace Посредничество
{
    class Program
    {
        public static file cur;
        public static List<List<int>> Con;
        public static int N;
        public static float[] Cb;
        static void Main(string[] args)
        {
            Start(args[0], args[1], args[2]);
        }
        public static file rus = new file() { N = 14773, NAME = "RUG.txt" };
        public static file eng = new file() { N = 742212, NAME = "ENG.txt" };
        public static void Start(string o, string de, string len)
        {
            if (len == "ru")
            {
                cur = rus;
            }
            else
            {
                cur = eng;
            }

            N = cur.N;
            Con = new List<List<int>>();
            for (int i = 0; i < N; i++)
            {
                Con.Add(new List<int>());
            }

            int ot = Convert.ToInt32(o);
            int dod = Convert.ToInt32(de);
            dod = Math.Min(dod, N);
            using (StreamReader SR = new StreamReader(cur.NAME))
            {
                string line = "";
                while ((line = SR.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    Con[Convert.ToInt32(words[0])].Add(Convert.ToInt32(words[1]));
                }
            }

            float[] d = new float[N];
            bool[] used = new bool[N];
            List<float> res = new List<float>();

            for (int s = ot; s < dod; s++)
            {
                Console.WriteLine("(" + ot + "-" + dod + "):" + s);
                used = new bool[N];
                for (int i = 0; i < N; i++)
                {
                    d[i] = float.PositiveInfinity;
                    used[i] = false;
                }
                d[s] = 0;

                for (int i = 0; i < N; i++)
                {
                    int v = -1;
                    for (int j = 0; j < N; j++)
                    {
                        if ((used[j] == false) && (v == -1 || d[j] < d[v]))
                        {
                            v = j;
                        }
                    }
                    if (d[v] == float.PositiveInfinity)
                    {
                        break;
                    }
                    used[v] = true;

                    foreach (int u in Con[v])
                    {
                        if (d[u] > d[v] + 1)
                        {
                            d[u] = d[v] + 1;
                        }
                    }
                }

                foreach (float p in d)
                {
                    if (!float.IsPositiveInfinity(p))
                    {
                        res.Add(p);
                    }
                }
            }

            using (StreamWriter SW = new StreamWriter("Path" + cur.NAME + "(" + ot + "-" + dod + ").txt"))
            {
                foreach (float s in res)
                {
                    SW.WriteLine(s);
                }
            }
        }
        public struct file
        {
            public int N { get; set; }
            public string NAME { get; set; }
        }

    }
}
