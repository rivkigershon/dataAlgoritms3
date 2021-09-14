using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson3
{
    class Vertice
    {
        public bool IsVisited { get; set; }
        public int Name { get; set; }
        public List<int> Adjacency { get; set; }
        public int Dist { get; set; }
        public Vertice Prev { get; set; }
        public int Pre { get; set; }
        public int Post { get; set; }
        public Vertice(int name, bool visited)
        {
            Name = name;
            IsVisited = visited;
            Adjacency = new List<int>();
        }
        public void addAdjacency(int adj)
        {
            Adjacency.Add(adj);
        }
    }
    class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public Edge(int from, int to)
        {
            From = from;
            To = to;
        }
    }

    class Graph
    {
        public Dictionary<int, Vertice> d { get; set; }
        public int ccNum { get; set; }
        public Graph(Edge[] edgesArr)
        {
            for (int i = 0; i < edgesArr.Length; i++)
            {
                if (!d.ContainsKey(edgesArr[i].From))
                    d.Add(edgesArr[i].From, new Vertice(edgesArr[i].From, false));
                if (!d.ContainsKey(edgesArr[i].To))
                    d.Add(edgesArr[i].To, new Vertice(edgesArr[i].From, false));
                d[edgesArr[i].From].addAdjacency(edgesArr[i].From);
            }
        }

        public void Explore(Vertice v)
        {
            v.IsVisited = true;
            v.Pre = ccNum++;
            foreach (var adj in v.Adjacency)
            {
                if (!d[adj].IsVisited)
                   Explore(d[adj]);
            }
            v.Post = ccNum++;
        }

        public int DFS()
        {
            int cc = 0;
            foreach (var item in d.Values)
            {
                if (!item.IsVisited)
                {
                    Explore(item);
                    cc++;
                }
            }
            return cc;
        }

        public void BFS(Vertice src)
        {
            foreach (var value in d.Values)
            {
                value.Dist = int.MaxValue;
                value.Prev = null;
            }
            src.Dist = 0;
            Queue<Vertice> q = new Queue<Vertice>();
            Vertice u;
            while (q.Count > 0)
            {
                u = q.Dequeue();
                foreach (var adj in u.Adjacency)
                {
                    if(d[adj].Dist == int.MaxValue)
                    {
                        q.Enqueue(d[adj]);
                        d[adj].Dist = u.Dist + 1;
                        d[adj].Prev = u;
                    }                   
                }
            }
        }

        public void TopologicalSort()
        {
            DFS();
            mergeSort();
        }

        public Vertice[] mergeSort()
        {
            return mergeSort(d.Values.ToArray(), 0, d.Count);
        }
        private Vertice[] mergeSort(Vertice[] a, int low, int high)
        {
            int n = high - low;
            if (n <= 1) return a;
            int mid = (low + high) / 2;
            mergeSort(a, low, mid);
            mergeSort(a, mid, high);
            int i = low, j = mid, k = 0;
            Vertice[] temp = new Vertice[n];
            while (i < mid && j < high)
            {
                if (a[i].Post < a[j].Post) temp[k++] = a[i++];
                else temp[k++] = a[j++];
            }
            while (i < mid) temp[k++] = a[i++];
            while (j < high) temp[k++] = a[j++];
            for (int l = 0; l < n; l++)
            {
                a[low + l] = temp[l];
            }
            return temp;
        }
    }
}

