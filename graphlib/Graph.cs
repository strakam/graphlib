using System;
using System.Collections.Generic;

namespace graphlib
{
    // Class describing edges in graph
    public class Edge
    {
        // In oriented graph names are self explanatory
        public long source {get; set;}
        public long destination {get;set;}
        public long weight {get;set;}
        public Edge(long source, long destination, long weight)
        {
            this.source = source;
            this.destination = destination;
            this.weight = weight;
        }
    }

    public partial class Graph
    {
        /* Internal representation of the graph is in form of list of neighbors
         * for every vertex */
        protected List<List<Edge>> graph = new List<List<Edge>>();
        // Transposed graph
        protected List<List<Edge>> gT = new List<List<Edge>>();
        /* indexes is Dictionary that maps names of vertices to their position
         * in internal representation */
        protected Dictionary<long, int> indexes = new Dictionary<long, int>();
        // Size is a number of vertices
        public long size = 0;


        // User calls this function to add vertex into the graph
        public virtual bool addVertex(long val)
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

        // Remove given vertex and all edges connected with it
        public bool removeVertex(long v)
        {
            if(indexes.ContainsKey(v))
            {
                graph.RemoveAt(vIndex(v));
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

        // User calls this function to add edge into the graph
        public virtual void addEdge(long v1, long v2, long weight)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                graph[indexes[v1]].Add(new Edge(v1, v2, weight));
                graph[indexes[v2]].Add(new Edge(v2, v1, weight));
            }
        }


        // Overloading for unweighted graph
        public virtual void addEdge(long v1, long v2)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                long weight = 1;
                graph[indexes[v1]].Add(new Edge(v1, v2, weight));
                graph[indexes[v2]].Add(new Edge(v2, v1, weight));
            }
        }

        // Remove edge from both vertices
        public virtual bool removeEdge(long v1, long v2)
        {
            if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
            {
                bool removed = false;
                for(int i = 0; i < graph[vIndex(v1)].Count; i++)
                {
                    if(graph[vIndex(v1)][i].destination == v2)
                    {
                        graph[vIndex(v1)].RemoveAt(i); 
                        removed = true;
                    }
                }
                for(int i = 0; i < graph[vIndex(v2)].Count; i++)
                {
                    if(graph[vIndex(v2)][i].destination == v1)
                    {
                        graph[vIndex(v2)].RemoveAt(i); 
                    }
                }
                if(removed)
                {
                    return true;
                }
            }
            return false;
        }

        // Function to translate name of vertex to its position
        public int vIndex(long vertex)
        {
            return indexes[vertex];
        }

        // Function to print vertices and list their neighbors
        public List<long> printGraph()
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
            return output;
        }

    }

    /* Class for oriented graph has the same logic as Graph class 
     * but edges are added only in one way */
    public partial class OrientedGraph:Graph
    {
        public override void addEdge(long source, long destination, long weight)
        {
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                graph[indexes[source]].Add(new Edge(source, destination, weight));
                gT[indexes[destination]].Add(new Edge(destination, source, weight));
            }
        }

        public override void addEdge(long source, long destination)
        {
            long weight = 1;
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                graph[indexes[source]].Add(new Edge(source, destination, weight));
                gT[indexes[destination]].Add(new Edge(destination, source, weight));
            }
        }

        // Overriding for oriented graph
        public override bool removeEdge(long source, long destination)
        {
            if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
            {
                bool removed = false;
                for(int i = 0; i < graph[vIndex(source)].Count; i++)
                {
                    if(graph[vIndex(source)][i].destination == destination)
                    {
                        graph[vIndex(source)].RemoveAt(i); 
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
    }
}
