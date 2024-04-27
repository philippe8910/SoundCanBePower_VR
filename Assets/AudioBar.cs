using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBar : MonoBehaviour
{
    public Vector3 targetScale;
    public float maxScale = 100f;

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale / 100 * SoundPowerAction.Instance.loudness , 0.9f);
    }
}
