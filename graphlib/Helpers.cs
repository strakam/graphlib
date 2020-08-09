using System.Collections.Generic;

namespace graphlib
{
	public struct Bipartite
	{
		public bool is_bipartite {get; set;}
		public List<int> red_part, blue_part;

	}
    public partial class Graph
    {
		public Bipartite check_bipartity()
		{
			Bipartite bp = new Bipartite();
			bp.is_bipartite = true;
			short [] color = new short[size];	
			bool res = true;
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(color[v_index(kp.Key)] == 0)
				{
					res = cb(kp.Key, 1, ref color);
					if(!res)
					{
						bp.is_bipartite = false;
						break;
					}
				}
			}
			if(res)
			{
				bp.red_part = new List<int>();
				bp.blue_part = new List<int>();
				foreach(KeyValuePair<int, int> kp in indexes)
				{
					if(color[v_index(kp.Key)] == 1)
						bp.red_part.Add(kp.Key);
					else 
						bp.blue_part.Add(kp.Key);
				}
			}
			return bp;
		}

		private bool cb(int vertex, short color, ref short [] colors)
		{
			int v = v_index(vertex);			
			bool res = true;
			colors[v] = color;
			for(int i = 0; i < graph[v].Count; i++)
			{
				int neighbor = graph[v][i].destination;
				if(colors[v_index(neighbor)] == 0)
				{
					res = cb(neighbor, (short)(color * -1), ref colors);
				}
				else if(colors[v_index(neighbor)] == color)
					res = false;
				if(!res) return false;
			}
			return true;
		}

    }
}
