using UnityEngine;
using System.Collections;

public class CameraNode : MonoBehaviour {

	public CameraEdge[] edges;
	public float cameraDist = 3;
	public float heightOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    public void AddEdge(CameraEdge edge) {
        var newEdges = new CameraEdge[edges == null ? 1 : edges.Length + 1];
        if (edges != null)
        {
            for (int i = 0; i < edges.Length; i++)
            {
                newEdges[i] = edges[i];
            }
        }
        newEdges[newEdges.Length - 1] = edge;
        edges = newEdges;
    }
}
