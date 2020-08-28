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
			fg.addVertex(1);
			fg.addVertex(2);
			fg.addVertex(3);
			fg.addVertex(4);
			fg.addEdge(1, 2, 1);
			fg.addEdge(1, 4, 1);
			fg.addEdge(2, 3, 2);
			fg.addEdge(4, 2, 1);
			fg.addEdge(1, 3, 3);
			fg.addEdge(3, 4, 1);

			// g
			g.addVertex(5);
			g.addVertex(8);
			g.addVertex(1);
			g.addVertex(6);
			g.addVertex(3);
			g.addVertex(2);
			g.addVertex(4);
			g.addVertex(7);
			g.addEdge(5, 1, 3);
			g.addEdge(1, 8, 4);
			g.addEdge(5, 8, 9);
			g.addEdge(1, 6, 2);
			g.addEdge(3, 6, 9);
			g.addEdge(3, 8, 3);
			g.addEdge(2, 8, 5);
			g.addEdge(2, 3, 1);
			g.addEdge(7, 3, 1);
			g.addEdge(7, 2, 2);
			g.addEdge(7, 4, 2);
			
			// og
			for(int i = 1; i < 7; i++)
				og.addVertex(i);
			og.addEdge(1, 2, 2);
			og.addEdge(1, 3, 1);
			og.addEdge(2, 3, 1);
			og.addEdge(1, 4, 4);
			og.addEdge(3, 6, 3);
			og.addEdge(4, 5, 3);
			og.addEdge(5, 6, 1);

			// g2
			for(int i = 1; i < 8; i++)
				g2.addVertex(i);
			g2.addEdge(1, 2);
			g2.addEdge(1, 4);
			g2.addEdge(2, 3);
			g2.addEdge(4, 3);
			g2.addEdge(4, 6);
			g2.addEdge(3, 5);
			g2.addEdge(6, 5);
			g2.addEdge(3, 7);
			g2.addEdge(6, 7);
        }
		[Test]
		public void testSCCS()
		{
			OrientedGraph s = new OrientedGraph();
			for(int i = 1; i < 8; i++)
				s.addVertex(i);
			s.addEdge(1, 2);	
			s.addEdge(2, 3);	
			s.addEdge(3, 1);	
			s.addEdge(2, 4);	
			s.addEdge(2, 5);	
			s.addEdge(5, 6);	
			s.addEdge(6, 7);	
			s.addEdge(7, 5);	
			List<List<long>> res = s.findSCCS();
			foreach(List<long> l in res)
			{
				foreach(long i in l)
					Console.Write(i + " ");
				Console.WriteLine();
			}
			Assert.AreEqual(3, res.Count);
			Assert.AreEqual(6, og.findSCCS().Count);
		}

		[Test]
		public void testAps()
		{
			Graph a = new Graph();
			for(int i = 1; i < 6; i++)
				a.addVertex(i);
			a.addEdge(1, 2);
			a.addEdge(3, 2);
			a.addEdge(3, 4);
			a.addEdge(4, 5);
			a.addEdge(3, 5);
			List<long> ans = a.findArticulations();
			Assert.AreEqual(new List<long>(){2,3}, ans);
			Assert.AreEqual(2, a.findBridges().Count);

			Graph t = new Graph();
			for(int i = 1; i < 8; i++)
				t.addVertex(i);
			t.addEdge(1, 3);	
			t.addEdge(2, 5);	
			t.addEdge(3, 5);	
			t.addEdge(3, 4);	
			t.addEdge(6, 4);	
			t.addEdge(6, 5);	
			t.addEdge(6, 7);	
			List<long> ans2 = t.findArticulations();
			Assert.AreEqual(3, t.findBridges().Count);
			Assert.AreEqual(new List<long>(){3, 5, 6}, ans2);

			ans = g.findArticulations();
			Assert.AreEqual(1, g.findBridges().Count);
			Assert.AreEqual(new List<long>(){7}, ans);
		}

		[Test]
		public void test_kruskal()
		{
			SpanningTree s = g.getSpanning();
			Assert.AreEqual(16, s.cost);
			Assert.AreEqual(s.edges.Count, 7);

			SpanningTree mst = mstGraph().getSpanning();
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
		public void testBipartity()
		{
			Bipartite b = g.checkBipartity();
			Assert.AreEqual(b.isBipartite, false);

			Bipartite b2 = g2.checkBipartity();
			Assert.AreEqual(true, b2.isBipartite);
			List<long> redNodes = new List<long>(){1, 3, 6};
			List<long> blueNodes = new List<long>(){2, 4, 5, 7};
			Assert.AreEqual(b2.redPart, redNodes);
			Assert.AreEqual(b2.bluePart, blueNodes);
		}

		[Test]
		public void testToposort()
		{
			List<long> order = og.topologicalOrdering();
			List<long> correct = new List<long>(){6, 3, 2, 5, 4, 1};
			Assert.AreEqual(correct, order);
			og.addEdge(3, 1, 1);
			order = og.topologicalOrdering();
			Assert.AreEqual(new List<long>(), order);
		}

		[Test]
		public void testHeap()
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
		public void testDijkstra()
		{
			Dijkstra a = g.findShortestPath(5, 4);
			Assert.AreEqual(13, a.cost);
			List<long> sa = new List<long>(){5, 1, 8, 3, 7, 4};
			Assert.AreEqual(sa , a.shortestPath);

			Dijkstra b = g.findShortestPath(4, 8);
			Assert.AreEqual(6, b.cost);
			List<long> sb = new List<long>(){4, 7, 3, 8};
			Assert.AreEqual(sb, b.shortestPath);

			Dijkstra c = g.findShortestPath(5, 8);
			Assert.AreEqual(7, c.cost);
			List<long> sc = new List<long>(){5, 1, 8};
			Assert.AreEqual(sc, c.shortestPath);
		}

		[Test]
		public void testFloyd()
		{
			long [,] ans = fg.floydWarshall();
			long [,] correct = new long[4, 4]
				{{0, 1, 2, 1}, {1, 0, 2, 1}, {2, 2, 0, 1}, {1, 1, 1, 0}};
			Assert.AreEqual(correct, ans);
		}

		Graph mstGraph()
		{
			Graph g = new Graph();
			g.addVertex(1);
			g.addVertex(2);
			g.addVertex(3);
			g.addVertex(4);
			g.addVertex(5);
			g.addVertex(6);
			g.addVertex(7);
			g.addVertex(8);
			g.addEdge(1, 4, 4);
			g.addEdge(1, 7, 1);
			g.addEdge(1, 6, 3);
			g.addEdge(1, 8, 7);
			g.addEdge(2, 3, 7);
			g.addEdge(2, 6, 3);
			g.addEdge(2, 5, 1);
			g.addEdge(2, 4, 3);
			g.addEdge(3, 4, 2);
			g.addEdge(3, 8, 3);
			g.addEdge(5, 4, 1);
			g.addEdge(5, 6, 2);
			g.addEdge(5, 7, 1);
			g.addEdge(6, 7, 1);
			g.addEdge(5, 4, 1);
			return g;
		}
    }
}
