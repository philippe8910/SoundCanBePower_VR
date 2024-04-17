using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(PhotonView), typeof(Throwable) , typeof(PhotonTransformView))]
public class Grabbable : MonoBehaviour
{
    private Throwable throwable { get => GetComponent<Throwable>();}
    private Rigidbody rb { get => GetComponent<Rigidbody>();}
    private PhotonView photonView { get => GetComponent<PhotonView>();}

    void Start()
    {
        throwable.onPickUp.AddListener(delegate {
            photonView.RequestOwnership();
            photonView.RPC("CancelGravityRPC", RpcTarget.All, true);
        });

        throwable.onDetachFromHand.AddListener(delegate {
            photonView.RPC("CancelGravityRPC", RpcTarget.All, false);
        });
    }

    [PunRPC]
    public void CancelGravityRPC(bool cancelGravity)
    {
        //todo sync transform has problem
        rb.useGravity = !cancelGravity;
        Debug.Log("Gravity is " + (cancelGravity ? "canceled" : "enabled"));
    }
}
