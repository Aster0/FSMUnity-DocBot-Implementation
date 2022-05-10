using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ReturnChargerState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ReturnChargerState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            fsm.agent.isStopped = false; // make sure it can move
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
    
            if (fsm.BrokenBotLocation != null && !fsm.carryingBot) // also make sure we're not carrying a bot, if so, we're actually still tending.
            {
                // was tending to a bot when it needs charging,

                fsm.BrokenBotLocation.docBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
            }
            
            fsm.agent.SetDestination(fsm.chargingTransform.position); // move to the resupply area.
         
        }

        public override void Update()
        {

            if (fsm.carryingBot) // if we are carrying broken bot
            {
                fsm.CarryTargetBot(); // carry the bot
            }

            if (Vector3.Distance(fsm.transform.position, fsm.chargingTransform.position) < 3) // if its nearby 
            {
                // we can charge.

                if (fsm.carryingBot) // if we are carrying broken bot
                {
                    
                    Debug.Log("Broken bot is charging now.");
                    fsm.BrokenBotLocation.stateManager.ChangeState(DocBotFSM.DocBotTypes.CHARGE);
                    // we change the broken bot's state to charging so it gets charged.
                    
                    
                    fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.PLACE_BOT);
                    // then we can  place the bot down..
                    
                    
                    
                }
                else // if we aren't carrying a broken bot, we can charge ourselves (as per FSM)
                {
                    Debug.Log("REACH CHARGING");
                    fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.CHARGE);
                }
              
            }
        }

        public override void Exit()
        {
            base.Exit();
         
        }


    }

}