using System.Collections.Generic;
using System;

namespace graphlib
{
    /// <summary>
    /// class Dijkstra contains informations about shortest path between two
    /// given vertices
    /// </summary>
    public class Dijkstra
    {
        /// <value> List of vertices in shortest path in order </value>
        public List<long> shortestPath = new List<long>();
        /// <value> Cost of the shortest path </value>
        public long cost = long.MaxValue;
        public Dijkstra(long cP, List<long> shortestPath)
        {
            cost = cP;
            this.shortestPath = shortestPath;
            shortestPath.Reverse();
        }
    }

    public partial class Graph
    {
        /// <summary>
        /// Method finShortestPath is using Dijkstra's algorithm to find
        /// shortest path between two vertices.
        /// </summary>
        /// <returns>
        /// It returns instance of Dijkstra class, containing info about
        /// shortest path. If there is no path between given vertices,
        /// shortestPath parameter in Dijkstra instance is empty and distance is
        /// set to long.MaxValue.
        /// </returns>
        /// <param name="source"> long that is ID of source vertex of the
        /// shortest path </param>
        /// <param name="destination"> long that is ID of destination vertex of
        /// the shortest path </param>
        public Dijkstra FindShortestPath(long source, long destination)
        {
            // Create priority queue
            Heap pq = new Heap();
            short [] visited = new short[graph.Count];
            // For every node, remember from where it was visited first
            long [] backtrack = new long[graph.Count];
            long cost = long.MaxValue;
            // 0 - not visited, 1 - in queue, 2 - closed
            pq.add(source, 0, source);
            backtrack[indexes[source]] = source;

            // While there are unprocessed vertices do search
            while(pq.size() > 0)
            {
                /* Take closest reached vertex, mark it as visited and process
                 * its neighbors */
                Heap.Vertex top = pq.pop();
                visited[indexes[top.v]] = 2;
                // Set parent of the current node
                backtrack[indexes[top.v]] = top.parent;
                // If destination was reached, stop the search
                if(top.v == destination)
                {
                    cost = Math.Min(cost, top.cost);
                    break;
                }
                // for all neighbors
                for(int i = 0; i < graph[indexes[top.v]].Count; i++)
                {
                    long neighbor = graph[indexes[top.v]][i].destination;
                    long weight = graph[indexes[top.v]][i].weight;
                    // Skip if the neighbor was already processed
                    if(visited[Vindex(neighbor)] == 2)
                    {
                        continue;
                    }
                    // If neighbor is not visited, add it to queue
                    if(visited[Vindex(neighbor)] == 0)
                    {
                        long price = top.cost + weight;
                        pq.add(neighbor, price, top.v);
                        visited[Vindex(neighbor)] = 1;
                    }
                    // Upload cost if there was a better path found
                    else if(pq.position.ContainsKey(neighbor))
                    {
                        pq.decrease_key((int)neighbor, top.cost+weight, top.v);
                    }
                }	
            }
            long parent = destination;
            List<long> path = new List<long>();
            // Reconstruct the shortest path
            while(backtrack[indexes[parent]] != parent)
            {
                path.Add(parent);	
                parent = backtrack[indexes[parent]];
            }
            path.Add(source);
            // Return Dijkstra object with informations about shortest path
            return new Dijkstra(cost, path);
        }
    }
}
