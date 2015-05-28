using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraFollow : MonoBehaviour
{
	public Transform player;
	public CameraNode node;
	public CameraEdge CurrentEdge;
	public Transform edgesObj;
	private CameraEdge[] edges;
	private readonly float timeStep = 3;

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
		edges = edgesObj.GetComponentsInChildren<CameraEdge> (); //should just check adjascent edges
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (CameraEdge edge in edges.OrderBy (e => MinimumDistance3D (e.node1.transform.position,
		                                                    e.node2.transform.position, 
		                                                    player.transform.position)).ToArray ()) {
			if (edge.followPlayer) {
				if (edge.activationHeight < player.transform.position.y) {
					CurrentEdge = edge;
					break;
				}
			} else{
				CurrentEdge = edge;
				break;
			}
		}
				       
		Vector3 playerPos = player.position; playerPos.y = 0;
		Vector3 nodePos = CurrentEdge.node1.transform.position; nodePos.y = 0;
		Vector3 nextNodePos = CurrentEdge.node2.transform.position; nextNodePos.y = 0;
		Vector3 nodesVec = nextNodePos - nodePos;

		Vector3 nodeDist = (CurrentEdge.node2.transform.position - CurrentEdge.node1.transform.position);

		float totalDist = Vector3.Distance (nextNodePos, nodePos);
		float playerDist = Vector3.Dot (playerPos - nodePos, nodesVec / nodesVec.magnitude);
		float ratio = playerDist / totalDist;
		if (ratio < 0)ratio = 0;
		else if (ratio > 1)ratio = 1;

		float cameraDist = CurrentEdge.node1.cameraDist - CurrentEdge.node2.cameraDist; 

		float heightOffset = CurrentEdge.node1.heightOffset + (CurrentEdge.node2.heightOffset - CurrentEdge.node1.heightOffset) * ratio;

		float yPos = CurrentEdge.followPlayer ? player.transform.position.y + 2f + heightOffset : player.GetComponentInChildren<CharacterControl> ().height + 2f + heightOffset;

		Vector3 newPos = new Vector3 (CurrentEdge.node1.transform.position.x + (CurrentEdge.node2.transform.position.x - CurrentEdge.node1.transform.position.x) * ratio, yPos, CurrentEdge.node1.transform.position.z + nodeDist.z * ratio - (CurrentEdge.node1.cameraDist - cameraDist * ratio));
		this.transform.position = Vector3.Lerp (this.transform.position, newPos, timeStep * Time.deltaTime);

		Quaternion newRot = Quaternion.Lerp (CurrentEdge.node1.transform.rotation, CurrentEdge.node2.transform.rotation, ratio);
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, newRot, timeStep * Time.deltaTime);
	}

	public void NextNode (CameraNode newNode)
	{

	}
}