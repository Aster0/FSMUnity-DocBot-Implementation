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


      
            fsm.agent.SetDestination(fsm.BrokenBotLocation); // move to the broken bot's location to approach
            
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
         
        }

        public override void Update()
        {
            // do nothing on broken

            if (Vector3.Distance(fsm.transform.position, fsm.
                    BrokenBotLocation) <= 3) // if the tending bot is near the other bot's location already,
            {
                fsm.agent.isStopped = true; // we stop moving.


          
                
            }
        }


  

        public override void Exit()
        {
            base.Exit();

          
        }


    }

}