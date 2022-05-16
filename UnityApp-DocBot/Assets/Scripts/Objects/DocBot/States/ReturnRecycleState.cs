using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ReturnRecycleState : State<string> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ReturnRecycleState(DocBotFSM fsm, string typeName, GenericStateManager<string> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RETURN_RECYCLE + ": Returning to the recycling center.");

            
            fsm.UpdateDocBotText( GetTypeName().ToString());



            fsm.agent.isStopped = false; // make sure it can move.
            fsm.agent.SetDestination(fsm.recyclingTransform.transform.position);


        }

        public override void Update()
        {
            
            if (fsm.carryingBot) // if we are carrying broken bot
            {
                fsm.CarryTargetBot(); // carry the bot
            }
            

            if (Vector3.Distance(fsm.transform.position, fsm.recyclingTransform.position) < 3) // if its nearby the recycling area
            {
                // we can resupply.
                
                fsm.stateManager.ChangeState("RECYCLE_BOT");
            }
            
        }

        public override void Exit()
        {
            base.Exit();
         
        }


    }

}