using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SwordAction : MonoBehaviour
{
    public PhotonView photonView { get; private set;}

    public GameObject objectToMove; // 要移动的物体
    public Transform positionObject; // 记录运动轨迹的位置物体

    public List<Vector3> positions = new List<Vector3>(); // 保存位置的数组
    public List<Quaternion> rotations = new List<Quaternion>(); // 保存旋转值的数组

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        positionObject = GameObject.FindWithTag("SwordPoint").transform;
    }

    void Update()
    {
        if(!photonView.IsMine)
            return;

        positions.Add(positionObject.position);
        rotations.Add(positionObject.rotation);

        objectToMove.transform.position = Vector3.Lerp(transform.position, positions[0], 0.05f);
        objectToMove.transform.rotation = Quaternion.Lerp(transform.rotation, rotations[0], 0.05f);

        positions.RemoveAt(0);
        rotations.RemoveAt(0);
    }
}
