using System;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Graph is a main class of this library. It contains all the graph
    /// properties and algorithms
    /// </summary>
    /// <item>
    /// <term>AddVertex</term>
    /// <description>Adds vertex to a graph.</description>
    /// </item>
    /// <item>
    /// <term>RemoveVertex</term>
    /// <description>Removes vertex from a graph.</description>
    /// </item>
    /// <item>
    /// <term>AddEdge</term>
    /// <description>Adds edge to a graph.</description>
    /// </item>
    /// <item>
    /// <term>Remove Edge</term>
    /// <description>Removes edge to a graph.</description>
    /// </item>
    /// <item>
    /// <term>FindArticulations</term>
    /// <description>Method used to find articulation points.</description>
    /// </item>
    /// <item>
    /// <term>FindBridges</term>
    /// <description>Method used to find bridges in graph.</description>
    /// </item>
    /// <item>
    /// <term>GetSpanning</term>
    /// <description>Method that finds minimum spanning tree.</description>
    /// </item>
    public class Graph:SharedGraph
    {
        /// <summary>
        /// AddVertex adds vertex with given value to a graph.
        /// </summary>
        /// <returns>
        /// True, if vertex was added. False if vertex is already in graph.
        /// </returns>
        /// <param name="val"> Long that is an ID of added vertex. </param>
        public override int AddVertex()
        {
            graph.Add(new List<Edge>());
            return graph.Count-1;
        }

        /// <summary>
        /// AddEdge adds edge with given parameters to a graph.
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that represents weight of edge. </param>
        public override void AddEdge(int v1, int v2, long weight)
        {
            if(Math.Max(v1, v2) < graph.Count)
            {
                graph[v1].Add(new Edge(v1, v2, weight));
                graph[v2].Add(new Edge(v2, v1, weight));
                Edges.Add(new Edge(v1, v2, weight));
            }
        }


        /// <summary>
        /// Overloading of AddEdge.
        /// It adds edge with given parameters to a graph with weight=1
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        public override void AddEdge(int v1, int v2)
        {
            if(Math.Max(v1, v2) < graph.Count)
            {
                long weight = 1;
                graph[v1].Add(new Edge(v1, v2, weight));
                graph[v2].Add(new Edge(v2, v1, weight));
                Edges.Add(new Edge(v1, v2, weight));
            }
        }

        /// <summary>
        /// RemoveVertex removes edge connecting given vertices from a graph.
        /// </summary>
        /// <returns>
        /// True, if edge was removed. False if edge didn't exist.
        /// </returns>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        public override bool RemoveEdge(int v1, int v2)
        {
            if(Math.Max(v1, v2) < graph.Count)
            {
                bool removed = false;
                for(int i = 0; i < graph[v1].Count; i++)
                {
                    if(graph[v1][i].destination == v2)
                    {
                        graph[v1].RemoveAt(i); 
                        removed = true;
                    }
                }
                for(int i = 0; i < graph[v2].Count; i++)
                {
                    if(graph[v2][i].destination == v1)
                    {
                        graph[v2].RemoveAt(i); 
                    }
                }
                if(removed)
                {
                    for(int i = 0; i < Edges.Count; i++)
                    {
                        if((Edges[i].source == v1 && Edges[i].destination == v2)
                            || (Edges[i].source == v2 && Edges[i].destination == v1))
                        {
                            Edges.RemoveAt(i);
                            break;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Function to print vertices and list their neighbors
        /// </summary>
        public override void PrintGraph()
        {
            int i = 0;
            foreach(List<Edge> k in graph)
            {
                Console.Write("Susedia vrcholu {0}, su ", i);
                foreach(Edge e in k)
                {
                    Console.Write(e.destination + " ");
                }
                Console.WriteLine();
                i++;
            }
        }

        /// <summary>
        /// Method that computes number of components in a graph
        /// </summary>
        /// <returns>
        /// Int that represents number of components
        /// </returns>
        public int GetNumberOfComponents()
        {
            UFvertex [] uf = new UFvertex[graph.Count];
            HashSet<int> comps = new HashSet<int>();
            for(int i = 0; i < graph.Count; i++)
            {
                uf[i] = new UFvertex(i, 1);
            }
            foreach(Edge e in Edges)
            {
                UnionFind.Union(e, uf);
            }
            foreach(UFvertex uv in uf)
            {
                comps.Add(uv.parent);
            }
            return comps.Count;
        }
    }
}
