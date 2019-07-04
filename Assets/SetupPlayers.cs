using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//import mirror- NOT unity networking
using Mirror;

//inherit from networkbehaviour
public class SetupPlayers : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = !isLocalPlayer;
        //uses the network behaviour property "islocalPlayer"
        if (isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = true;
            CameraFollow.player = this.gameObject.transform;
        }
        else
        {
            GetComponent<PlayerController>().enabled = false;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
