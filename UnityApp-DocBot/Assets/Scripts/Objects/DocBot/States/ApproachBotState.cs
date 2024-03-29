using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ApproachState : State<string> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ApproachState(DocBotFSM fsm, string typeName, GenericStateManager<string> stateManager) : base(stateManager, typeName) 
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
           

            
            RaycastHit hitInfo;
       
            
      
            Collider[] colliders = Physics.OverlapSphere(fsm.transform.position, 3); // check if its nearby
            // raycast a sphere around a detection range and get an array of colliders

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == fsm.BrokenBotLocation.gameObject) // if the tending bot is near the other bot's location already,
                {
                    fsm.agent.isStopped = true; // we stop moving.


                    fsm.stateManager.ChangeState("DIAGNOSE_BOT");


                    break; // found the target, break out of iteration to save memory.
                }
            }

     
     
            
            
        }


  

        public override void Exit()
        {
            base.Exit();

          
        }


    }

}