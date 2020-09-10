using graphlib;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace tests
{
    [TestFixture]
    public class Tests
    {
        SharedGraph fg = new Graph();

        [OneTimeSetUp]
        public void Setup()
        {
            // fg
            for(int i = 0; i < 5; i++)
            {
                fg.AddVertex();
            }
            fg.AddEdge(1, 2, 1);
            fg.AddEdge(1, 4, 1);
            fg.AddEdge(2, 3, 2);
            fg.AddEdge(4, 2, 1);
            fg.AddEdge(1, 3, 3);
            fg.AddEdge(3, 4, 1);
        }
        [Test]
        public void testAddRemoveEdgesVertices()
        {
            // Not oriented graph
            Graph g = new Graph();
            for(int i = 0; i < 6; i++)
            {
                g.AddVertex();
            }
            g.AddEdge(2, 3);
            g.AddEdge(1, 3);
            g.AddEdge(2, 5);
            g.AddEdge(3, 5);
            Assert.False(g.RemoveEdge(1, 2));
            Assert.False(g.RemoveEdge(5, 1));
            Assert.True(g.RemoveEdge(1, 3));
            Assert.True(g.RemoveEdge(2, 5));
            Assert.False(g.RemoveEdge(2, 5));

            // Oriented graph
            OrientedGraph og = new OrientedGraph();
            for(int i = 0; i < 7; i++)
            {
                og.AddVertex();
            }
            og.AddEdge(1, 4, 2);
            og.AddEdge(6, 1, 3);
            og.AddEdge(1, 3, 3);
            og.AddEdge(4, 6, 1);
            og.AddEdge(6, 4, 2);
            Assert.True(og.RemoveEdge(1, 4));
            Assert.False(og.RemoveEdge(1, 4));
            Assert.False(og.RemoveEdge(1, 6));
            Assert.False(og.RemoveEdge(3, 1));
            Assert.True(og.RemoveEdge(4, 6));
            Assert.True(og.RemoveEdge(1, 3));
        }

        [Test]
        public void testSCCS()
        {
            OrientedGraph s = new OrientedGraph();
            OrientedGraph og = oriented();
            for(int i = 1; i < 9; i++)
                s.AddVertex();
            s.AddEdge(1, 2);	
            s.AddEdge(2, 3);	
            s.AddEdge(3, 1);	
            s.AddEdge(2, 4);	
            s.AddEdge(2, 5);	
            s.AddEdge(5, 6);	
            s.AddEdge(6, 7);	
            s.AddEdge(7, 5);	
            int [] correct = new int[]{4, 3, 3, 3, 2, 1, 1, 1};
            int [] res = Scc.FindSCCS(s);
            Assert.AreEqual(correct, res);
            Assert.AreEqual(new int[]{7,6,5,4,3,2,1}, Scc.FindSCCS(og));
        }

        [Test]
        public void testAps()
        {
            Graph g = g1();
            Graph a = new Graph();
            for(int i = 1; i < 7; i++)
                a.AddVertex();
            a.AddEdge(1, 2);
            a.AddEdge(3, 2);
            a.AddEdge(3, 4);
            a.AddEdge(4, 5);
            a.AddEdge(3, 5);
            List<int> ans = BridgesArticulations.FindArticulations(a);
            Assert.AreEqual(new List<int>(){2,3}, ans);
            Assert.AreEqual(2, BridgesArticulations.FindBridges(a).Count);

            Graph t = new Graph();
            for(int i = 1; i < 9; i++)
                t.AddVertex();
            t.AddEdge(1, 3);	
            t.AddEdge(2, 5);	
            t.AddEdge(3, 5);	
            t.AddEdge(3, 4);	
            t.AddEdge(6, 4);	
            t.AddEdge(6, 5);	
            t.AddEdge(6, 7);	
            List<int> ans2 = BridgesArticulations.FindArticulations(t);
            Assert.AreEqual(3, BridgesArticulations.FindBridges(t).Count);
            Assert.AreEqual(new List<int>(){3, 5, 6}, ans2);

            ans = BridgesArticulations.FindArticulations(g);
            Assert.AreEqual(1, BridgesArticulations.FindBridges(g).Count);
            Assert.AreEqual(new List<int>(){7}, ans);
        }

        [Test]
        public void test_kruskal()
        {
            Graph g = g1();
            SpanningTreeInfo s = SpanningTree.GetSpanning(g);
            Assert.AreEqual(16, s.cost);

            Graph mg = mstGraph();
            SpanningTreeInfo mst = SpanningTree.GetSpanning(mg);
            Assert.AreEqual(10, mst.cost);
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
            Graph g = g1();
            BipartiteInfo b = Bipartite.CheckBipartity(g);
            Assert.AreEqual(b.isBipartite, false);

            Graph g2 = new Graph();
            for(int i = 0; i < 8; i++)
                g2.AddVertex();
            g2.AddEdge(1, 2);
            g2.AddEdge(1, 4);
            g2.AddEdge(2, 3);
            g2.AddEdge(4, 3);
            g2.AddEdge(0, 2);
            g2.AddEdge(4, 6);
            g2.AddEdge(3, 5);
            g2.AddEdge(6, 5);
            g2.AddEdge(3, 7);
            g2.AddEdge(6, 7);

            BipartiteInfo b2 = Bipartite.CheckBipartity(g2);
            Assert.AreEqual(true, b2.isBipartite);
            List<int> redNodes = new List<int>(){0, 1, 3, 6};
            List<int> blueNodes = new List<int>(){2, 4, 5, 7};
            Assert.AreEqual(b2.redPart, redNodes);
            Assert.AreEqual(b2.bluePart, blueNodes);
        }

        [Test]
        public void testToposort()
        {
            OrientedGraph og = oriented();
            List<int> order = Toposort.TopologicalOrdering(og);
            List<int> correct = new List<int>(){0, 6, 3, 2, 5, 4, 1};
            Assert.AreEqual(correct, order);
            og.AddEdge(3, 1, 1);
            order = Toposort.TopologicalOrdering(og);
            Assert.AreEqual(new List<int>(), order);
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
            SharedGraph g = g1();
            DijkstraInfo a = Dijkstra.FindShortestPath(g, 5, 4);
            Assert.AreEqual(13, a.cost);
            List<int> sa = new List<int>(){5, 1, 8, 3, 7, 4};
            Assert.AreEqual(sa , a.shortestPath);

            DijkstraInfo b = Dijkstra.FindShortestPath(g, 4, 8);
            Assert.AreEqual(6, b.cost);
            List<int> sb = new List<int>(){4, 7, 3, 8};
            Assert.AreEqual(sb, b.shortestPath);

            DijkstraInfo c = Dijkstra.FindShortestPath(g, 5, 8);
            Assert.AreEqual(7, c.cost);
            List<int> sc = new List<int>(){5, 1, 8};
            Assert.AreEqual(sc, c.shortestPath);
        }

        [Test]
        public void testFloyd()
        {
            FloydInfo ans = FloydWarshall.AllShortestPaths(fg);
            long m = long.MaxValue;
            long [,] correct = 
            new long[5, 5]{{0, m, m, m, m}, {m, 0, 1, 2, 1}, {m, 1, 0, 2, 1}, {m, 2, 2, 0, 1}, {m, 1, 1, 1, 0}};
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    Console.Write(ans.GetDistance(i, j) + " ");
                }
                Console.WriteLine();
            }
            Assert.AreEqual(correct, ans.map);
        }

        Graph mstGraph()
        {
            Graph g = new Graph();
            for(int i = 0; i < 9; i++)
            {
                g.AddVertex();
            }
            g.AddEdge(1, 4, 4);
            g.AddEdge(1, 7, 1);
            g.AddEdge(1, 6, 3);
            g.AddEdge(1, 8, 7);
            g.AddEdge(2, 3, 7);
            g.AddEdge(2, 6, 3);
            g.AddEdge(2, 5, 1);
            g.AddEdge(2, 4, 3);
            g.AddEdge(3, 4, 2);
            g.AddEdge(3, 8, 3);
            g.AddEdge(5, 4, 1);
            g.AddEdge(5, 6, 2);
            g.AddEdge(5, 7, 1);
            g.AddEdge(6, 7, 1);
            g.AddEdge(5, 4, 1);
            return g;
        }

        Graph g1()
        {
            Graph g = new Graph();
            for(int i = 0; i < 9; i++)
            {
                g.AddVertex();
            }
            g.AddEdge(5, 1, 3);
            g.AddEdge(1, 8, 4);
            g.AddEdge(5, 8, 9);
            g.AddEdge(1, 6, 2);
            g.AddEdge(3, 6, 9);
            g.AddEdge(3, 8, 3);
            g.AddEdge(2, 8, 5);
            g.AddEdge(2, 3, 1);
            g.AddEdge(7, 3, 1);
            g.AddEdge(7, 2, 2);
            g.AddEdge(7, 4, 2);
            return g;
        }
        OrientedGraph oriented()
        {
            OrientedGraph og = new OrientedGraph();
            for(int i = 0; i < 7; i++)
                og.AddVertex();
            og.AddEdge(1, 2, 2);
            og.AddEdge(1, 3, 1);
            og.AddEdge(2, 3, 1);
            og.AddEdge(1, 4, 4);
            og.AddEdge(3, 6, 3);
            og.AddEdge(4, 5, 3);
            og.AddEdge(5, 6, 1);
            return og;
        }
    }
}
