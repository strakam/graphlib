using graphlib;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;

namespace tests
{
	[TestFixture]
    public class Tests
    {
		Graph g  = new Graph();
		Graph g2 = new Graph();
		Graph fg = new Graph();
		OrientedGraph og = new OrientedGraph();

        [OneTimeSetUp]
        public void Setup()
        {
			// fg
			fg.add_vertex(1);
			fg.add_vertex(2);
			fg.add_vertex(3);
			fg.add_vertex(4);
			fg.add_edge(1, 2, 1);
			fg.add_edge(1, 4, 1);
			fg.add_edge(2, 3, 2);
			fg.add_edge(4, 2, 1);
			fg.add_edge(1, 3, 3);
			fg.add_edge(3, 4, 1);

			// g
			g.add_vertex(5);
			g.add_vertex(8);
			g.add_vertex(1);
			g.add_vertex(6);
			g.add_vertex(3);
			g.add_vertex(2);
			g.add_vertex(4);
			g.add_vertex(7);
			g.add_edge(5, 1, 3);
			g.add_edge(1, 8, 4);
			g.add_edge(5, 8, 9);
			g.add_edge(1, 6, 2);
			g.add_edge(3, 6, 9);
			g.add_edge(3, 8, 3);
			g.add_edge(2, 8, 5);
			g.add_edge(2, 3, 1);
			g.add_edge(7, 3, 1);
			g.add_edge(7, 2, 2);
			g.add_edge(7, 4, 2);
			
			// og
			for(int i = 1; i < 7; i++)
				og.add_vertex(i);
			og.add_edge(1, 2, 2);
			og.add_edge(1, 3, 1);
			og.add_edge(2, 3, 1);
			og.add_edge(1, 4, 4);
			og.add_edge(3, 6, 3);
			og.add_edge(4, 5, 3);
			og.add_edge(5, 6, 1);

			// g2
			for(int i = 1; i < 8; i++)
				g2.add_vertex(i);
			g2.add_edge(1, 2);
			g2.add_edge(1, 4);
			g2.add_edge(2, 3);
			g2.add_edge(4, 3);
			g2.add_edge(4, 6);
			g2.add_edge(3, 5);
			g2.add_edge(6, 5);
			g2.add_edge(3, 7);
			g2.add_edge(6, 7);
        }
		[Test]
		public void test_sccs()
		{
			OrientedGraph s = new OrientedGraph();
			for(int i = 1; i < 8; i++)
				s.add_vertex(i);
			s.add_edge(1, 2);	
			s.add_edge(2, 3);	
			s.add_edge(3, 1);	
			s.add_edge(2, 4);	
			s.add_edge(2, 5);	
			s.add_edge(5, 6);	
			s.add_edge(6, 7);	
			s.add_edge(7, 5);	
			List<List<int>> res = s.find_sccs();
			foreach(List<int> l in res)
			{
				foreach(int i in l)
					Console.Write(i + " ");
				Console.WriteLine();
			}
			Assert.AreEqual(3, res.Count);
			Assert.AreEqual(6, og.find_sccs().Count);
		}

		[Test]
		public void test_aps()
		{
			Graph a = new Graph();
			for(int i = 1; i < 6; i++)
				a.add_vertex(i);
			a.add_edge(1, 2);
			a.add_edge(3, 2);
			a.add_edge(3, 4);
			a.add_edge(4, 5);
			a.add_edge(3, 5);
			List<int> ans = a.find_aps();
			Assert.AreEqual(new List<int>(){2,3}, ans);
			Assert.AreEqual(2, a.find_bridges().Count);

			Graph t = new Graph();
			for(int i = 1; i < 8; i++)
				t.add_vertex(i);
			t.add_edge(1, 3);	
			t.add_edge(2, 5);	
			t.add_edge(3, 5);	
			t.add_edge(3, 4);	
			t.add_edge(6, 4);	
			t.add_edge(6, 5);	
			t.add_edge(6, 7);	
			List<int> ans2 = t.find_aps();
			Assert.AreEqual(3, t.find_bridges().Count);
			Assert.AreEqual(new List<int>(){3, 5, 6}, ans2);

			ans = g.find_aps();
			Assert.AreEqual(1, g.find_bridges().Count);
			Assert.AreEqual(new List<int>(){7}, ans);
		}

		[Test]
		public void test_kruskal()
		{
			SpanningTree s = g.get_spanning();
			Assert.AreEqual(16, s.cost);
			Assert.AreEqual(s.edges.Count, 7);

			SpanningTree mst = mst_graph().get_spanning();
			Assert.AreEqual(10, mst.cost);
			Assert.AreEqual(mst.edges.Count, 7);
			// Test for listing edges
			/* foreach(Edge e in mst.edges) */
			/* { */
			/* 	Console.WriteLine("From {0} to {1} with cost {2}", */ 
			/* 			e.source, e.destination, e.weight); */
			/* } */
			/* Assert.True(false); */
		}

		[Test]
		public void test_bipartity()
		{
			Bipartite b = g.check_bipartity();
			Assert.AreEqual(b.is_bipartite, false);

			Bipartite b2 = g2.check_bipartity();
			Assert.AreEqual(true, b2.is_bipartite);
			List<int> red_nodes = new List<int>(){1, 3, 6};
			List<int> blue_nodes = new List<int>(){2, 4, 5, 7};
			Assert.AreEqual(b2.red_part, red_nodes);
			Assert.AreEqual(b2.blue_part, blue_nodes);
		}

		[Test]
		public void test_toposort()
		{
			List<int> order = og.toposort();
			List<int> correct = new List<int>(){6, 3, 2, 5, 4, 1};
			Assert.AreEqual(correct, order);
			og.add_edge(3, 1, 1);
			order = og.toposort();
			Assert.AreEqual(new List<int>(), order);
		}

		[Test]
		public void test_heap()
		{
			Heap h = new Heap();
			h.add(3, 6, 1);
			h.add(2, 7, 1);
			h.add(4, 5, 1);
			Assert.AreEqual(h.pop().v, 4);
			h.add(0, 9, 1);
			Assert.AreEqual(h.pop().v, 3);
			Assert.AreEqual(h.size(), 2);
			h.add(4, 8, 1);
			Assert.AreEqual(h.pop().v, 2);
			Assert.AreEqual(h.pop().v, 4);
			Assert.AreEqual(h.pop().v, 0);
		}

		[Test]
		public void test_dijkstra()
		{
			Dijkstra a = g.shortest_path(5, 4);
			Assert.AreEqual(13, a.cheapestPath);
			List<int> sa = new List<int>(){5, 1, 8, 3, 7, 4};
			Assert.AreEqual(sa , a.shortestPath);

			Dijkstra b = g.shortest_path(4, 8);
			Assert.AreEqual(6, b.cheapestPath);
			List<int> sb = new List<int>(){4, 7, 3, 8};
			Assert.AreEqual(sb, b.shortestPath);

			Dijkstra c = g.shortest_path(5, 8);
			Assert.AreEqual(7, c.cheapestPath);
			List<int> sc = new List<int>(){5, 1, 8};
			Assert.AreEqual(sc, c.shortestPath);
		}

		[Test]
		public void test_floyd()
		{
			int [,] ans = fg.floydWarshall();
			int [,] correct = new int[4, 4]
				{{0, 1, 2, 1}, {1, 0, 2, 1}, {2, 2, 0, 1}, {1, 1, 1, 0}};
			Assert.AreEqual(correct, ans);
		}

		Graph mst_graph()
		{
			Graph g = new Graph();
			g.add_vertex(1);
			g.add_vertex(2);
			g.add_vertex(3);
			g.add_vertex(4);
			g.add_vertex(5);
			g.add_vertex(6);
			g.add_vertex(7);
			g.add_vertex(8);
			g.add_edge(1, 4, 4);
			g.add_edge(1, 7, 1);
			g.add_edge(1, 6, 3);
			g.add_edge(1, 8, 7);
			g.add_edge(2, 3, 7);
			g.add_edge(2, 6, 3);
			g.add_edge(2, 5, 1);
			g.add_edge(2, 4, 3);
			g.add_edge(3, 4, 2);
			g.add_edge(3, 8, 3);
			g.add_edge(5, 4, 1);
			g.add_edge(5, 6, 2);
			g.add_edge(5, 7, 1);
			g.add_edge(6, 7, 1);
			g.add_edge(5, 4, 1);
			return g;
		}
    }
}
