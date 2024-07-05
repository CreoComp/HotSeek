using UnityEngine;

public class TopColliderCreator : MonoBehaviour
{
    public float heightThreshold = 0.5f; // Высота, выше которой будет создан коллайдер

    void Start()
    {
        CreateTopCollider();
    }

    void CreateTopCollider()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter not found on the object!");
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Определяем верхние треугольники
        var topTriangles = new System.Collections.Generic.List<int>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];

            if (v1.y > heightThreshold && v2.y > heightThreshold && v3.y > heightThreshold)
            {
                topTriangles.Add(triangles[i]);
                topTriangles.Add(triangles[i + 1]);
                topTriangles.Add(triangles[i + 2]);
            }
        }

        // Создаем новый объект для коллайдера
        GameObject topColliderObject = new GameObject("TopCollider");
        topColliderObject.transform.position = transform.position;
        topColliderObject.transform.rotation = transform.rotation;
        topColliderObject.transform.localScale = transform.localScale;
        topColliderObject.transform.parent = transform;

        MeshFilter topMeshFilter = topColliderObject.AddComponent<MeshFilter>();
        MeshCollider topMeshCollider = topColliderObject.AddComponent<MeshCollider>();

        Mesh topMesh = new Mesh();
        topMesh.vertices = vertices;
        topMesh.triangles = topTriangles.ToArray();

        topMesh.RecalculateNormals();
        topMeshFilter.mesh = topMesh;
        topMeshCollider.sharedMesh = topMesh;
    }
}
