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
		public virtual void addVertex(long val)
		{
			graph.Add(new List<Edge>());
			gT.Add(new List<Edge>());
			indexes[val] = graph.Count-1;
			if(indexes[val] == -1)
            {
				indexes[val] = 0;
            }
			size++;
		}

        // User calls this function to add edge into the graph
		public virtual void addEdge(long v1, long v2, long weight)
		{
			if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
			{
				graph[indexes[v1]].Add(new Edge(v1, v2, weight));
				graph[indexes[v2]].Add(new Edge(v2, v1, weight));
			}
			else
            {
                // TODO tu treba dat vynimku
				Console.WriteLine("There are no such vertices!");
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
			else
				Console.WriteLine("There are no such vertices!");
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
				Console.Write("Susedia vrcholu {0} s indexom {1}, su ", k.Key, k.Value);
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
			else
				Console.WriteLine("There are no such vertices!");
		}

		// Overloading for unweighted graph
		public override void addEdge(long source, long destination)
		{
			long weight = 1;
			if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
			{
				graph[indexes[source]].Add(new Edge(source, destination, weight));
				gT[indexes[destination]].Add(new Edge(destination, source, weight));
			}
			else
            {
				Console.WriteLine("There are no such vertices!");
            }
		}
	}
}
