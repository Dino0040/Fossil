using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public class Graph : ScriptableObject
    {
        public List<Node> nodes;

        public static Graph Create()
        {
            Graph graph = CreateInstance<Graph>();
            graph.nodes = new List<Node>();
            return graph;
        }
    }
}