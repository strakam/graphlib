using System;
using System.Collections.Generic;

namespace graphlib
{
    interface IGraph
    {
        bool AddVertex(long v);
        bool RemoveVertex(long v);
        void AddEdge(long source, long destination);
        bool RemoveEdge(long source, long destination);
        void PrintGraph();
        int Vindex(long v);
    }
}
