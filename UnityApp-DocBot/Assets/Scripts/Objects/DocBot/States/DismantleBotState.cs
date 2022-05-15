using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class DismantleBotState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public DismantleBotState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.DISMANTLE_BOT + ": Dismantling a broken bot that can't be fixed.");

            
            fsm.UpdateDocBotText( GetTypeName().ToString());


            fsm.docBotDetails.docBotHardware.DismantleHardware(fsm); // dismantle the hardware that is still good and keep it.
            
            
            fsm.carryingBot = true;  // we will start carrying the bot.
            
            fsm.StartCoroutine(fsm.ChangeDelayedState("RETURN_RECYCLE", "DISMANTLE_BOT"));
            // delay a state change to return to recycle.)

          
            

        }

      

        public override void Update()
        {

          
            
        }

        public override void Exit()
        {
            base.Exit();
         
        }


    }

}