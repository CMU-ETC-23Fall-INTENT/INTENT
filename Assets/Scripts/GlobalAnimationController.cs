using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class GlobalAnimationController : MonoBehaviour
    {


        public Animator playerAlexAnimator, tonyAnimator, blakeAnimator, rileyAnimator;
        

        [YarnCommand ("PlayAnimation")]
        public void PlayAnimation(string characterName, int stateID)
        {            
            switch (characterName)
            {
                case "Alex":
                    playerAlexAnimator.SetTrigger(playerAlexAnimator.parameters[stateID - 1].name);
                    break;

                case "Tony":
                    tonyAnimator.SetTrigger(playerAlexAnimator.parameters[stateID - 1].name);
                    break;

                case "Blake":
                    blakeAnimator.SetTrigger(playerAlexAnimator.parameters[stateID - 1].name);
                    break;
                case "Riley":
                    rileyAnimator.SetTrigger(playerAlexAnimator.parameters[stateID - 1].name);
                    break;
            }
        }

    }




}
