using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView)) , RequireComponent(typeof(PhotonTransformView)) , RequireComponent(typeof(PhotonRigidbodyView))]
public class PhotonViewObject : MonoBehaviour
{
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        photonView.OwnershipTransfer = OwnershipOption.Takeover;
        StartCoroutine(DelayDelect());

        IEnumerator DelayDelect()
        {
            yield return new WaitForSeconds(5);
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
