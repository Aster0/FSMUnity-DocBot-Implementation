using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ReturnSupplyState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ReturnSupplyState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RETURN_SUPPLY + ": Returning to the supply shack.");

            
            fsm.agent.isStopped = false; // let it move to the resupply zone.
            
            fsm.UpdateDocBotText( GetTypeName().ToString());

            fsm.agent.SetDestination(fsm.resupplyTransform.position); // move to the resupply area.


        }

        public override void Update()
        {
            
            if (Vector3.Distance(fsm.transform.position, fsm.resupplyTransform.position) < 3) // if its nearby 
            {
                // we can resupply.
                
                fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.RESUPPLY_MATERIALS);
            }
        }

        public override void Exit()
        {
            base.Exit();
      
        }


    }

}