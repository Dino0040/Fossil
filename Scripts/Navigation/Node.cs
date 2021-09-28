using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public class Node : ScriptableObject
    {
        public Vector2 position;
        public List<Connection> connections;

        [System.NonSerialized]
        public Node cameFrom = null;
        [System.NonSerialized]
        public bool explored = false;
        [System.NonSerialized]
        public float directCost = float.MaxValue;
        [System.NonSerialized]
        public float? targetCost = float.MaxValue;
        public float Cost => directCost + targetCost.Value;

        public void Reset()
        {
            cameFrom = null;
            explored = false;
            directCost = float.MaxValue;
            targetCost = null;
        }

        public static Node Create(Vector2 position)
        {
            Node node = CreateInstance<Node>();
            node.position = position;
            node.connections = new List<Connection>();
            return node;
        }
    }
}