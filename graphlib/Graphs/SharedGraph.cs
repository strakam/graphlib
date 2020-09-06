using System;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// SharedGraph is a class that contains common methods and properties
    /// of Graph and Orientedgraph.
    /// </summary>
    public partial class SharedGraph
    {
        /// <summary>
        /// AddVertex adds vertex with given value to a graph.
        /// </summary>
        /// <returns>
        /// True, if vertex was added. False if vertex is already in graph.
        /// </returns>
        /// <param name="val"> Long that is an ID of added vertex. </param>
        public virtual void AddEdge(long v1, long v2, long weight){}
        public virtual void AddEdge(long v1, long v2){}
        public virtual bool RemoveEdge(long v1, long v2)
        {
            return false;
        }
        public bool AddVertex(long val)
        {
            if(graph.ContainsKey(val))
            {
                return false;
            }
            graph.Add(val, new List<Edge>());
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
        public bool RemoveVertex(long v)
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
        /// Function to print vertices and list their neighbors
        /// </summary>
        public void PrintGraph()
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
