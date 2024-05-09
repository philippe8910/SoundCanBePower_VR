using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Windows.Speech;

public class NetworkSystem : MonoBehaviourPunCallbacks
{
    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "Link Start" , "Go" , "Link" , "Start" , "Rinku" , "Sutato" , "Rinku Sutato"};

    public GameObject environment , video;

    public VideoPlayer videoPlayer;

    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        videoPlayer.loopPointReached += delegate
        {
           StartRecording();
        };
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string keyword = args.text;
        Debug.Log("Detected keyword: " + keyword);
        if (keyword == "Link Start")
        {
            environment.SetActive(false);
            video.SetActive(true);
        }
    }

    [ContextMenu("StartRecording")]
    public void StartRecording()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("NetworkSystem");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //todo: create room

        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinRandomOrCreateRoom(null, 2);

        Debug.Log("OnJoinedLobby");        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("OnJoinedRoom");

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel("Level");
            Debug.Log("Load Game Scene");
        }   
    }
}


/*
1.數位平權、包容
2.

1.語音辨識:https://learn.microsoft.com/zh-tw/windows/mixed-reality/develop/unity/voice-input-in-unity
2.網路模組:https://doc.photonengine.com/zh-tw/pun/v2/getting-started/pun-intro
3.SteamVR:https://valvesoftware.github.io/steamvr_unity_plugin/
4.C#物驗導向程式設計:https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/object-oriented-programming
*/
