using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public class basic
    {
		static void Main()
		{
			Graph g = new Graph();
			g.add_vertex(5);
			g.add_vertex(8);
			g.add_vertex(1);
			g.add_edge(5, 1);
			g.add_edge(1, 8);
			g.print_graph();
		}
    }
}
