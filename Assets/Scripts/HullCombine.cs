using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullCombine : MonoBehaviour
{
    
    void Start()
    {
        List<Mesh> Children = new List<Mesh>();
        foreach (Transform child in transform)
        {
            Children.Add(child.gameObject.GetComponent<Mesh>());
        }
        Debug.Log(Children);
        var mesh = CombineMeshes(Children);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private Mesh CombineMeshes(List<Mesh> meshes)
    {
        var combine = new CombineInstance[meshes.Count];
        for(int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
}
