using System;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
		protected List<List<Edge>> graph = new List<List<Edge>>();
		protected Dictionary<int, int> indexes = new Dictionary<int, int>();
		public int size = 0;

		public struct Edge
		{
			public int destination {get;set;}
			public int weight {get;set;}
			public Edge(int destination, int weight)
			{
				this.destination = destination;
				this.weight = weight;
			}
		}

		public void add_vertex(int val)
		{
			graph.Add(new List<Edge>());
			indexes[val] = graph.Count-1;
			if(indexes[val] == -1)
				indexes[val] = 0;
			size++;
		}

		public virtual void add_edge(int v1, int v2, int weight)
		{
			if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
			{
				graph[indexes[v1]].Add(new Edge(v2, weight));
				graph[indexes[v2]].Add(new Edge(v1, weight));
			}
			else
				Console.WriteLine("There are no such vertices!");
		}

		// Overloading for unweighted graph
		public virtual void add_edge(int v1, int v2)
		{
			if(indexes.ContainsKey(v1) && indexes.ContainsKey(v2))
			{
				int weight = 1;
				graph[indexes[v1]].Add(new Edge(v2, weight));
				graph[indexes[v2]].Add(new Edge(v1, weight));
			}
			else
				Console.WriteLine("There are no such vertices!");
		}

		public int v_index(int vertex)
		{
			return indexes[vertex];
		}

		public List<int> print_graph()
		{
			List<int> output = new List<int>();
			foreach(KeyValuePair<int, int> k in indexes)
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

	public partial class OrientedGraph:Graph
	{
		public override void add_edge(int source, int destination, int weight)
		{
			if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
				graph[indexes[source]].Add(new Edge(destination, weight));
			else
				Console.WriteLine("There are no such vertices!");
		}

		// Overloading for unweighted graph
		public override void add_edge(int source, int destination)
		{
			int weight = 1;
			if(indexes.ContainsKey(source) && indexes.ContainsKey(destination))
				graph[indexes[source]].Add(new Edge(destination, weight));
			else
				Console.WriteLine("There are no such vertices!");
		}
	}
}
