using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public CameraNode node;
    public CameraEdge currentEdge;
    public Transform edgesObj;

    private readonly float timeStep = 6;
	private readonly float longTimeStep = 2;
    public static float height = 10f;

	private Vector3 newPos;
	private Quaternion newRot;
	public Vector3 TargetCameraRotation {get { return newRot.eulerAngles; }}

	[SerializeField] private bool addDirectionOffset = false ;

	private CharacterControl controller;

	Vector3 playerPos;
	Vector3 nodePos; 
	Vector3 nextNodePos;
	Vector3 nodesVec;
	Vector3 nodeDist;

	float totalDist;
	float playerDist;
	float ratio;
	
	float cameraHeight;
	float heightOffset;

	float yPos;
	float cameraDist;

    // Use this for initialization
    void Start()
    {
		controller = player.GetComponent<CharacterControl> ();
        TransformCamera(false);
    }

    // Update is called once per frame
    void Update()
    {
        TransformCamera(true);
    }

    private void TransformCamera(bool lerp) {
        foreach (CameraEdge edge in currentEdge.adjascentEdges().OrderBy(e => MinimumDistance3D(e.node1.transform.position,
                                                           e.node2.transform.position,
                                                           player.transform.position)).ToArray())
        {
            if (edge.minHeight != 0 || edge.maxHeight != 0)
            {
                if ((edge.minHeight == 0 ? true : player.transform.position.y > edge.minHeight) && (edge.maxHeight == 0 ? true : player.transform.position.y < edge.maxHeight))
                {
                    currentEdge = edge;
                    break;
                }
            }
            else
            {
                currentEdge = edge;
                break;
            }
        }

		playerPos = !addDirectionOffset ? player.position : player.position + new Vector3((controller.CurrentRotation < 180 && controller.CurrentRotation > 0) ? 1 : -1,0,0) * 1.2f;
        playerPos.y = 0;
        nodePos = currentEdge.node1.transform.position; nodePos.y = 0;
        nextNodePos = currentEdge.node2.transform.position; nextNodePos.y = 0;
        nodesVec = nextNodePos - nodePos;

        nodeDist = (currentEdge.node2.transform.position - currentEdge.node1.transform.position);

        // TODO change ratio calculation to ratio of distance between bisected edges between nodes

        totalDist = Vector3.Distance(nextNodePos, nodePos);
        playerDist = Vector3.Dot(playerPos - nodePos, nodesVec / nodesVec.magnitude);
        ratio = playerDist / totalDist;

        if (ratio < 0) ratio = 0;
        else if (ratio > 1) ratio = 1;

        cameraHeight = currentEdge.node1.transform.position.y + (currentEdge.node2.transform.position.y - currentEdge.node1.transform.position.y) * ratio;
        heightOffset = currentEdge.node1.heightOffset + (currentEdge.node2.heightOffset - currentEdge.node1.heightOffset) * ratio;

        yPos = currentEdge.followPlayer ? player.transform.position.y + 1.5f : controller.height + cameraHeight + 2f + heightOffset;
	
        newRot = Quaternion.Lerp(currentEdge.node1.transform.rotation, currentEdge.node2.transform.rotation, ratio);
        this.transform.rotation = lerp? Quaternion.Slerp(this.transform.rotation, newRot, longTimeStep * Time.deltaTime) : newRot;

        cameraDist = CalculateCameraDist(currentEdge, ratio);

        newPos = new Vector3(currentEdge.node1.transform.position.x + (currentEdge.node2.transform.position.x - currentEdge.node1.transform.position.x) * ratio, yPos, currentEdge.node1.transform.position.z + nodeDist.z * ratio);
        newPos = RotatePointAroundPivot(newPos - new Vector3(0, 0, cameraDist), newPos, new Vector3(0, newRot.eulerAngles.y, 0));
		this.transform.position = lerp? Vector3.MoveTowards (transform.position, newPos, Vector3.Distance(transform.position,newPos) /20) : newPos; 
    }
    
    public float CalculateCameraDist(CameraEdge currentEdge, float ratio)
    {
        float cameraDist = currentEdge.node1.cameraDist - currentEdge.node2.cameraDist;
        cameraDist = currentEdge.node1.cameraDist - cameraDist * ratio;
        return cameraDist;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir; 
        point = dir + pivot;
        return point;
    }

    private float MinimumDistance3D(Vector3 p1, Vector3 p2, Vector3 player)
    {
        Vector3 u = p2 - p1;
        Vector3 v = player - p1;
        float dot = Vector3.Dot(u, v) / Vector3.Dot(u, u);
        Vector3 projection = p1 + (dot) * u;
        if (dot < 0) return Vector3.Distance(player, p1);
        if (dot > 1) return Vector3.Distance(player, p2);
        return Vector3.Distance(player, projection);
    }

    private float LengthSquared(Vector2 v, Vector2 w)
    {
        float mag = ((w - v).magnitude);
        return mag * mag;
    }

}