using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class RepairBotState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public RepairBotState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

      
            fsm.UpdateDocBotText( GetTypeName().ToString());


            if (fsm.BrokenBotLocation.docBotDetails.docBotHardware.RepairIssues(fsm.name,
                    fsm.BrokenBotLocation.name, DocBotFSM.DocBotTypes.REPAIR_BOT, fsm)) // check if repair will be successful
            // note that the transition to the RETURN_RESUPPLY state is checked in the method above (#RepairIssues)
            
            {
                // can be fixed

                fsm.StartCoroutine(SuccessfulRepair());
            }
     
            // the bot will move to the scrapping state in the #RepairIssues too.

        }


        // so it doesn't repair instantly, takes time.
        private IEnumerator SuccessfulRepair()
        {

            yield return new WaitForSeconds(3);

            if (fsm.stateManager.GetCurrentStateName() !=
                DocBotFSM.DocBotTypes.BROKEN) // make sure when after we wait 2 seconds, the state didnt change to broken.
                // if not, we don't want to change state anymore.
            {
                fsm.BrokenBotLocation.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER);
                // change broken bot back to wander.

                fsm.BrokenBotLocation = null; // we should not have a reference to it anymore
                // as its fixed already.
                
                fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER); // change itself to wander as well.
            }


            
            
        }

        public override void Update()
        {
            // do nothing on broken
        }

        public override void Exit()
        {
            base.Exit();
            
            
            
     
        }


    }

}