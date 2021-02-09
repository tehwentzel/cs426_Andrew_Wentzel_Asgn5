using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TargetManager : NetworkBehaviour
{
    public Target[] targets;
    public SyncListString addresses = new SyncListString();
    public GameObject winObject;

    [SyncVar]
    public float gravityY = -10.0f;
    private float maxScore = 4; //dont include some of the targets
    // Start is called before the first frame update

    //This should probably be made into a singleton pattern is I use it for more than like 2 seconds of gameplay
    void Start()
    {
        //finds all objects with the Target script
        if(isServer){
            targets = (Target[]) (GameObject.FindObjectsOfType(typeof(Target)));

            //use these premade addresses, shuffles them, as assigns them to targets
            List<string> baseAddresses = new List<string> {"0xa1b2c3d4", "0x3A28214A","0x6339392C","0x7363682E","CPU","0x0000000","USB"};
            //this will be the end order that the player needs to hit the targets in
            Shuffle(baseAddresses);
            int currAddressIdx = 0;
            foreach(var target in targets){
                string currAddress = baseAddresses[currAddressIdx];
                target.setAddress(baseAddresses[currAddressIdx]);
                if(currAddressIdx < maxScore){
                    addresses.Add(currAddress);
                }
                currAddressIdx += 1;
            }
        }
        postAddresses();
    }

    [Command]
    public void CmdInvertGravity(){
        gravityY = -1*gravityY;
        targets = (Target[]) (GameObject.FindObjectsOfType(typeof(Target)));
        foreach(Target target in targets){
            target.invertGravityDirection();
        }
        //Todo: move up/down billboard text
        StartCoroutine(MoveBillBoard());
    }

    IEnumerator MoveBillBoard(){
        int i = 0;
        while(i < 2){
            if(gravityY > 0)
                GameObject.Find("Billboard").gameObject.transform.Translate(new Vector3(0, 7/2, 0));
            else
                GameObject.Find("Billboard").gameObject.transform.Translate(new Vector3(0, -7/2, 0));
            i++;
            yield return new WaitForSeconds(1);
        }
    }

    public Vector3 getGravity(){
        return new Vector3(0, gravityY, 0);
    }

    void postAddresses(){
        string billBoardText = "Instructions:\n";
        foreach(string addr in addresses){
            billBoardText += "goto ";
            billBoardText += addr;
            billBoardText += "\n";
        }
        GameObject billBoard = GameObject.Find("BillboardText").gameObject;
        billBoard.GetComponent<Text>().text = billBoardText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SyncListString getAddresses(){
        //used to send addresses to the players
        return addresses;
    }

    [ClientRpc]
    public void RpcWin(){
        //todo: do stuff in a win event here
        Debug.Log("Game over");
        foreach(var target in targets){
            // target.setInactive();
            Instantiate(winObject, target.transform.position, target.transform.rotation);
            Destroy(target.gameObject);
        }
        GameObject billBoard = GameObject.Find("BillboardText").gameObject;
        billBoard.GetComponent<Text>().text = "Game over";
    }

    [ClientRpc]
    public void RpcUpdatePoints(int clientID, int playerScore){
        Text playerText;
        if (clientID == 0){
            Debug.Log("This is client " + clientID);
            playerText = GameObject.Find("ScoreCanvas/Player 1").GetComponent("Text") as Text;
            playerText.GetComponent<UnityEngine.UI.Text>().text = "Player 1: " + playerScore.ToString();

        }
        else if (clientID == 1){
            Debug.Log("This is client " + clientID);
            playerText = GameObject.Find("ScoreCanvas/Player 2").GetComponent("Text") as Text;
            playerText.GetComponent<UnityEngine.UI.Text>().text = "Player 2: " + playerScore.ToString();

        }
        else if (clientID == 2){
            Debug.Log("This is client " + clientID);
            playerText = GameObject.Find("ScoreCanvas/Player 3").GetComponent("Text") as Text;
            playerText.GetComponent<UnityEngine.UI.Text>().text = "Player 3: " + playerScore.ToString();

        }
        else{
            Debug.Log("This is client " + clientID);
            playerText = GameObject.Find("ScoreCanvas/Player 4").GetComponent("Text") as Text;
            playerText.GetComponent<UnityEngine.UI.Text>().text = "Player 4: " + playerScore.ToString();
        }
    }

    void Shuffle (List<string> deck) {
        for (int i = 0; i < deck.Count; i++) {
            string temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

}
