using System.Collections.Generic;

namespace BfsTest.Net
{
    //Undirected
    public class Graph
    {
        private int verticals;//VerNum

        private int edges;//EdgeNum

        private List<int>[] adjacency;//Adjacency neighbors

        public Graph(int vertical)
        {
            verticals = vertical;
            edges = 0;

            //record the id of vertex to i-1(notice the size of the array)
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
            //Undirected
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
