﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class tankShoot : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;

    /**
     * Fairly standard shooting script but updated to send network command data
     */

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
            //Ensures shooting is on all of the versions
            CmdShoot(); 
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 50;
        Destroy(bullet, 2.5f);
    }

    //create RPC- all RPC must be prefaced by Rpc  
    [ClientRpc]
    void RpcSpawnBullet()
    {
        //if you're not a server spawn a bullet (if you are the above will shoot)
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
