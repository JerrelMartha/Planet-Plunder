using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleCrater2D : MonoBehaviour
{
    [Header("Circle Mesh")]
    public float radius = 5f;
    public int rings = 20;        // more rings = more middle detail
    public int segments = 64;     // more segments = smoother circle

    [Header("Crater Settings")]
    public float craterRadius = 1f;
    public float craterDepth = 0.6f;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    Camera cam;

    void Start()
    {
        cam = Camera.main;

        mesh = new Mesh();
        mesh.name = "Circle Crater Mesh";

        GetComponent<MeshFilter>().mesh = mesh;

        // Give it a material if you forgot one
        var mr = GetComponent<MeshRenderer>();
        if (mr.sharedMaterial == null)
            mr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));

        GenerateMesh();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            MakeCrater(mouseWorld, craterRadius, craterDepth);
        }
    }

    void GenerateMesh()
    {
        // Vertex count:
        // ring 0 = 1 vertex (center)
        // ring 1..rings = (segments + 1) vertices each (extra for seam)
        int vertsPerRing = segments + 1;
        int vertexCount = 1 + rings * vertsPerRing;

        vertices = new Vector3[vertexCount];

        // Center vertex
        vertices[0] = Vector3.zero;

        // Build rings
        int v = 1;
        for (int r = 1; r <= rings; r++)
        {
            float t = r / (float)rings;
            float ringRadius = t * radius;

            for (int i = 0; i <= segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;

                float x = Mathf.Cos(angle) * ringRadius;
                float y = Mathf.Sin(angle) * ringRadius;

                vertices[v] = new Vector3(x, y, 0f);
                v++;
            }
        }

        // Triangles:
        // - Center fan to ring 1
        // - Then quads between ring r and r+1 split into 2 triangles
        int centerTris = segments * 3;
        int ringTris = (rings - 1) * segments * 6;
        triangles = new int[centerTris + ringTris];

        int tIndex = 0;

        // ---- Center fan (center -> ring 1)
        int ring1Start = 1;
        for (int i = 0; i < segments; i++)
        {
            triangles[tIndex++] = 0;
            triangles[tIndex++] = ring1Start + i;
            triangles[tIndex++] = ring1Start + i + 1;
        }

        // ---- Rings (between ring r and ring r+1)
        for (int r = 1; r < rings; r++)
        {
            int innerStart = 1 + (r - 1) * vertsPerRing;
            int outerStart = 1 + r * vertsPerRing;

            for (int i = 0; i < segments; i++)
            {
                int a = innerStart + i;
                int b = innerStart + i + 1;
                int c = outerStart + i;
                int d = outerStart + i + 1;

                // Triangle 1
                triangles[tIndex++] = a;
                triangles[tIndex++] = c;
                triangles[tIndex++] = b;

                // Triangle 2
                triangles[tIndex++] = b;
                triangles[tIndex++] = c;
                triangles[tIndex++] = d;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void MakeCrater(Vector2 worldPos, float craterR, float depth)
    {
        // Convert world position into local mesh space
        Vector3 localHit = transform.InverseTransformPoint(worldPos);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v2 = vertices[i];

            float dist = Vector2.Distance(v2, localHit);

            if (dist > craterR)
                continue;

            // Smooth falloff: 1 at center, 0 at edge
            float t = 1f - (dist / craterR);

            // Nice smooth curve
            float falloff = t * t;

            // Push inward toward crater center (shrinks surface = crater)
            Vector2 dir = (v2 - (Vector2)localHit).normalized;

            // If it's exactly center, skip direction push
            if (dir.sqrMagnitude < 0.0001f)
                continue;

            vertices[i] -= (Vector3)(dir * (falloff * depth));
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void OnDrawGizmosSelected()
    {
        // Shows circle radius in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
