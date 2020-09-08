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
        public override bool AddVertex(long val)
        {
            if(graph.ContainsKey(val))
            {
                return false;
            }
            graph.Add(val, new List<Edge>());
            Vertices.Add(val);
            NumberOfVertices++;
            return true;
        }

        /// <summary>
        /// RemoveVertex removes vertex with given value to a graph.
        /// </summary>
        /// <returns>
        /// True, if vertex was removed. False if vertex wasn't in a graph.
        /// </returns>
        /// <param name="v"> Long that is an ID of removed vertex. </param>
        public override bool RemoveVertex(long v)
        {
            if(graph.ContainsKey(v))
            {
                graph.Remove(v);
                foreach(KeyValuePair<long, List<Edge>> l in graph)
                {
                    for(int i = 0; i < l.Value.Count; i++)
                    {
                        if(l.Value[i].destination == v)
                        {
                            l.Value.RemoveAt(i);
                            Vertices.Remove(v);
                            break;
                        }
                    }
                }
                NumberOfVertices--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// AddEdge adds edge with given parameters to a graph.
        /// </summary>
        /// <param name="source"> Long that is an ID of a first vertex. </param>
        /// <param name="destination"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that is a weight of added edge. </param>
        public void AddEdge(long source, long destination, long weight)
        {
            foreach(KeyValuePair<long, List<Edge>> k in graph)
            {
                Console.WriteLine(k.Key);
            }
            if(graph.ContainsKey(source) && graph.ContainsKey(destination))
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
        public override void AddEdge(long source, long destination)
        {
            long weight = 1;
            if(graph.ContainsKey(source) && graph.ContainsKey(destination))
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
        public override bool RemoveEdge(long source, long destination)
        {
            if(graph.ContainsKey(source) && graph.ContainsKey(destination))
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
            foreach(KeyValuePair<long, List<Edge>> k in graph)
            {
                Console.Write("Susedia vrcholu {0}, su ", k.Key);
                foreach(Edge e in k.Value)
                {
                    Console.Write(e.destination + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
