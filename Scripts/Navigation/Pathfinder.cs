using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Fossil
{
    public class Pathfinder : MonoBehaviour
    {
        List<Node> nodes;
        public LayerMask raycastMask;

        void Awake()
        {
            GlobalReferenceProvider.Register(typeof(Pathfinder), this);
            nodes = new List<Node>();
        }

        public void AddGraph(List<Node> nodes)
        {
            this.nodes.AddRange(nodes);
        }

        public void RemoveGraph(List<Node> nodes)
        {
            this.nodes = this.nodes.Except(nodes).ToList();
        }

        public Vector2 GetWalkDirection(Vector2 start, Vector2 end, float bodySize)
        {
            List<Node> nodeFront = new List<Node>();
            HashSet<Node> endNodes = new HashSet<Node>();

            Vector2 direction = end - start;
            if (!Physics2D.CircleCast(start, bodySize, direction, direction.magnitude, raycastMask))
            {
                return direction;
            }

            nodeFront.Clear();
            endNodes.Clear();

            foreach (Node node in nodes)
            {
                node.Reset();

                direction = node.position - start;
                float distance = direction.magnitude;
                if (!Physics2D.CircleCast(start, bodySize, direction, distance, raycastMask))
                {
                    node.directCost = distance;
                    node.targetCost = (end - node.position).magnitude;
                    nodeFront.Add(node);
                }
            }

            foreach (Node node in nodes)
            {
                direction = node.position - end;
                float distance = direction.magnitude;
                if (!Physics2D.CircleCast(end, bodySize, direction, distance, raycastMask))
                {
                    endNodes.Add(node);
                }
            }

            int i = 0;
            while (nodeFront.Count > 0 && i++ < 9999)
            {
                nodeFront.Sort((x, y) =>
                {
                    if (x.Cost == y.Cost)
                        return 0;
                    return x.Cost > y.Cost ? 1 : -1;
                });

                Node exploringNode = nodeFront[0];
                if (endNodes.Contains(exploringNode))
                {
                    while (exploringNode.cameFrom != null)
                    {
                        exploringNode = exploringNode.cameFrom;
                    }
                    return exploringNode.position - start;
                }
                foreach (Connection connection in exploringNode.connections)
                {
                    if (connection.node.explored)
                    {
                        continue;
                    }
                    if (connection.distance < connection.node.directCost)
                    {
                        connection.node.directCost = connection.distance;
                        connection.node.cameFrom = exploringNode;
                    }
                    if (!connection.node.targetCost.HasValue)
                    {
                        connection.node.targetCost = (end - connection.node.position).magnitude;
                    }
                    nodeFront.Add(connection.node);
                }

                exploringNode.explored = true;
                nodeFront.RemoveAt(0);
            }
            return Vector2.zero;
        }
    }
}