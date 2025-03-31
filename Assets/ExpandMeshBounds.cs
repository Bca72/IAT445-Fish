using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ExpandMeshBounds : MonoBehaviour
{
    public Vector3 extraBounds = new Vector3(0f, 1f, 0f); // Increase height upward

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null && mf.mesh != null)
        {
            Mesh mesh = mf.mesh;
            var bounds = mesh.bounds;
            bounds.Expand(extraBounds);
            mesh.bounds = bounds;
        }
    }
}

