using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
    protected LevelManager levelManager;
    protected CharacterControl player;
    protected SoundManager soundManager;
    public bool breakable = true;

    // Use this for initialization
    public virtual void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void HitPlayer(GameObject g, Vector3 point)
    {
        //if (g.transform.position.y > transform.position.y + 0.5f && (player.Velocity.y < -2f || player.Jumping)) {
        print(point.y - transform.position.y);
        if (player.Spinning)
        {
            OnSpin();
        }
        else if (point.y >= transform.position.y + 0.24f && ((player.Velocity.y < -2f && !player.Jumping) || (player.Velocity.y < 0f && player.Jumping)))
        {
            OnBounce();
        }
    }

    public virtual void OnSpin()
    {
        Break();
    }

    public virtual void OnBounce()
    {
        if (player.Spinning)
        {
            player.Stop();
        }
        else
        {
            player.Jump(6f);
        }
        Break();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            CharacterControl player = col.GetComponentInParent<CharacterControl>();
            player.Touching(this);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            CharacterControl player = col.GetComponentInParent<CharacterControl>();
            player.NotTouching(this);
        }
    }

    public void Break()
    {
        if (breakable)
        {
            soundManager.PlayClipAtPoint("box_break", transform.position, 0.05f);
            levelManager.InventoryManager.BreakBox(5);
            gameObject.SetActive(false);
            player.NotTouching(this);
        }
    }
}