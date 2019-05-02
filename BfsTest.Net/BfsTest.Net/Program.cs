using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BfsTest.Net
{
    class Program
    {
        //private const string File_Path = "rmat_20.txt";
        static void Main(string[] args)
        {
            Console.WriteLine("Please Enter File Name in Release Folder:");
            string file_name = Console.ReadLine();
            StreamReader sR = new StreamReader(file_name);
            //StreamReader sR = new StreamReader(File_Path);
            //Graph g = new Graph(9);
            string line1;
            line1 = sR.ReadLine();
            string[] nums = line1.Split(' ');
            int verNum = int.Parse(nums[0]);
            Graph g = new Graph(verNum);
            int EdgeNum = int.Parse(nums[1]);
            char temp;
            char[] temp2 = new char[1];
            int v1, v2;
            int weight;
            int i = 0;
            string line;
            while ((line = sR.ReadLine()) != null)
            {
                string[] values = line.Split(' ');
                v1 = int.Parse(values[1]);
                v2 = int.Parse(values[2]);
                g.AddEdge(v1, v2);
            }

            BreadthFirstSearch bfs = new BreadthFirstSearch();

            while (true)
            {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter Number----（1 :NormalBFS，2: ParallelBFS1(find adjacency parallel)，3: ParallelBFS2(2 layer)，4: ParallelBFS3(lock-free)，5: ParallelBFS4(queue parallel)）");

                string cmd = Console.ReadLine();

                if ("1".Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Start-NormalBFS");
                    stopwatch.Start();
                    bfs.NormalBFS(g, 1);
                    stopwatch.Stop();

                    TimeSpan timespan = stopwatch.Elapsed;
                    Console.WriteLine("NormalBFS Takes:" + timespan.TotalMilliseconds + "ms");
                }
                else if ("2".Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Start ParallelBFS1");
                    stopwatch.Start();
                    bfs.ParallelBFS1(g, 1);
                    stopwatch.Stop();

                    TimeSpan timespan = stopwatch.Elapsed;
                    Console.WriteLine("ParallelBFS2 Takes:" + timespan.TotalMilliseconds + "ms");
                }
                else if ("3".Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Start-ParallelBFS2");
                    stopwatch.Start();
                    bfs.ParallelBFS2(g, 1);
                    stopwatch.Stop();

                    TimeSpan timespan = stopwatch.Elapsed;
                    Console.WriteLine("ParallelBFS2 Takes:" + timespan.TotalMilliseconds + "ms");
                }
                else if ("4".Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Start-ParallelBFS3");
                    stopwatch.Start();
                    bfs.ParallelBFS3(g, 1);
                    stopwatch.Stop();

                    TimeSpan timespan = stopwatch.Elapsed;
                    Console.WriteLine("ParallelBFS3 Takes:" + timespan.TotalMilliseconds + "ms");
                }
                else if ("5".Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Start-ParallelBFS4");
                    stopwatch.Start();
                    bfs.ParallelBFS4(g, 1);
                    stopwatch.Stop();

                    TimeSpan timespan = stopwatch.Elapsed;
                    Console.WriteLine("ParallelBFS4 Takes:" + timespan.TotalMilliseconds + "ms");
                }
                else
                {
                    Console.WriteLine("Bad Command");
                }
            }
        }
    }
}
