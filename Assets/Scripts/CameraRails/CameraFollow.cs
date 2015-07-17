using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraFollow : MonoBehaviour
{
	public Transform player;
	public CameraNode node;
	public CameraEdge currentEdge;
	public Transform edgesObj;
	private readonly float timeStep = 3;
    public static float height = 10f;

	private float MinimumDistance3D (Vector3 p1, Vector3 p2, Vector3 player)
	{
		Vector3 u = p2 - p1;
		Vector3 v = player - p1;
		float dot = Vector3.Dot (u, v) / Vector3.Dot (u, u);
		Vector3 projection = p1 + (dot) * u;
		if (dot < 0) return Vector3.Distance (player, p1);
		if (dot > 1) return Vector3.Distance (player, p2);
		return Vector3.Distance (player, projection);
	}

	private float LengthSquared (Vector2 v, Vector2 w)
	{
		float mag = ((w - v).magnitude);
		return mag * mag;
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
        foreach (CameraEdge edge in currentEdge.adjascentEdges().OrderBy (e => MinimumDistance3D (e.node1.transform.position,
		                                                    e.node2.transform.position, 
		                                                    player.transform.position)).ToArray ()) {
			if (edge.minHeight != 0 || edge.maxHeight != 0) {
				if ((edge.minHeight == 0 ? true : player.transform.position.y > edge.minHeight) && (edge.maxHeight == 0 ? true : player.transform.position.y < edge.maxHeight) ) {
					currentEdge = edge;
					break;
				}
			} else{
				currentEdge = edge;
				break;
			}
		}
				       
		Vector3 playerPos = player.position;
        playerPos.y = 0;
		Vector3 nodePos = currentEdge.node1.transform.position; nodePos.y = 0;
		Vector3 nextNodePos = currentEdge.node2.transform.position; nextNodePos.y = 0;
		Vector3 nodesVec = nextNodePos - nodePos;

		Vector3 nodeDist = (currentEdge.node2.transform.position - currentEdge.node1.transform.position);

		float totalDist = Vector3.Distance (nextNodePos, nodePos);
		float playerDist = Vector3.Dot (playerPos - nodePos, nodesVec / nodesVec.magnitude);
		float ratio = playerDist / totalDist;

		if (ratio < 0) ratio = 0;
		else if (ratio > 1) ratio = 1;

		float cameraDist = currentEdge.node1.cameraDist - currentEdge.node2.cameraDist; 

        float cameraHeight = currentEdge.node1.transform.position.y + (currentEdge.node2.transform.position.y - currentEdge.node1.transform.position.y) * ratio;
        float heightOffset = currentEdge.node1.heightOffset + (currentEdge.node2.heightOffset - currentEdge.node1.heightOffset) * ratio;

		float yPos = currentEdge.followPlayer ? player.transform.position.y + 1.5f : player.GetComponentInChildren<CharacterControl> ().height + cameraHeight + 2f + heightOffset;

		Vector3 newPos = new Vector3 (currentEdge.node1.transform.position.x + (currentEdge.node2.transform.position.x - currentEdge.node1.transform.position.x) * ratio, yPos, currentEdge.node1.transform.position.z + nodeDist.z * ratio - (currentEdge.node1.cameraDist - cameraDist * ratio));
		this.transform.position = Vector3.Lerp (this.transform.position, newPos, timeStep * Time.deltaTime);

		Quaternion newRot = Quaternion.Lerp (currentEdge.node1.transform.rotation, currentEdge.node2.transform.rotation, ratio);
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, newRot, timeStep * Time.deltaTime);
	}

	public void NextNode (CameraNode newNode)
	{

	}
}