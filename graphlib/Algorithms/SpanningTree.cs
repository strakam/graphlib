using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    /* This class contains info about minimum spanning tree of a graph
     * MST finding is implemented via Kruskals algorithm so we use union-find
     * operations here */
    public class SpanningTreeInfo
    {
        // Mst tree
        public Graph mst = new Graph();
        // sum of weights of edes in the tree
        public long cost;

        public SpanningTreeInfo(ref Graph g, long cost)
        {
            this.mst = g;
            this.cost = cost;
        }
    }

    public static class SpanningTree
    {
        /* ufVertex is short for union-find Vertex. Its a different
         * representation of graph vertex that is useful for union find
         * operations */ 
        public struct ufVertex 
        {
            // Size - number of vertices hanged under given vertex
            // Parent - id of ancestor in union find tree
            public long size, parent;
            public ufVertex(long parent, long size)
            {
                this.size = size;
                this.parent = parent;
            }
        }

        // Comparator for sorting edges in ascending order
        public static int comparison(Edge a, Edge b)
        {
            return (int)a.weight - (int)b.weight;
        }

        // Union function - return bool indicating whether merge happened
        static bool union(Edge e, ref Dictionary<long, ufVertex> p)
        {
            // Union hanging by size
            long rootA = find(e.source, ref p);
            long rootB = find(e.destination, ref p);
            // If we are comparing two different trees, merge them into one
            if(rootA != rootB)
            {
                // Compare size of both trees and hang smaller one under the
                // bigger one to increase performance
                if(p[rootA].size > p[rootB].size)
                {
                    // Hanging operations using temporary variables
                    ufVertex ta = p[rootA];
                    ta.size += p[rootB].size;
                    p[rootA] = ta;

                    ufVertex tb = p[rootB];
                    tb.parent = rootA;
                    p[rootB] = tb;
                }
                // Else is the same as above just for opposite case
                else
                {
                    ufVertex tb = p[rootB];
                    tb.size += p[rootA].size;
                    p[rootB] = tb;

                    ufVertex ta = p[rootA];
                    ta.parent = rootB;
                    p[rootA] = ta;
                }
                return true;
            }
            return false;
        }

        // Recursively find root of a given vertex in union find tree
        static long find(long v, ref Dictionary<long, ufVertex> p)
        {
            // Path compression 
            long root = v;
            if(p[v].parent != v)
            {
                root = find(p[v].parent, ref p);
            }
            ufVertex t = p[v];
            t.parent = root;
            p[v] = t;
            return root;
        }

        /* Classic implementation of Kruskal -
         * 1. Get all edges
         * 2. Sort them by size
         * 3. Go one by one and add them into MST if needed */
        public static SpanningTreeInfo GetSpanning(ref Graph g)
        {
            Dictionary<long, List<Edge>> graph = g.graph;
            // List of all graph edges
            List<Edge> edges = new List<Edge>();

            // For every vertex there is union-find representation for it
            Dictionary<long, ufVertex> parents = 
                new Dictionary<long, ufVertex>();

            // Actual MST
            Graph mst = new Graph();

            // Edges that are used in MST
            long totalCost = 0;
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                mst.AddVertex(kp.Key);
                parents.Add(kp.Key, new ufVertex(kp.Key, 1));
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
                if(union(e, ref parents))
                {
                    g.AddEdge(e.source, e.destination, e.weight);
                    totalCost += e.weight;
                }
            }
            return new SpanningTreeInfo(ref mst, totalCost);
        }
    }
}
