using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordActor : MonoBehaviour
{
    public float shakeSpeed = 1.0f; // 震動速度
    public float shakeAmount = 0.1f; // 震動幅度
    public float effectValue = 0.0f; // 震動持續時間
    
    public Renderer renderer; // 參考你想要調整的物體的 Renderer 組件

    private Vector3 startPosition; // 初始位置

    void Start()
    {
        startPosition = transform.localPosition; // 保存初始位置
        renderer = GetComponent<Renderer>(); // 獲取 Renderer 組件
    }

    void Update()
    {
        // 生成一個隨機的震動偏移量
        Vector3 offset = new Vector3(
            Mathf.PerlinNoise(Time.time * shakeSpeed, 0) - 0.5f,
            Mathf.PerlinNoise(0, Time.time * shakeSpeed) - 0.5f,
            Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f
        ) * shakeAmount;

        // 設置物體的局部位置，加上震動偏移量
        transform.localPosition = startPosition + offset;

        //AdjustEmissionIntensity((effectValue / 10) - 5); // 調整 Emission 強度
    } 

     // 調整 Emission 屬性
    void AdjustEmissionIntensity(float intensity)
    {
        Material material = renderer.material;
        Color color = material.GetColor("_EmissiveColor");
        material.SetColor("_EmissiveColor", color * intensity);
    }

}
