using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class tankShoot : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = !isLocalPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdShoot();
        
        }
    }

    void SpawnBullet()
    {

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 100;
        Destroy(bullet, 2.5f);
    }

    //create RPC- all RPC muse be prefaced by Rpc  
    [ClientRpc]
    void RpcSpawnBullet()
    {
        //if you're not a server spawn a bullet (if you are the below will shoot)
        if (!isServer)
            SpawnBullet();
    }

    //set it as a command to send across the network- Cmd must preface all function names
    [Command]
    void CmdShoot()
    {
        SpawnBullet();
        RpcSpawnBullet();
    }
}
