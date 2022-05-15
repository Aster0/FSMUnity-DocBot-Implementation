using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ReturnBotLocationState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ReturnBotLocationState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RETURN_BOT_LOCATION + ": Returning to broken bot's location.");

            
            fsm.UpdateDocBotText( GetTypeName().ToString());

            fsm.agent.SetDestination(fsm.BrokenBotLocation.transform.position);


        }

        public override void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(fsm.transform.position, 3); // check if its nearby

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject ==
                    fsm.BrokenBotLocation.gameObject) // if the tending bot is near the other bot's location already,
                {
                    fsm.agent.isStopped = true; // we stop moving.


                    // not the same as approach as this needs to become REPAIR_BOT as it has already diagnosed.
                
                    // just needed to resupply and continue repairing after.
                    fsm.stateManager.ChangeState("REPAIR_BOT");
                }
            }
            

        }

        public override void Exit()
        {
            base.Exit();
      
        }


    }

}