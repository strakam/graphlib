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
    public partial class Graph:SharedGraph
    {
        /* indexes is Dictionary that maps names of vertices to their position
         * in internal representation. */


        /// <summary>
        /// AddVertex adds edge with given parameters to a graph.
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that represents weight of edge. </param>
        public override void AddEdge(long v1, long v2, long weight)
        {
            if(graph.ContainsKey(v1) && graph.ContainsKey(v2))
            {
                graph[v1].Add(new Edge(v1, v2, weight));
                graph[v2].Add(new Edge(v2, v1, weight));
            }
        }


        /// <summary>
        /// Overloading of AddEdge.
        /// It adds edge with given parameters to a graph with weight=1
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        public override void AddEdge(long v1, long v2)
        {
            if(graph.ContainsKey(v1) && graph.ContainsKey(v2))
            {
                long weight = 1;
                graph[v1].Add(new Edge(v1, v2, weight));
                graph[v2].Add(new Edge(v2, v1, weight));
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
        public override bool RemoveEdge(long v1, long v2)
        {
            if(graph.ContainsKey(v1) && graph.ContainsKey(v2))
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
                    return true;
                }
            }
            return false;
        }
    }
}
