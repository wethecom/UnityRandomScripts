using UnityEngine;

public class MoldGenerator : MonoBehaviour
{
    public float moldProbability = 0.05f;
    public float moldSize = 0.1f;
    public float moldSpread = 0.2f;
    public Material moldMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateMold();
    }

    void GenerateMold()
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (Random.value < moldProbability)
            {
                Vector3 vertex = vertices[i];
                Vector2 uv = uvs[i];

                float size = Random.Range(moldSize * 0.5f, moldSize * 1.5f);
                float spread = Random.Range(moldSpread * 0.5f, moldSpread * 1.5f);

                Vector3 offset = Random.insideUnitSphere * spread;
                vertex += offset;

                Vector2 uvOffset = new Vector2(offset.x, offset.y);

                GameObject mold = new GameObject("Mold");
                mold.transform.position = vertex;
                mold.transform.parent = transform;

                MeshFilter moldMeshFilter = mold.AddComponent<MeshFilter>();
                MeshRenderer moldMeshRenderer = mold.AddComponent<MeshRenderer>();

                moldMeshFilter.mesh = CreateMoldMesh(size);
                moldMeshRenderer.material = moldMaterial;
                moldMeshRenderer.material.mainTextureOffset = uv + uvOffset;
            }
        }
    }

    Mesh CreateMoldMesh(float size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        float halfSize = size * 0.5f;

        vertices[0] = new Vector3(-halfSize, -halfSize, 0);
        vertices[1] = new Vector3(-halfSize, halfSize, 0);
        vertices[2] = new Vector3(halfSize, -halfSize, 0);
        vertices[3] = new Vector3(halfSize, halfSize, 0);

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(1, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;
    }
}
/*
This script generates mold on a GameObject's mesh based on a probability value (moldProbability)
, size (moldSize), and spread (moldSpread) parameters. It creates a new GameObject for each mold 
instance, with a mesh that is randomly sized and positioned around the original mesh's vertices.
 The mold GameObjects are parented to the original GameObject and use a specified `m
 */