using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class GlobalAnimationController : MonoBehaviour
    {

        [SerializeField]
        int animationID;

        public Animator playerAnimator, tonyAnimator, blakeAnimator, rileyAnimator;
        

        [YarnCommand ("PlayAnimation")]
        public void PlayAnimation(string characterName, int stateID)
        {            
            switch (characterName)
            {
                case "Player":
                    Debug.Log(playerAnimator.parameters[stateID - 1].name);
                    playerAnimator.SetTrigger(playerAnimator.parameters[stateID - 1].name);
                    break;

                case "Tony":
                    tonyAnimator.SetTrigger(tonyAnimator.parameters[stateID - 1].name);
                    break;

                case "Blake":
                    blakeAnimator.SetTrigger(blakeAnimator.parameters[stateID - 1].name);
                    break;
                case "Riley":
                    rileyAnimator.SetTrigger(rileyAnimator.parameters[stateID - 1].name);
                    break;
            }
        }

    }




}
