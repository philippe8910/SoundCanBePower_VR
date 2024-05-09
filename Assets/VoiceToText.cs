using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceToText : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "Fireball" , "Fire" , "Ball" , "Fire Ball" , "Fireball" , "Fire"};

    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;

    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        fireballSpawnPoint = GameObject.FindWithTag("LeftHand").transform;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string keyword = args.text;
        Debug.Log("Detected keyword: " + keyword);
        if (keyword == "Fireball")
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        var fireball = PhotonNetwork.Instantiate("fireballPrefab", fireballSpawnPoint.position, fireballSpawnPoint.rotation);
        //PhotonNetwork.Destroy(fireball);
    }
}
