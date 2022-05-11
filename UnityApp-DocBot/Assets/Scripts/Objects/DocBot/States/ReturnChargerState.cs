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
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RETURN_CHARGE + ": Returning to the charger pad's location.");


            fsm.agent.isStopped = false; // make sure it can move
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
    
            fsm.RemoveBrokenBot();
            
            fsm.agent.SetDestination(fsm.chargingTransform.position); // move to the resupply area.

            if (fsm.carryingBot)
            {
                // if its carrying a bot, meaning we are planning to put this bot at the charging station,
                // and it's actually already repaired so just put it to alive.
                DocBotsManager.Instance.docBotsAlive += 1; // plus one to the total alive as the docbot 
 
            }
         
        }

        public override void Update()
        {

            if (fsm.carryingBot) // if we are carrying broken bot
            {

                if (fsm.BrokenBotLocation.docBotDetails.docBotHardware.totalFailure) // if its total failure broken bot
                {
                    // we are actually meant to charge ourselves and not the bot, its just a coincidence
                    // that we're otw to the recycling center and this bot ran out of battery.
                    // so we shouldn't bring the broken bot to the charging station with us.
                    fsm.carryingBot = false;
                    fsm.RemoveBrokenBot();
                }
                
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