using UnityEngine;
using System.Collections;

public class CameraEdge : MonoBehaviour {

	public CameraNode node1;
	public CameraNode node2;
	public bool followPlayer;
	public float minHeight;
	public float maxHeight;

	// Use this for initialization
	void Start () {
        transform.position = node1.transform.position + (node2.transform.position - node1.transform.position)/2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public CameraEdge[] adjascentNodes(){
		CameraEdge[] edges = new CameraEdge[node1.edges.Length + node2.edges.Length];
		for (int i = 0; i < node1.edges.Length; i++) {
			edges[i] = node1.edges[i];
		}
		for (int i = 0; i < node2.edges.Length; i++) {
			edges[i + node1.edges.Length] = node2.edges[i];
		}
		return edges;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (node1 != null && node2 != null) {
            Gizmos.DrawLine(node1.transform.position, node2.transform.position);
        }
        
    }

    public void SetNodes(CameraNode n1, CameraNode n2) {
        node1 = n1;
        node2 = n2;
    }
}