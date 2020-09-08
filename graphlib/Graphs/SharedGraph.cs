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
        public Dictionary<long, List<Edge>> graph = 
            new Dictionary<long, List<Edge>>();

        /// <value> Number of vertices of a given graph </value>
        public long NumberOfVertices = 0;

        /// <value> List of all edges in a graph
        public List<Edge> Edges = new List<Edge>();
        /// <value> List of all vertices in a graph
        public List<long> Vertices = new List<long>();

        /* public abstract void AddEdge(long v1, long v2, long weight); */
        public abstract void AddEdge(long v1, long v2);
        public abstract bool RemoveEdge(long v1, long v2);
        public abstract bool AddVertex(long val);
        public abstract bool RemoveVertex(long v);
        public abstract void PrintGraph();
    }
}
