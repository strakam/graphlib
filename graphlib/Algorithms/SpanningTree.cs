using System;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// This class contains info about minimum spanning tree of a graph
    /// MST finding is implemented via Kruskals algorithm so we use union-find
    /// operations here.
    ///  </summary>
    public class SpanningTreeInfo
    {
        /// <value> mst is graph that is a minimum spanning tree of input
        /// graph </value>
        public Graph mst = new Graph();
        /// <value> cost is a sum of weights of edes in the tree </value>
        public long cost;

        /// Constructor
        public SpanningTreeInfo(Graph g, long cost)
        {
            this.mst = g;
            this.cost = cost;
        }
    }

    /// <summary>
    /// SpanningTree is a static class containing Kruskals algorithm to find
    /// minimum spanning tree.
    /// </summary>
    public static class SpanningTree
    {
        /* UFvertex is short for union-find Vertex. Its a different
         * representation of graph vertex that is useful for union find
         * operations */ 
        // Comparator for sorting edges in ascending order
        static int comparison(Edge a, Edge b)
        {
            return (int)a.weight - (int)b.weight;
        }

        /// <summary>
        /// This method finds minimum spanning tree of an input graph
        /// </summary>
        /// <returns>
        /// It returns instance of SpanningTreeInfo class, that contains
        /// cost of MST and MST graph itself.
        /// </returns>
        /// <param name="g"> Is a graph in which MST will be found </param>
        public static SpanningTreeInfo GetSpanning(Graph g)
        {
            Dictionary<long, List<Edge>> graph = g.graph;
            // List of all graph edges
            List<Edge> edges = new List<Edge>();

            // For every vertex there is union-find representation for it
            Dictionary<long, UFvertex> parents = new Dictionary<long, UFvertex>();

            // Actual MST
            Graph mst = new Graph();

            // Edges that are used in MST
            long totalCost = 0;
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                mst.AddVertex(kp.Key);
                parents.Add(kp.Key, new UFvertex(kp.Key, 1));
            }
            // Get all edges
            for(int i = 0; i < graph.Count; i++)
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                foreach(Edge e in kp.Value)
                {
                    edges.Add(e);
                }
            }

            // Sort all edges
            edges.Sort(comparison);

            foreach(Edge e in edges)
            {
                /* If union happened, that means that current edge connected
                 * together two components of a graph so it is added into MST */
                if(UnionFind.Union(e, parents))
                {
                    mst.AddEdge(e.source, e.destination, e.weight);
                    totalCost += e.weight;
                    /* Console.WriteLine("Hrana z {0} do {1}", e.source, e.destination); */
                }
            }
            return new SpanningTreeInfo(mst, totalCost);
        }
    }
}
