using Photon.Pun;
using UnityEngine;

public class VRBodySetting : MonoBehaviour
{
    [SerializeField] private bool isComputer;
    public PhotonView photonView;
    public GameObject avatar , playerPos;
    public Transform HMD, headModelPos;
    public Transform rightHandControllerPos, rightHandModelPos;
    public Transform leftHandControllerPos, leftHandModelPos;
    
    private void Start()
    {
        avatar = transform.GetChild(0).gameObject;
        if (photonView.IsMine)
        {
            avatar.SetActive(false);
            playerPos = GameObject.Find("Player");
            if (!isComputer)
            {
                rightHandControllerPos = GameObject.Find("RightHand").transform;
                leftHandControllerPos = GameObject.Find("LeftHand").transform;   
            }
        }
    }

    private void Update()
    {
        if(playerPos != null && photonView.IsMine)
            transform.position = playerPos.transform.position;

        if (!photonView.IsMine || isComputer || leftHandControllerPos || rightHandControllerPos == null)
            return;

        rightHandModelPos.position = rightHandControllerPos.position;
        rightHandModelPos.rotation = rightHandControllerPos.rotation;

        leftHandModelPos.position = leftHandControllerPos.position;
        leftHandModelPos.rotation = leftHandControllerPos.rotation;
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
    }
}
