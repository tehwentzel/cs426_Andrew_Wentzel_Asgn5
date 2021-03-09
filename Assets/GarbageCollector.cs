using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GarbageCollector : NetworkBehaviour
{
    Rigidbody rb;
    Transform t;
    TargetManager targetManager;
    AudioSource laserAudio;
    Light flashLight;
    bool gameFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        laserAudio = GetComponent<AudioSource>();
        flashLight = GetComponentInChildren<Light>();
        resetSelf();
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

        attackPlayer();
    }

    void attackPlayer(){
        PlayerScore[] players = (PlayerScore[]) (GameObject.FindObjectsOfType(typeof(PlayerScore)));
        if(players.Length < 1){
            Debug.Log("No players");
        }
        PlayerScore lowestPlayer = null;
        float highestDownTime = 3f; //start at minimum time
        foreach(var player in players){
            if(player.timeSinceLastScore > highestDownTime){
                lowestPlayer = player;
                highestDownTime = player.timeSinceLastScore;
            }
        }
        if(lowestPlayer != null){
            moveToPlayer(lowestPlayer);
        } else{
            resetSelf();
        }
    }

    void moveToPlayer(PlayerScore player){
        flashLight.enabled = true;
        t.LookAt(player.transform);
        GetComponent<MeshRenderer>().material.color = Color.red;
        Vector3 positionOffset = t.position - player.transform.position;
        rb.velocity = -2f*positionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void resetSelf(){
        rb.velocity = new Vector3(0,0,0);
        rb.velocity *= 0f;
        GetComponent<MeshRenderer>().material.color = Color.black;
        flashLight.enabled = false;
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player"){
            if(collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer){
                Debug.Log("collision with garbage and player");
                Rigidbody player = collision.gameObject.GetComponent<Rigidbody>();
                float force = -10000f;
                Vector3 collisionDir = transform.position - collision.contacts[0].point;
                Vector3 pushBack = (force)*(collisionDir.normalized);
                player.AddForce(pushBack);
                collision.gameObject.GetComponent<PlayerScore>().resetDownTime();
                laserAudio.PlayOneShot(laserAudio.clip);
                resetSelf();
            } else{
                Debug.Log("collision not local");
            }
            
        }
    }
}