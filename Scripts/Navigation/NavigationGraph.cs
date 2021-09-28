using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace Fossil
{
    public class NavigationGraph : MonoBehaviour
    {
        public Graph graph;

        public float nodeRadius = 0.5f;

        Pathfinder pathfinder = null;

        private void Start()
        {
            if (GlobalReferenceProvider.TryFill(ref pathfinder) && graph)
            {
                pathfinder.AddGraph(graph.nodes);
            }
        }

        private void OnDestroy()
        {
            if (GlobalReferenceProvider.TryFill(ref pathfinder) && graph)
            {
                pathfinder.RemoveGraph(graph.nodes);
            }
        }

#if UNITY_EDITOR
        [Button]
        public void Generate()
        {
            pathfinder = FindObjectOfType<Pathfinder>();
            if (pathfinder)
            {
                graph = Graph.Create();

                foreach (Transform child in transform)
                {
                    graph.nodes.Add(Node.Create(child.position));
                }

                List<(Node node1, Node node2)> connections = new List<(Node node1, Node node2)>();
                foreach (Node node1 in graph.nodes)
                {
                    foreach (Node node2 in graph.nodes)
                    {
                        if (node1.Equals(node2))
                        {
                            continue;
                        }
                        Vector2 direction = node2.position - node1.position;
                        if (!Physics2D.Raycast(node1.position, direction, direction.magnitude, pathfinder.raycastMask))
                        {
                            connections.Add((node1, node2));
                        }
                    }
                }

                List<HashSet<Node>> islands = new List<HashSet<Node>>();
                foreach (Node node in graph.nodes)
                {
                    islands.Add(new HashSet<Node>(new Node[] { node }));
                }

                IEnumerable<(Node node1, Node node2)> orderedConnections = connections.OrderBy(x => (x.node1.position - x.node2.position).sqrMagnitude);
                List<(Node node1, Node node2)> minimumConnection = new List<(Node node1, Node node2)>();

                foreach ((Node node1, Node node2) connection in orderedConnections)
                {
                    HashSet<Node> island1 = islands.Find(x => x.Contains(connection.node1));
                    HashSet<Node> island2 = islands.Find(x => x.Contains(connection.node2));
                    if (island1 != island2)
                    {
                        minimumConnection.Add(connection);
                        island1.UnionWith(island2);
                        islands.Remove(island2);
                        if (islands.Count <= 1)
                        {
                            break;
                        }
                    }
                }

                foreach ((Node node1, Node node2) in minimumConnection)
                {
                    float distance = (node1.position - node2.position).magnitude;
                    node1.connections.Add(new Connection(node2, distance));
                    node2.connections.Add(new Connection(node1, distance));
                }

                string assetPath = Path.GetDirectoryName(gameObject.scene.path) + @"\" + Path.GetFileNameWithoutExtension(gameObject.scene.path) + @"\NavigationGraph.asset";
                if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(assetPath)))
                {
                    AssetDatabase.CreateFolder(Path.GetDirectoryName(Path.GetDirectoryName(assetPath)), Path.GetFileNameWithoutExtension(gameObject.scene.path));
                }
                AssetDatabase.CreateAsset(graph, assetPath);
                foreach (Node node in graph.nodes)
                {
                    AssetDatabase.AddObjectToAsset(node, graph);
                }
            }
        }
#endif

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (graph)
            {
                foreach (Node node in graph.nodes)
                {
                    foreach (Connection connection in node.connections)
                    {
                        Gizmos.DrawLine(node.position, connection.node.position);
                    }
                }
            }
            foreach (Transform child in transform)
            {
                Gizmos.DrawWireSphere(child.transform.position, nodeRadius);
            }
        }
    }
}