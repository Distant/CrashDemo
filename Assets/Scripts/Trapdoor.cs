using UnityEngine;
using System.Collections;
using System;

public class Trapdoor : MonoBehaviour, Triggerable {

    public Transform left;
    public Transform right;
    private bool moving = false;
    private float speed = 0.075f;
    public bool sticky;
    public float openTime;
    public float closeTime;

    // Use this for initialization
    void Start() {
       
    }

    // Update is called once per frame
    void Update() {
        if (!moving && !sticky) {
            StartCoroutine(Move());
        }
    }

    public IEnumerator Move()
    {
        moving = true;
        Vector3 initLeft = left.position;
        Vector3 initRight = right.position;
        while (true)
        {
            while ((left.position - initLeft).x > -1)
            {
                left.position -= new Vector3(speed,0,0);
                right.position += new Vector3(speed, 0, 0);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(openTime);
            if (sticky) { moving = false; break; }else
            {
                while ((left.position - initLeft).x < 0)
                {
                    left.position += new Vector3(speed, 0, 0);
                    right.position -= new Vector3(speed, 0, 0);
                    yield return new WaitForEndOfFrame();
                }

               
                yield return new WaitForSeconds(closeTime);
            }
        }
    }

    public void OnTrigger()
    {
        if (!moving && sticky) StartCoroutine(Move());
    }
}
