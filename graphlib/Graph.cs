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
    public partial class Graph:IGraph
    {
        /* Internal representation of the graph is in form of list of neighbors
         * for every vertex. */
        protected List<List<Edge>> graph = new List<List<Edge>>();
        // Transposed graph.
        protected List<List<Edge>> gT = new List<List<Edge>>();
        /* indexes is Dictionary that maps names of vertices to their position
         * in internal representation. */
        protected Dictionary<long, int> indexes = new Dictionary<long, int>();
        // Size is a number of vertices
        public long size = 0;


        /// <summary>
        /// AddVertex adds vertex with given value to a graph.
        /// </summary>
        /// <returns>
        /// True, if vertex was added. False if vertex is already in graph.
        /// </returns>
        /// <param name="val"> Long that is an ID of added vertex. </param>
        public virtual bool AddVertex(long val)
        {
            if(indexes.ContainsKey(val))
            {
                return false;
            }
            graph.Add(new List<Edge>());
            gT.Add(new List<Edge>());
            indexes[val] = graph.Count-1;
            if(indexes[val] == -1)
            {
                indexes[val] = 0;
            }
            size++;
            return true;
        }

        /// <summary>
        /// RemoveVertex removes vertex with given value to a graph.
        /// </summary>
        /// <returns>
        /// True, if vertex was removed. False if vertex wasn't in a graph.
        /// </returns>
        /// <param name="v"> Long that is an ID of removed vertex. </param>
        public bool RemoveVertex(long v)
        {
            if(indexes.ContainsKey(v))
            {
                graph.RemoveAt(Vindex(v));
                foreach(List<Edge> l in graph)
                {
                    for(int i = 0; i < l.Count; i++)
                    {
                        if(l[i].destination == v)
                        {
                            l.RemoveAt(i);
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// AddVertex adds edge with given parameters to a graph.
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        /// <param name="weight"> Long that represents weight of edge. </param>
        public virtual void AddEdge(long v1, long v2, long weight)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                graph[indexes[v1]].Add(new Edge(v1, v2, weight));
                graph[indexes[v2]].Add(new Edge(v2, v1, weight));
            }
        }


        /// <summary>
        /// Overloading of AddEdge.
        /// It adds edge with given parameters to a graph with weight=1
        /// </summary>
        /// <param name="v1"> Long that is an ID of a first vertex. </param>
        /// <param name="v2"> Long that is an ID of a second vertex. </param>
        public virtual void AddEdge(long v1, long v2)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                long weight = 1;
                graph[indexes[v1]].Add(new Edge(v1, v2, weight));
                graph[indexes[v2]].Add(new Edge(v2, v1, weight));
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
        public virtual bool RemoveEdge(long v1, long v2)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                bool removed = false;
                for(int i = 0; i < graph[Vindex(v1)].Count; i++)
                {
                    if(graph[Vindex(v1)][i].destination == v2)
                    {
                        graph[Vindex(v1)].RemoveAt(i); 
                        removed = true;
                    }
                }
                for(int i = 0; i < graph[Vindex(v2)].Count; i++)
                {
                    if(graph[Vindex(v2)][i].destination == v1)
                    {
                        graph[Vindex(v2)].RemoveAt(i); 
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
        /// Function to translate name of vertex to its position.
        /// </summary>
        /// <param name="vertex"> Is an ID of vertex </param>
        public int Vindex(long vertex)
        {
            return indexes[vertex];
        }

        /// <summary>
        /// Prints neighbors of every edge in given graph
        /// </summary>
        public void PrintGraph()
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
