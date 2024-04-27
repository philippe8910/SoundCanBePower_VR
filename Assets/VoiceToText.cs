using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceToText : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "Link Start" , "Go" , "Link" , "Start"};

    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string keyword = args.text;
        Debug.Log("Detected keyword: " + keyword);
        if (keyword == "Link Start")
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        
    }
}
