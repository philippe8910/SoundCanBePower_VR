using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayerPrefabSpawner : MonoBehaviourPunCallbacks
{
    public GameObject spawnPlayer = null , spawnPlayerSword = null;

    public GameObject playerPos;

    public Transform player_1 , player_2;

    private void Start()
    {
        //todo this is not good to fix lag, 
        PhotonNetwork.SendRate = 90; //Default is 30
        PhotonNetwork.SerializationRate = 60; //5 is really laggy, jumpy. Default is 10?


        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("Load Menu Scene");
        }


        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && playerPos != null && spawnPlayer == null)
        {
            SpawnPlayer();
            Debug.Log("NetworkPlayerPrefabSpawner");
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            GameObject.FindWithTag("Player").transform.position = player_1.position;
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GameObject.FindWithTag("Player").transform.position = player_2.position;
        }

       // GameObject.Find("Player").transform.position = new Vector3(0, 0, 0);
    }

    private void SpawnPlayer()
    {
        spawnPlayer = PhotonNetwork.Instantiate("NetworkPlayer", playerPos.transform.position, playerPos.transform.rotation);
        spawnPlayerSword = PhotonNetwork.Instantiate("Orc_Sword", playerPos.transform.position, playerPos.transform.rotation);

        if (spawnPlayer == null)
        {
            Debug.LogError("Failed to spawn player");
        }
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();

        PhotonNetwork.Destroy(spawnPlayer);
        PhotonNetwork.Destroy(spawnPlayerSword);
    }
}
