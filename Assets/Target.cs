using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Target : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            Physics.gravity = -Physics.gravity;
        }
        //on collision adding point to the score
    }
}
