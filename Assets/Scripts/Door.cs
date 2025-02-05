using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float r;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] cols = Physics2D.CircleCastAll(transform.position, r, Vector2.up);
        bool playerfound = false;
        foreach (RaycastHit2D col in cols)
        {
            if(col.collider.gameObject.GetComponent<PlayerMovement>())
            {
                GetComponent<Collider2D>().enabled = false;
                playerfound = true;
            }
        }

        if(!playerfound)
                GetComponent<Collider2D>().enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, r);
    }
}
