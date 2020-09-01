using System;
using System.IO;
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
    public partial class OrientedGraph:IGraph
    {
        /// <summary>
        /// AddEdge adds edge with given parameters to a graph.
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that is a weight of added edge. </param>
        public override void AddEdge(long source, long destination, long weight)
        {
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                graph[indexes[source]].Add(new Edge(source, destination, weight));
                gT[indexes[destination]].Add(new Edge(destination, source, weight));
            }
        }

        /// <summary>
        /// Overriding of AddEdge
        /// AddEdge adds edge with given parameters to a graph with weight=1
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        public override void AddEdge(long source, long destination)
        {
            long weight = 1;
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                graph[indexes[source]].Add(new Edge(source, destination, weight));
                gT[indexes[destination]].Add(new Edge(destination, source, weight));
            }
        }

        /// <summary>
        /// RemoveEdge removes edge connecting given vertices from a graph.
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        public override bool RemoveEdge(long source, long destination)
        {
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                bool removed = false;
                for(int i = 0; i < graph[Vindex(source)].Count; i++)
                {
                    if(graph[Vindex(source)][i].destination == destination)
                    {
                        graph[Vindex(source)].RemoveAt(i); 
                        removed = true;
                    }
                }
                if(removed)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Function to print vertices and list their neighbors
        /// </summary>
        public new void PrintGraph()
        {
            List<long> output = new List<long>();
            foreach(KeyValuePair<long, int> k in indexes)
            {
                Console.Write("Susedia vrcholu {0}, su ", k.Key);
                foreach(Edge e in graph[k.Value])
                {
                    output.Add(e.destination);
                    Console.Write(e.destination + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
