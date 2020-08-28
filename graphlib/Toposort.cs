using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class OrientedGraph:Graph
    {
        // Function that finds topologicalOrdering if it exists
		public List<long> topologicalOrdering()
		{
			List<long> order = new List<long>();
            /* In visited array there are 3 types of vertices
             * 0 - not visited
             * 1 - visited but not closed
             * 2 - visited and closed */
			long[] visited = new long[this.size];		
            /* If result is true, topological ordering is returned, else - empty
             * list is returned */
			bool result = true;
			foreach(KeyValuePair<long, int> kp in this.indexes)
			{
                // If child vertex is not visied, go search from it
				if(visited[vIndex(kp.Key)] == 0)
				{
					result = tsDFS(kp.Key, ref order, ref visited);
					if(!result)
                    {
						return new List<long>();
                    }
				}
			}
			return order;
		}

        /* tsDFS (topological sort dfs) is a modified dfs that searches the
         * graph and adds vertices to list in topological order */
        // First argument - current vertex
        // Second argument - list of all vertices in order
        // Third argument - arrays that tells status of vertices
		private bool tsDFS(long vertex, ref List<long> order, ref long[] visited)
		{
			int v = vIndex(vertex);
            // Set as visited but not closed
			visited[v] = 1;
			for(int i = 0; i < graph[v].Count; i++)
			{
				int neighbor = (int)graph[v][i].destination;
                // If child is unvisited, go search from there
				if(visited[vIndex(neighbor)] == 0)
				{
					bool res = tsDFS(neighbor, ref order, ref visited);
					if(!res)
                    {
                        return false;
                    }
				}
                // If a child is visited but not closed, we found a cycle
                // so topological ordering does not exist
				else if(visited[vIndex(neighbor)] == 1)
				{
					return false;
				}
			}
            // Set as closed and add to ordering
			visited[vIndex(vertex)] = 2;
			order.Add(vertex);
			return true;
		}
    }
}
