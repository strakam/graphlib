using System.Collections.Generic;
using System;

namespace graphlib
{
    public class Dijkstra
    {
		public List<int> shortestPath = new List<int>();
		public int cheapestPath = Int32.MaxValue;
		public Dijkstra(int cP, List<int> shortestPath)
		{
			cheapestPath = cP;
			this.shortestPath = shortestPath;
			shortestPath.Reverse();
		}
    }

	public partial class Graph
	{
		public Dijkstra shortest_path(int source, int destination)
		{
			// Create priority queue
			Heap pq = new Heap();
			short [] visited = new short[graph.Count];
			int [] backtrack = new int[graph.Count];
			int cheapestPath = Int32.MaxValue;
			// 0 - not visited, 1 - in queue, 2 - closed
			pq.add(source, 0, source);
			backtrack[indexes[source]] = source;
			while(pq.size() > 0)
			{
				Heap.Vertex top = pq.pop();
				visited[indexes[top.v]] = 2;
				backtrack[indexes[top.v]] = top.parent;
				if(top.v == destination)
				{
					cheapestPath = Math.Min(cheapestPath, top.cost);
					break;
				}
				// for all neighbors
				for(int i = 0; i < graph[indexes[top.v]].Count; i++)
				{
					int neighbor = graph[indexes[top.v]][i].destination;
					int weight = graph[indexes[top.v]][i].weight;
					if(visited[v_index(neighbor)] == 2)
						continue;
					// If neighbor is not visited, add it to queue
					if(visited[v_index(neighbor)] == 0)
					{
						int price = top.cost + weight;
						pq.add(neighbor, price, top.v);
						visited[v_index(neighbor)] = 1;
					}
					else if(pq.pos.ContainsKey(neighbor))
						pq.decrease_key(neighbor, top.cost+weight, top.v);
				}	
			}
			int parent = destination;
			List<int> path = new List<int>();
			while(backtrack[indexes[parent]] != parent)
			{
				path.Add(parent);	
				parent = backtrack[indexes[parent]];
			}
			path.Add(source);
			return new Dijkstra(cheapestPath, path);
		}
	}
}
