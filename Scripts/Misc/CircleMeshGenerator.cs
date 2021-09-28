using NaughtyAttributes;
using UnityEngine;

namespace Fossil
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class CircleMeshGenerator : MonoBehaviour
    {
        public float radius = 1.0f;
        [MinValue(3)]
        public int edgeVertexCount = 12;
        public bool ring;
        [ShowIf("ring")]
        [MinValue(0.0f)]
        public float ringWidth = 0.2f;

        MeshFilter meshFilter;

        void Update()
        {
            if (edgeVertexCount < 3)
            {
                edgeVertexCount = 3;
            }
            if (meshFilter != null)
            {
                Mesh m = new Mesh();
                Vector3[] vertices = new Vector3[edgeVertexCount * 2 + 2];
                Vector3[] uvs = new Vector3[edgeVertexCount * 2 + 2];
                int[] triangles = new int[edgeVertexCount * 2 * 3];
                for (int i = 0; i < edgeVertexCount + 1; i++)
                {
                    float radians = (float)i / edgeVertexCount * Mathf.PI * 2.0f;
                    Vector3 pos = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);
                    vertices[i * 2] = ring ? pos * (radius + ringWidth) : pos * radius;
                    uvs[i * 2] = new Vector2((float)i / edgeVertexCount, 1);

                    if (radius - ringWidth < 0 && ring)
                    {
                        vertices[i * 2 + 1] = Vector3.zero;
                        uvs[i * 2 + 1] = new Vector2((float)i / edgeVertexCount, -(radius - ringWidth) / (ringWidth * 2));
                    }
                    else
                    {
                        vertices[i * 2 + 1] = ring ? pos * (radius - ringWidth) : Vector3.zero;
                        uvs[i * 2 + 1] = new Vector2((float)i / edgeVertexCount, 0);
                    }

                    if (i < edgeVertexCount)
                    {
                        triangles[i * 2 * 3] = (i * 2 + 2) % (edgeVertexCount * 2 + 2);
                        triangles[i * 2 * 3 + 1] = i * 2 + 1;
                        triangles[i * 2 * 3 + 2] = i * 2;

                        triangles[i * 2 * 3 + 3] = i * 2 + 1;
                        triangles[i * 2 * 3 + 4] = (i * 2 + 2) % (edgeVertexCount * 2 + 2);
                        triangles[i * 2 * 3 + 5] = (i * 2 + 3) % (edgeVertexCount * 2 + 2);
                    }
                }
                m.SetVertices(vertices);
                m.SetTriangles(triangles, 0);
                m.SetUVs(0, uvs);
                meshFilter.mesh = m;
            }
            else
            {
                meshFilter = GetComponent<MeshFilter>();
            }
        }
    }
}