using System.Collections.Generic;
using System;

namespace graphlib
{
    public class Dijkstra
    {
		public Dijkstra(int cP)
		{
			cheapestPath = cP;
		}
		List<int> shortestPath = new List<int>();
		public int cheapestPath = Int32.MaxValue;
    }

	public partial class Graph
	{
		public Dijkstra shortest_path(int source, int destination)
		{
			// Create priority queue
			Heap pq = new Heap();
			short [] visited = new short[graph.Count];
			int cheapestPath = Int32.MaxValue;
			// 0 - not visited, 1 - in queue, 2 - closed
			pq.add(source, 0);
			while(pq.size() > 0)
			{
				Heap.Vertex top = pq.pop();
				if(top.v == destination)
					cheapestPath = Math.Min(cheapestPath, top.cost);
				visited[indexes[top.v]] = 2;
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
						pq.add(neighbor, price);
						visited[v_index(neighbor)] = 1;
					}
					else if(pq.pos.ContainsKey(neighbor))
						pq.decrease_key(neighbor, top.cost+weight);
				}	
			}
			return new Dijkstra(cheapestPath);
		}
	}
}
