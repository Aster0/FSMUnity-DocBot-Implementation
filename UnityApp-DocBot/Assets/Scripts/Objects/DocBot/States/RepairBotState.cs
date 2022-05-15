using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class RepairBotState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public RepairBotState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

      
            fsm.UpdateDocBotText( GetTypeName().ToString());

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.REPAIR_BOT + ": Attempts to perform repairs on a broken bot.");



            fsm.StartCoroutine(BeginRepair());

     
            // the bot will move to the dismantle state in the #RepairIssues too.
            // also bringingg to the charging station is in the same method.

        }

        private IEnumerator BeginRepair()
        {
            yield return new WaitForSeconds(2);
            if (fsm.BrokenBotLocation != null && !fsm.stateManager.GetCurrentStateName().Equals("BROKEN")) // null check and not broken check
            {
                if (fsm.BrokenBotDetails.docBotHardware.RepairIssues(fsm.name,
                        fsm.BrokenBotLocation.name, DocBotFSM.DocBotTypes.REPAIR_BOT, fsm)) // check if repair will be successful
                    // note that the transition to the RETURN_RESUPPLY state is checked in the method above (#RepairIssues)
            
                {
                    // can be fixed

                    fsm.StartCoroutine(SuccessfulRepair());
                }
            }
        }


        // so it doesn't repair instantly, takes time.
        private IEnumerator SuccessfulRepair()
        {

            yield return new WaitForSeconds(3);

            if (!fsm.stateManager.GetCurrentStateName().Equals("BROKEN")) // make sure when after we wait 2 seconds, the state didnt change to broken.
                // if not, we don't want to change state anymore.
            {
                fsm.BrokenBotLocation.ChangeState("WANDER");
                // change broken bot back to wander.

                fsm.BrokenBotLocation = null; // we should not have a reference to it anymore
                // as its fixed already.
                
                fsm.stateManager.ChangeState("WANDER"); // change itself to wander as well.
                
                DocBotsManager.Instance.docBotsAlive += 1; // plus one to the total alive as the docbot is just repaired

            }


            
            
        }

        public override void Update()
        {
           
            
        }

        public override void Exit()
        {
            base.Exit();
            
            
            
     
        }


    }

}