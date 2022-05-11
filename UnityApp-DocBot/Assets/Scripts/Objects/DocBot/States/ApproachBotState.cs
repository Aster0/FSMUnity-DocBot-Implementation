using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States.BrokenState // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ApproachState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ApproachState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.APPROACH_BOT + ": Approaches the location of an identified broken bot.");

      
            fsm.agent.SetDestination(fsm.BrokenBotLocation.transform.position); // move to the broken bot's location to approach
            
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
         
        }

        public override void Update()
        {
           

            if (Vector3.Distance(fsm.transform.position, fsm.
                    BrokenBotLocation.transform.position) <= 3) // if the tending bot is near the other bot's location already,
            {
                fsm.agent.isStopped = true; // we stop moving.


                fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.DIAGNOSE_BOT);
          
                
            }
        }


  

        public override void Exit()
        {
            base.Exit();

          
        }


    }

}