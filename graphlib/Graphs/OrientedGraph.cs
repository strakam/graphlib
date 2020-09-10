using System;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// OrientedGraph is a second main class of this library. 
    /// It contains all algorithms that work on OrientedGraphs.
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
    /// <term>FindSCCs</term>
    /// <description>Method used to find all strongly connected components in a
    /// graph.</description>
    /// </item>
    /// <item>
    /// <term>TopologicalOrdering</term>
    /// <description>Used to find topological ordering of a graph.</description>
    /// </item>
    /// <item>
    public partial class OrientedGraph:SharedGraph
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
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that is a weight of added edge. </param>
        public override void AddEdge(int source, int destination, long weight)
        {
            if(Math.Max(source, destination) < graph.Count)
            {
                graph[source].Add(new Edge(source, destination, weight));
                Edges.Add(new Edge(source, destination, weight));
            }
        }

        /// <summary>
        /// Overriding of AddEdge
        /// AddEdge adds edge with given parameters to a graph with weight=1
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        public override void AddEdge(int source, int destination)
        {
            long weight = 1;
            if(Math.Max(source, destination) < graph.Count)
            {
                graph[source].Add(new Edge(source, destination, weight));
                Edges.Add(new Edge(source, destination, weight));
            }
        }

        /// <summary>
        /// RemoveEdge removes edge connecting given vertices from a graph.
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        public override bool RemoveEdge(int source, int destination)
        {
            if(Math.Max(source, destination) < graph.Count)
            {
                bool removed = false;
                for(int i = 0; i < graph[source].Count; i++)
                {
                    if(graph[source][i].destination == destination)
                    {
                        graph[source].RemoveAt(i); 
                        removed = true;
                    }
                }
                if(removed)
                {
                    for(int i = 0; i < Edges.Count; i++)
                    {
                        if(Edges[i].source == source 
                                && Edges[i].destination == destination)
                        {
                            Edges.RemoveAt(i);
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
            foreach(List<Edge> l in graph)
            {
                Console.Write("Susedia vrcholu {0}, su ", i);
                foreach(Edge e in l)
                {
                    Console.Write(e.destination + " ");
                }
                Console.WriteLine();
                i++;
            }
        }
    }
}
