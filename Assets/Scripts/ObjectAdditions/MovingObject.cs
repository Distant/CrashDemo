using UnityEngine;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player") { col.transform.parent = this.transform; print("player parent now " + this.name); }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.collider.tag == "Player" && col.transform.parent == this.transform) col.transform.parent = null;
    }
}