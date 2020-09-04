using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public class basic
    {
		static void Main()
		{
            OrientedGraph g = new OrientedGraph();
            g.AddVertex(8);
            Console.WriteLine("fiesta");
            g.AddVertex(3);
            g.AddEdge(3, 8, 2);
		}
    }
}
