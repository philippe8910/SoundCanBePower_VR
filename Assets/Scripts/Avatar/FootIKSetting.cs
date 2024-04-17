using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKSetting : MonoBehaviour
{
    [SerializeField] private float rightIKWeight = 1;
    [SerializeField] private float leftIKWeight = 1;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        var rightIKPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        var leftIKPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);

        RaycastHit hit;

        var hasHit = Physics.Raycast(rightIKPos + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot , rightIKWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot , hit.point);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot , 0);
        }

        hasHit = Physics.Raycast(leftIKPos + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot , leftIKWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot , hit.point);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot , 0);
        }
        
        
    }
}
