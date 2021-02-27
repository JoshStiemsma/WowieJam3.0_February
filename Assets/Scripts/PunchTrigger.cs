using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    public bool canHitPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canHitPlayer);
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("i hit the enemy");
            canHitPlayer = true;
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
