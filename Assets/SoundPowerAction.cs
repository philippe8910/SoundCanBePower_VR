using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Valve.VR;
using VolumetricLines;

public class SoundPowerAction : MonoBehaviour
{
    public PhotonView photonView { get; private set; }

    public TMP_Text loudnessText;

    public GameObject swordObject, swordShadowObject;
    public GameObject cellPrefab;
    public GameObject effect, fireEffect;
    public GameObject rebornEffect;

    public VolumetricLineBehavior volumetricLineBehavior;
    public BoxCollider boxCollider;
    public MeshRenderer meshRenderer;
    public LightSabarAction lightSabarAction;
    public TrailRenderer trailRenderer;

    public float loudness = 100f; // 灵敏度
    public float[] loudnessThreshold;

    public SwordActor swordActor;

    private string microphoneName;
    private AudioClip audioClip;

    private AudioSource audioSource;

    public static SoundPowerAction Instance;

    private void Awake()
    {
        Instance = GetComponent<SoundPowerAction>();
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        boxCollider = GetComponent<BoxCollider>();
       // audioSource = GetComponent<AudioSource>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        volumetricLineBehavior.LineColor = Color.blue;

        // 获取麦克风设备名称
        microphoneName = Microphone.devices.Length > 0 ? Microphone.devices[0] : "";
        Debug.Log("Microphone name: " + microphoneName);

        // 请求麦克风权限
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            StartMicrophone();
        else
            StartCoroutine(RequestMicrophonePermission());
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            loudness = GetMicrophoneLoudness();
        }
        

        if (loudness > loudnessThreshold[0] && loudness < loudnessThreshold[1])
            HandleSound(10, 0.01f, 0.25f, false, Color.blue);
        else if (loudness > loudnessThreshold[1] && loudness < loudnessThreshold[2])
            HandleSound(0, 0.02f, 0.2f, false, Color.red);
        else if (loudness > loudnessThreshold[2])
            HandleSound(5, 0.025f, 0, true, Color.green);
        else if (loudness < loudnessThreshold[0])
            HandleSound(0, 0, 0, false, Color.white);
            

        swordActor.effectValue = loudness;
        loudnessText.text = "Loudness: " + loudness;
//        audioSource.clip = audioClip;
       // Debug.Log("Loudness: " + loudness);
    }

    private IEnumerator RequestMicrophonePermission()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            StartMicrophone();
        else
            Debug.LogError("Microphone permission denied!");
    }

    private void StartMicrophone()
    {
        audioClip = Microphone.Start(microphoneName, true, 1, AudioSettings.outputSampleRate);
    }

    private float GetMicrophoneLoudness()
    {
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        float sum = 0;
        foreach (float sample in samples)
            sum += Mathf.Abs(sample);

        float rms = Mathf.Sqrt(sum / samples.Length);
        return rms * 100;
    }

    private void HandleSound(float shakeSpeedOffset, float shakeAmountFactor, float materialEmission, bool activateFireEffect, Color material)
    {
        swordActor.enabled = true;
        swordActor.shakeSpeed = loudness + shakeSpeedOffset;
        swordActor.shakeAmount = loudness * shakeAmountFactor;
        volumetricLineBehavior.LineColor = material;

        trailRenderer.material.SetColor("_EmissionColor", material * materialEmission);

        if (activateFireEffect)
            fireEffect.SetActive(true);
        else
            fireEffect.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Sword"))
        {
            GameObject cells = Instantiate(cellPrefab, swordObject.transform.position, swordObject.transform.rotation);
            GameObject effectObject = Instantiate(effect, swordObject.transform.position, swordObject.transform.rotation);

            swordObject.SetActive(false);
            swordShadowObject.SetActive(true);
            boxCollider.enabled = false;


            Destroy(effectObject, 1f);

            if (collision.transform.TryGetComponent<SoundPowerAction>(out var _swordAction))
            {
                float waitForSeconds = 1.5f - Mathf.Clamp((loudness - _swordAction.loudness) / 10f, -100, 100);
                StartCoroutine(StartCountdown(waitForSeconds , cells));
            }
            else
            {
                float waitForSeconds = 1.5f - Mathf.Clamp((loudness - 0.5f) / 10f, -100, 100);
                StartCoroutine(StartCountdown(Mathf.Clamp(waitForSeconds, 1f, 100) , cells));
                Debug.Log("No SoundPowerAction");
                Debug.Log("waitForSeconds: " + waitForSeconds);
            }
        }
    }

    private IEnumerator StartCountdown(float waitForSeconds , GameObject cells = null)
    {
        yield return new WaitForSeconds(waitForSeconds);
        swordObject.SetActive(true);
        swordShadowObject.SetActive(false);
        boxCollider.enabled = true;

        lightSabarAction.StartEnable();

        //GameObject rebornEffectObject = Instantiate(rebornEffect, swordObject.transform.position, swordObject.transform.rotation);
        //Destroy(rebornEffectObject, 1f);
        Destroy(cells, 3f);
    }
}
