using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using Unity.VisualScripting;


public class LightSabarAction : MonoBehaviour
{
    //Dotween 

    public bool isEnable = true;

    public Vector3 oringinScale;

    void Start()
    {
        oringinScale = transform.localScale;
    }

    [ContextMenu("StartEnable")]
    public void StartEnable()
    {
        transform.localScale = new Vector3(oringinScale.x, 0, oringinScale.z);

        DOTween.To(() => transform.localScale, x => transform.localScale = x, oringinScale, 0.8f).OnComplete(() =>
        {
            isEnable = true;
        });
    }

    public void SabarBreak()
    {
        isEnable = false;
    }
}
