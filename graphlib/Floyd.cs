using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public class Floyd
    {
		public List<List<int>> floydWarshall(List<List<int>> map)
		{
			int l = map.Count;
			for(int k = 0; k < l; k++)
				for(int i = 0; i < l; i++)
					for(int j = 0; j < l; j++)
						map[i][j] = Math.Min(map[i][j], map[i][k] + map[k][j]);
			return map;
		}
    }
}
