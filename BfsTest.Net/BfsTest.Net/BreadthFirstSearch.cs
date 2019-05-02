using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BfsTest.Net
{
    public class BreadthFirstSearch
    {
        Queue<int> queue = new Queue<int>();
        ParallelOptions options = new ParallelOptions();
        private bool[] myVisited = null;
        private ConcurrentDictionary<int, bool> myVisitedDict = null;

        public BreadthFirstSearch()
        {
            //The hyperthread technology by intel may influence the efficiency of parallel performance
            options.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
            myVisitedDict = new ConcurrentDictionary<int, bool>();
        }
        public void NormalBFS(Graph g, int s)
        {
            bool[] visited = new bool[g.GetVerticals() + 1];
            queue.Clear();
            visited[s] = true;
            //Console.WriteLine(s);
            queue.Enqueue(s);
            while (queue.Count() != 0)
            {
                int v = queue.Dequeue();
                foreach (int w in g.GetAdjacency(v))
                {
                    if (!visited[w])
                    {
                        visited[w] = true;
                        //Thread.Sleep(1);
                        //Console.WriteLine(w);
                        queue.Enqueue(w);
                    }
                }
            }
        }

        public void ParallelBFS1(Graph g, int s, bool reverse = false)
        {
            bool[] visited = new bool[g.GetVerticals() + 1];
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

            visited[s] = true;
            //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId + " ,Start Node:" + s);
            queue.Enqueue(s);
            while (queue.Count() != 0)
            {
                if (queue.TryDequeue(out int v))
                {
                    List<int> nodes = g.GetAdjacency(v);

                    Parallel.For(0, nodes.Count, i =>
                    {
                        int node = nodes[i];

                        if (!visited[node])
                        {
                            bool visit = false;
                            lock (visited)
                            {
                                if (!visited[node])
                                {
                                    visit = true;
                                    visited[node] = true;
                                    //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " , visit " + node + ", " + v + "-->" + node);
                                }
                            }
                            if (visit)
                            {
                                queue.Enqueue(node);
                                //Thread.Sleep(1);
                            }
                        }
                    });
                }
            }
        }

        public void ParallelBFS2(Graph g, int s, bool reverse = false)
        {
            myVisited = new bool[g.GetVerticals() + 1];
            //ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            myVisited[s] = true;
            //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " ,start node:" + s);
            //queue.Enqueue(s);

            List<int> firstNodeList = new List<int>();
            firstNodeList.Add(s);

            do
            {
                firstNodeList = Find(g, firstNodeList);
            } while (firstNodeList.Count > 0);
        }

        public void ParallelBFS3(Graph g, int s, bool reverse = false)
        {
            myVisitedDict.Clear();
            myVisitedDict.TryAdd(s, true);

            //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " ,start node:" + s);


            List<int> firstNodeList = new List<int>();
            firstNodeList.Add(s);

            do
            {
                firstNodeList = Find2(g, firstNodeList);
            } while (firstNodeList.Count > 0);
        }

        public void ParallelBFS4(Graph g, int s, bool reverse = false)
        {
            myVisited = new bool[g.GetVerticals() + 1];
            //ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            myVisited[s] = true;
            //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " ,start node:" + s);
            //queue.Enqueue(s);

            List<int> firstNodeList = new List<int>();
            firstNodeList.Add(s);

            do
            {
                firstNodeList = Find3(g, firstNodeList);
            } while (firstNodeList.Count > 0);
        }

        //Double Layer Parallel
        private List<int> Find(Graph g, List<int> vList)
        {
            //List<int> nextNodeList = new List<int>();
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

            Parallel.For(0, vList.Count, index =>
            {
                List<int> nodes = g.GetAdjacency(vList[index]);

                //Since we are using sparse graph to test the performance, 
                //We set the maximum of threads to real core amount to get the best results
                //Switching from too many threads while each thread get low load work will be expensive
                Parallel.For(0, nodes.Count, options, i =>
                 {
                     int node = nodes[i];

                     if (!myVisited[node])
                     {
                         bool visit = false;
                         lock (myVisited)
                         {
                             if (!myVisited[node])
                             {
                                 visit = true;
                                 myVisited[node] = true;
                                 //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " , visit " + node + ", " + vList[index] + "-->" + node);
                             }
                         }
                         if (visit)
                         {
                             //lock (nextNodeList)
                             // {
                             // nextNodeList.Add(node);
                             queue.Enqueue(node);
                             // }
                             //Thread.Sleep(1);
                         }
                     }
                 });
            });

            return queue.ToList();
        }

        //lock-free
        private List<int> Find2(Graph g, List<int> vList)
        {
            //List<int> nextNodeList = new List<int>();
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

            Parallel.For(0, vList.Count, index =>
            {
                List<int> nodes = g.GetAdjacency(vList[index]);

                Parallel.For(0, nodes.Count, i =>
                {
                    int node = nodes[i];

                    if (myVisitedDict.TryAdd(node, true))
                    {
                        queue.Enqueue(node);
                        //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " , visit " + node + ", " + vList[index] + "-->" + node);
                    }
                });
            });

            return queue.ToList();
        }

        //Only Use Parallel in Looping through Queue
        private List<int> Find3(Graph g, List<int> vList)
        {
            //List<int> nextNodeList = new List<int>();
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

            Parallel.For(0, vList.Count, index =>
            {
                List<int> nodes = g.GetAdjacency(vList[index]);         

                foreach (int node in nodes)
                {
                    if (!myVisited[node])
                    {
                        bool visit = false;
                        lock (myVisited)
                        {
                            if (!myVisited[node])
                            {
                                visit = true;
                                myVisited[node] = true;
                                //Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " , visit " + node + ", " + vList[index] + "-->" + node);
                            }
                        }
                        if (visit)
                        {
                            //lock (nextNodeList)
                            // {
                            // nextNodeList.Add(node);
                            queue.Enqueue(node);
                            // }
                            //Thread.Sleep(1);
                        }
                    }
                }
            });

            return queue.ToList();
        }
    }
}
