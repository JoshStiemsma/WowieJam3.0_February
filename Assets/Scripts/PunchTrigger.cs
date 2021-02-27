using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    public bool canHitPlayer;

    public bool isBlocking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(canHitPlayer);
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("i hit the enemy");
            canHitPlayer = true;
            //isBlocking = col.GetComponent<PlayerController.blocking>;
        } 
    }
    void OnTriggerExit2D(Collider2D col)
    {        
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("i hit the enemy");
            canHitPlayer = false;
        } 
    }
}
