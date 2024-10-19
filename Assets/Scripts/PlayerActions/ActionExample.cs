using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    //Inherit from PlayerAction
    public class ActionExample : PlayerAction
    {
        //This is the first function that will be called when the player starts the action
        public override void StartAction()
        {
            //Call the GameManager to disable the player movement
            GameManager.Instance.PlayerEnterAction();
        }
        
        //Reset action if needed
        public override void ResetAction(int state)
        {
            //Do something that will reset the action
        }

        //This is the function that will be called when the player finishes the action
        public override void PerformAction()
        {
            //Call the GameManager to enable the player movement
            GameManager.Instance.PlayerExitAction();


            //Call the SuccessFinishAction() function to finish the action
            //This is required to finish the action
            SuccessFinishAction();
        }
    }
}
