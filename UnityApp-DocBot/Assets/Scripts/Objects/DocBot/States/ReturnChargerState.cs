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
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
    
            if (fsm.BrokenBotLocation != null)
            {
                // was tending to a bot when it got broken,

                fsm.BrokenBotLocation.docBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
            }
            
            fsm.agent.SetDestination(fsm.chargingTransform.position); // move to the resupply area.
         
        }

        public override void Update()
        {

            if (Vector3.Distance(fsm.transform.position, fsm.chargingTransform.position) < 3) // if its nearby 
            {
                // we can charge.
                
                Debug.Log("REACH CHARGING");
                fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.CHARGE);
            }
        }

        public override void Exit()
        {
            base.Exit();
         
        }


    }

}