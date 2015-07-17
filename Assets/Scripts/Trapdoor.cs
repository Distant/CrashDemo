using UnityEngine;
using System.Collections;
using System;

public class Trapdoor : MonoBehaviour, Triggerable {

    public enum Direction {
        HORIZONTAL, VERITCAL
    }

    private bool moving = false;
    public bool sticky;
    public bool requiresTrigger;

    public float openTime;
    public float closeTime;
    public Direction direction;
    private Animator anim;

    public Transform player;
    private bool active = true;
    public bool requirePlayer;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!moving && !requiresTrigger) {
            StartCoroutine(Move());
        }

        if (requirePlayer) {
            if (!sticky && player.position.z > transform.position.z)
            {
                sticky = true;
                Open();
            }

            else if (sticky && player.position.z < transform.position.z)
            {
                sticky = false;
                Close();
            }
        }
        
    }

    public IEnumerator Move()
    {
        moving = true;
        Open();
        yield return new WaitForSeconds(openTime + 0.5f);
        if (!sticky) {
            Close();
            yield return new WaitForSeconds(closeTime + 0.5f);
        }
        moving = false;
    }

    public void Open() {
        anim.SetBool("open", true);
    }
    public void Close() {
        anim.SetBool("open", false);
    }

    public void OnTrigger()
    {
        if (!moving) StartCoroutine(Move());
    }
}