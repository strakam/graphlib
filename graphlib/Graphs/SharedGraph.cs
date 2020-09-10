using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// SharedGraph is a class that contains common methods and properties
    /// of Graph and Orientedgraph.
    /// </summary>
    public abstract class SharedGraph
    {
        /// <value> Inner representation of the graph </value>
        public List<List<Edge>> graph = new List<List<Edge>>();

        /// <value> Number of vertices of a given graph </value>
        public int NumberOfVertices = 0;

        /// <value> List of all edges in a graph
        public List<Edge> Edges = new List<Edge>();

        /* public abstract void AddEdge(long v1, long v2, long weight); */
        public abstract int AddVertex();
        public abstract void AddEdge(int v1, int v2);
        public abstract void AddEdge(int v1, int v2, long weight);
        public abstract bool RemoveEdge(int v1, int v2);
        public abstract void PrintGraph();
    }
}
