using graphlib;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace tests
{
	[TestFixture]
    public class Tests
    {
		Graph g = new Graph();
        [SetUp]
        public void Setup()
        {
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
        }

        /* [Test] */
        /* public void test_print_graph() */
        /* { */
			/* List<int> correct = new List<int>(){1, 1, 5, 8}; */
			/* Assert.AreEqual(correct, g.print_graph()); */
        /* } */
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
    }
}
