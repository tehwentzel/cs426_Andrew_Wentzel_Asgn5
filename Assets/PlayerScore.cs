using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour
{
    // Start is called before the first frame update
    public TargetManager targetManager;
    public SyncListString addresses;

    [SyncVar]
    public int score = 0;
    public int maxScore;

    void Start()
    {
        //get the target manager.  assumes only one (todo: make it a singleton)
        targetManager =  (TargetManager) GameObject.FindObjectOfType(typeof(TargetManager));
        //get the shuffled addresses of the target manager
        addresses = targetManager.getAddresses();
        foreach(string a in addresses){
            Debug.Log(a);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void CmdScoreHit(){
        //increment the score if you hit the right target
        if(score < addresses.Count){
            score += 1;
        }
        if(score >= addresses.Count){
            targetManager.RpcWin();
        }
    }

    [Command]
    public void CmdInvertGravity(){
        targetManager.CmdInvertGravity();
    }

    public void checkTarget(string targetAddress){
        if(!isLocalPlayer)
            return;
        if(currentTargetAddress() == targetAddress){
            CmdScoreHit();
        } 
    }
    public string currentTargetAddress(){
        //check your current target based on the address list and current score
        if(score == addresses.Count){
            return "";
        }
        string currAddress = addresses[score];
        return currAddress;
    }

    private void OnCollisionEnter(Collision collision){
        if(!isLocalPlayer)
            return;

        if(collision.gameObject.tag == "AddressTarget"){
            //if we get hit by a player who is not supposed to hit us, invert gravity
            Target target = (Target) collision.gameObject.GetComponent(typeof(Target));
            if(!target.isActive)
                return;

            Debug.Log(target.getAddress());
            MeshRenderer targetMesh = collision.gameObject.GetComponent<MeshRenderer>();

            if(target.getAddress() == currentTargetAddress()){
                CmdScoreHit();
                targetMesh.material.color = Color.green;
            } else{
                CmdInvertGravity();
                targetMesh.material.color = Color.red;
            }
        }
        //on collision adding point to the score
    }

}
