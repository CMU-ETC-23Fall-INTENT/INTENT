using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class GlobalAnimationController : MonoBehaviour
    {


        public Animator playerAnimator, tonyAnimator, aliAnimator, mingAnimator, ashAnimator;
        

        [YarnCommand ("PlayAnimation")]
        public void PlayAnimation(string characterName, int stateID)
        {            
            switch (characterName)
            {
                case "Player":
                    playerAnimator.SetTrigger(playerAnimator.parameters[stateID - 1].name);
                    break;

                case "Tony":
                    tonyAnimator.SetTrigger(tonyAnimator.parameters[stateID - 1].name);
                    break;

                case "Ali":
                    aliAnimator.SetTrigger(aliAnimator.parameters[stateID - 1].name);
                    break;
                case "Ming":
                    mingAnimator.SetTrigger(mingAnimator.parameters[stateID - 1].name);
                    break;
                case "Ash":
                    ashAnimator.SetTrigger(ashAnimator.parameters[stateID - 1].name);
                    break;
            }
        }

    }




}
