using UnityEngine;
using System.Collections;

public class BonusTrigger : MonoBehaviour
{

    public Vector3 playerLocation;
    public CameraEdge newEdge;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(playerLocation, 0.2f);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.position = playerLocation;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().currentEdge = newEdge;
            Destroy(this.gameObject);
        }
    }

}
