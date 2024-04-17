using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;

public class SoundPowerAction : MonoBehaviour
{
    public GameObject swordObject , swordShadowObject;
    public GameObject cellPrefab;
    public GameObject effect;
    public GameObject rebornEffect;


    public BoxCollider boxCollider;


    public float loudness = 100f; // 灵敏度
    public float[] loudnessThreshold; 

    public SwordActor swordActor;

    private string microphoneName;
    private AudioClip audioClip;


    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        // 获取麦克风设备名称
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            Debug.Log("Microphone name: " + microphoneName);
        }
        else
        {
            Debug.LogError("No microphone found!");
        }

        // 请求麦克风权限
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            StartMicrophone();
        }
        else
        {
            StartCoroutine(RequestMicrophonePermission());
        }
    }

    void Update()
    {
        // 计算音量
        loudness = GetMicrophoneLoudness();
        
        // 判断是否在喊叫
        if(loudness > loudnessThreshold[0])
        {
            swordActor.enabled = true;
            swordActor.shakeSpeed = loudness - 10;
            swordActor.shakeAmount = loudness * 0.01f - 0.25f;
        }
        else if(loudness > loudnessThreshold[1])
        {
            swordActor.enabled = true;
            swordActor.shakeSpeed = loudness;
            swordActor.shakeAmount = loudness * 0.02f - 0.2f;
        }
        else if(loudness > loudnessThreshold[2])
        {
            swordActor.enabled = true;
            swordActor.shakeSpeed = loudness + 20;
            swordActor.shakeAmount = loudness * 0.03f;
        }
        else if(loudness < loudnessThreshold[0])
        {
            swordActor.enabled = true;
            swordActor.shakeSpeed = Mathf.Clamp((loudness - 10) , 0 , 100);
            swordActor.shakeAmount = Mathf.Clamp(loudness * 0.01f - 0.1f , 0 , 100);

        }

        swordActor.effectValue = loudness;
        Debug.Log("Loudness: " + loudness);
    }

    IEnumerator RequestMicrophonePermission()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            StartMicrophone();
        }
        else
        {
            Debug.LogError("Microphone permission denied!");
        }
    }

    void StartMicrophone()
    {
        audioClip = Microphone.Start(microphoneName, true, 1, AudioSettings.outputSampleRate);
    }

    float GetMicrophoneLoudness()
    {
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);
        
        float sum = 0;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }
        
        float rms = Mathf.Sqrt(sum / samples.Length);

        return rms * 100;

    }



    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Sword")
        {
            GameObject cells = Instantiate(cellPrefab, swordObject.transform.position, swordObject.transform.rotation);
            GameObject effectObject = Instantiate(effect, swordObject.transform.position, swordObject.transform.rotation);
            swordObject.SetActive(false);
            swordShadowObject.SetActive(true);
            boxCollider.enabled = false;

            Destroy(effectObject, 1f);

            if(collision.transform.GetComponent<SoundPowerAction>())
            {
                var _swordAction = collision.transform.GetComponent<SoundPowerAction>();

                float waitForSeconds = 1.5f - Mathf.Clamp((loudness - _swordAction.loudness) / 10f , -100 , 100);
                StartCoroutine(startCountdown(Mathf.Clamp(waitForSeconds , 0.001f , 100)));
            }
            else
            {
                float waitForSeconds = 1.5f - Mathf.Clamp((loudness - 50) / 10f , -100 , 100);

                StartCoroutine(startCountdown(Mathf.Clamp(waitForSeconds , 0.001f , 100)));
                Debug.Log("No SoundPowerAction");
                Debug.Log("waitForSeconds: " + waitForSeconds);
            }

            IEnumerator startCountdown(float waitForSeconds)
            {
                yield return new WaitForSeconds(waitForSeconds);
                swordObject.SetActive(true);
                swordShadowObject.SetActive(false);
                boxCollider.enabled = true;

                GameObject rebornEffectObject = Instantiate(rebornEffect, swordObject.transform.position, swordObject.transform.rotation);
                Destroy(rebornEffectObject, 1f);
                Destroy(cells, 3f);
            }
        }
    }

}
