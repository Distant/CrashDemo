using UnityEngine;
using System.Collections;

public class RotatingBlocks : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(new Vector3(0, 0, 75 * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
