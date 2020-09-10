using System.Collections.Generic;
using System;

namespace graphlib
{
    /// <summary>
    /// Class DijkstraInfo contains informations about shortest path between two
    /// given vertices
    /// </summary>
    public class DijkstraInfo
    {
        /// <value> List of vertices in shortest path in order </value>
        public List<int> shortestPath = new List<int>();
        /// <value> Cost of the shortest path </value>
        public long cost = long.MaxValue;
        public DijkstraInfo(long cP, List<int> shortestPath)
        {
            cost = cP;
            this.shortestPath = shortestPath;
            shortestPath.Reverse();
        }
    }

    /// <summary>
    /// Class Dijkstra contains implementation of FindShortestPath algorithm
    /// that finds shortest path between two given vertices
    /// </summary>
    public static class Dijkstra
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
        public static DijkstraInfo FindShortestPath(SharedGraph g, 
            int source, int destination)
        {
            List<List<Edge>> graph = g.graph;
            // Create priority queue
            Heap pq = new Heap();

            // 0 - not visited, 1 - in queue, 2 - closed
            byte [] visited = new byte[graph.Count];

            // For every node, remember from where it was visited first
            int [] backtrack = new int[graph.Count];
            for(int i = 0; i < graph.Count; i++)
            {
                backtrack[i] = i;
            }
            long cost = long.MaxValue;
            pq.add(source, 0, source);

            // While there are unprocessed vertices do search
            while(pq.size() > 0)
            {
                /* Take closest reached vertex, mark it as visited and process
                 * its neighbors */
                Heap.Vertex top = pq.pop();
                visited[top.v] = 2;
                // Set parent of the current node
                backtrack[top.v] = top.parent;
                // If destination was reached, stop the search
                if(top.v == destination)
                {
                    cost = Math.Min(cost, top.cost);
                    break;
                }
                // for all neighbors
                for(int i = 0; i < graph[top.v].Count; i++)
                {
                    int neighbor = graph[top.v][i].destination;
                    long weight = graph[top.v][i].weight;
                    // Skip if the neighbor was already processed
                    if(visited[neighbor] == 2)
                    {
                        continue;
                    }
                    // If neighbor is not visited, add it to queue
                    else if(visited[neighbor] == 0)
                    {
                        long price = top.cost + weight;
                        pq.add(neighbor, price, top.v);
                        visited[neighbor] = 1;
                    }
                    // Upload cost if there was a better path found
                    else if(pq.position.ContainsKey(neighbor))
                    {
                        pq.decrease_key(neighbor, top.cost+weight, top.v);
                    }
                }	
            }
            int parent = destination;
            List<int> path = new List<int>();
            // Reconstruct the shortest path
            while(backtrack[parent] != parent)
            {
                path.Add(parent);	
                parent = backtrack[parent];
            }
            path.Add(source);
            // Return Dijkstra object with informations about shortest path
            return new DijkstraInfo(cost, path);
        }
    }
}
