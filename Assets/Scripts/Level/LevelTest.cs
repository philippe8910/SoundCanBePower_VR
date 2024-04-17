using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(PhotonView))]
public class LevelTest : MonoBehaviour, IGameTrigger 
{
    [SerializeField] public float timeCount {get; set;}
    [SerializeField] public int scoreCount {get; set;}
    [SerializeField] public bool isGameStart {get; set;}

    public Text timeText;
    public Text scoreText;
    public PhotonView photonView {get => GetComponent<PhotonView>();}
    public TeleportPoint teleportPoint {get; set;}

    [PunRPC]
    public void OnGameStart()
    {
        timeCount = 60;
        scoreCount = 0;

        isGameStart = true;

        Debug.Log("Game Start");
    }

    [PunRPC]
    public void OnGameTimeUp()
    {
        timeCount = 0;
        scoreCount = 0;

        isGameStart = false;

        Debug.Log("Game Over");
    }

    // Start is called before the first frame update
    void Start()
    {
        teleportPoint.OnTeleport.AddListener(delegate {
            photonView.RPC("OnGameStart", RpcTarget.All);
        });
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");

        if(isGameStart)
        {
            timeCount -= Time.deltaTime;
            timeText.text = timeCount.ToString("F0");

            if(timeCount <= 0)
            {
                photonView.RPC("OnGameTimeUp", RpcTarget.All);
            }

            //Debug.Log("Time Count: " + timeCount);
        }
    }

    [ContextMenu("GameStartRPC")]
    public void GameStartRPC()
    {
        photonView.RPC("OnGameStart", RpcTarget.All);
        Debug.Log("GameStartRPC");
    }

    [ContextMenu("GameTimeUpRPC")]
    public void GameTimeUpRPC()
    {
        photonView.RPC("OnGameTimeUp", RpcTarget.All);
        Debug.Log("GameTimeUpRPC");
    }
}
