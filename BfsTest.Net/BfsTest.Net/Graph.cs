using System.Collections.Generic;

namespace BfsTest.Net
{
    //无向图
    public class Graph
    {
        private int verticals;//顶点个数

        private int edges;//边的个数

        private List<int>[] adjacency;//顶点联接列表

        public Graph(int vertical)
        {
            verticals = vertical;
            edges = 0;

            //注意一个问题，我偷懒了，本身是从0开始计数，但是这边从1开始计数。。。
            //所以索引号即使节点id名字...
            //在BFS算法里也有+1的代码，注意数组边界问题
            adjacency = new List<int>[vertical + 1];
            for (int v = 0; v < vertical + 1; v++)
            {
                adjacency[v] = new List<int>();
            }
        }

        public int GetVerticals()
        {
            return verticals;
        }

        public void AddEdge(int verticalStart, int verticalEnd)
        {
            //无向图加两条
            adjacency[verticalStart].Add(verticalEnd);
            adjacency[verticalEnd].Add(verticalStart);
            ++edges;
        }

        public List<int> GetAdjacency(int vetical)
        {
            return adjacency[vetical];
        }
    }
}
