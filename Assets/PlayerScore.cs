using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerScore : ManagedBehaviour
{
    // Start is called before the first frame update
    public SyncListString addresses;

    [SyncVar]
    public int score = 0;
    public int maxScore;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStartClient(){
        StartCoroutine(WaitForTargetManager());
    }

    private IEnumerator WaitForTargetManager(){
        while(GameObject.FindObjectOfType(typeof(TargetManager)) == null){
            yield return null;
        }
        targetManager = (TargetManager) GameObject.FindObjectOfType(typeof(TargetManager));
        addresses = targetManager.getAddresses();
        isLoaded = true;
    }

    [Command]
    public void CmdScoreHit(){
        //increment the score if you hit the right target
        if(!isLoaded)
            return;

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
        if(!isLocalPlayer || !isLoaded)
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
        if(!isLocalPlayer || !isLoaded)
            return;

        if(collision.gameObject.tag == "AddressTarget"){
            //if we get hit by a player who is not supposed to hit us, invert gravity
            Target target = (Target) collision.gameObject.GetComponent(typeof(Target));
            if(!target.isActive)
                return;

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
