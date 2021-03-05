using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Wire : NetworkBehaviour
{

    Rigidbody rb;
    Transform t;
    TargetManager targetManager;
    AudioSource buzzAudio;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        buzzAudio = GetComponent<AudioSource>();
    }

    void applyGravity(){
        var multiplier = 50;
        if(targetManager != null){  
            rb.AddForce(multiplier*targetManager.getGravity());
        } else{
            rb.AddForce(new Vector3(0,-multiplier*10,0));
        }
    }

    float gravityDirection(){
        return targetManager.getGravity().normalized.y;
    }

    void FixedUpdate(){
        targetManager = (TargetManager) GameObject.FindObjectOfType(typeof(TargetManager));
        applyGravity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player"){
            if(!collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer){ return; }
            if(buzzAudio != null){
                buzzAudio.PlayOneShot(buzzAudio.clip);
            }
        }
    }
}
