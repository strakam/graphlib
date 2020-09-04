using System.Collections.Generic;
using System;

namespace graphlib
{
    /// <summary>
    /// class Dijkstra contains informations about shortest path between two
    /// given vertices
    /// </summary>
    public class DijkstraInfo
    {
        /// <value> List of vertices in shortest path in order </value>
        public List<long> shortestPath = new List<long>();
        /// <value> Cost of the shortest path </value>
        public long cost = long.MaxValue;
        public DijkstraInfo(long cP, List<long> shortestPath)
        {
            cost = cP;
            this.shortestPath = shortestPath;
            shortestPath.Reverse();
        }
    }

    public static class Dijkstra
    {
        static Dictionary<long, List<Edge>> graph;
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
        public static DijkstraInfo FindShortestPath(ref SharedGraph g, long source, long destination)
        {
            graph = g.graph;
            // Create priority queue
            Heap pq = new Heap();
            Dictionary<long, byte> visited = new Dictionary<long, byte>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                visited.Add(kp.Key, 0);
            }
            // For every node, remember from where it was visited first
            Dictionary<long, long> backtrack = new Dictionary<long, long>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                backtrack.Add(kp.Key, long.MaxValue);
            }
            long cost = long.MaxValue;
            // 0 - not visited, 1 - in queue, 2 - closed
            pq.add(source, 0, source);
            backtrack[source] = source;

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
                    long neighbor = graph[top.v][i].destination;
                    long weight = graph[top.v][i].weight;
                    // Skip if the neighbor was already processed
                    if(visited[neighbor] == 2)
                    {
                        continue;
                    }
                    // If neighbor is not visited, add it to queue
                    if(visited[neighbor] == 0)
                    {
                        long price = top.cost + weight;
                        pq.add(neighbor, price, top.v);
                        visited[neighbor] = 1;
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
