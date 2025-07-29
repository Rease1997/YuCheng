using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;
        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if(Vector2.Distance(virtualMoveDirection, Vector2.zero) > starterAssetsInputs.moveValue)
            {
                starterAssetsInputs.sprint = true;
            }
            else
            {
                starterAssetsInputs.sprint = false;
            }
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        
    }

}
