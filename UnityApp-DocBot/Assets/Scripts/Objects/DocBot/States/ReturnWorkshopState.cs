using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ReturnWorkshopState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ReturnWorkshopState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RETURN_WORKSHOP + ": Returning to the workshop's location.");


            fsm.agent.isStopped = false; // make sure it can move
            
            fsm.UpdateDocBotText( GetTypeName().ToString());

            if (fsm.docBotDetails.docBotHardware.durability <= fsm.docBotDetails.docBotHardware.durabilityToRepairAt) // need to repair itself
            {
                fsm.StopCarryingBot();  // so stop carrying bot if it is carrying
                
                if(fsm.BrokenBotLocation != null) // if we are tending to a broken bot
                    fsm.BrokenBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
                Debug.Log("NOT TEND");
            }
            
            fsm.agent.SetDestination(fsm.workshopTransform.position); // move to the resupply area.

            if (fsm.carryingBot)
            {
                // if its carrying a bot, meaning we are planning to put this bot at the workshop,
                // and it's actually already repaired so just put it to alive.
                DocBotsManager.Instance.docBotsAlive += 1; // plus one to the total alive as the docbot 
 
            }
         
        }

        public override void Update()
        {

            if (fsm.carryingBot) // if we are carrying broken bot
            {

                if (fsm.BrokenBotDetails.docBotHardware.totalFailure) // if its total failure broken bot
                {
                    // we are actually meant to repair ourselves and not the bot, its just a coincidence
                    // that we're otw to the recycling center and this bot needs maintenance at the workshop.
                    // so we shouldn't bring the broken bot to the workshop with us.
                    fsm.carryingBot = false;
                    fsm.RemoveBrokenBot();
                }
                
                fsm.CarryTargetBot(); // carry the bot
            }

            if (Vector3.Distance(fsm.transform.position, fsm.workshopTransform.position) < 3) // if its nearby 
            {
                // we can charge.

                if (fsm.carryingBot) // if we are carrying broken bot
                {
                    
              
                    fsm.BrokenBotLocation.ChangeState("MACHINE_REPAIR");
                    // we change the broken bot's state to machine_repair so it gets repaired by the machines.
                    
                    
                    fsm.stateManager.ChangeState("PLACE_BOT");
                    // then we can  place the bot down..
                    
                    
                    
                }
                else // if we aren't carrying a broken bot, we can charge ourselves (as per FSM)
                {
                   
                    fsm.stateManager.ChangeState("MACHINE_REPAIR");
                }
              
            }
        }

        public override void Exit()
        {
            base.Exit();
         
        }


    }

}