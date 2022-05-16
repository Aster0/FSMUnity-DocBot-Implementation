using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class MachineRepairState : State<string> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        private bool doingAdvancedRepairs;
        
        public MachineRepairState(DocBotFSM fsm, string typeName, GenericStateManager<string> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.MACHINE_REPAIR + ": is using the machines in the workshop to either do maintenance on itself or advanced repairs on a broken bot.");

            doingAdvancedRepairs = false;
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
         
        }

        public override void Update()
        {
            //  repair


            RaycastHit hitInfo;
            Physics.Raycast(fsm.transform.position, -fsm.transform.up, out hitInfo);
            
            if (hitInfo.transform.gameObject == fsm.workshopTransform.gameObject) // double check if its actually at the workshop, then we repair using machines.
            {
                

                if (fsm.carryingBot && !doingAdvancedRepairs) // if carrying bot to do advanced repairs at the workshop,
                {
                    doingAdvancedRepairs = true;
                    fsm.StartCoroutine(DoAdvancedRepairs());
                }
                else // if not, maintenance on itself by increasing its own durability so it doesn't break that easily.
                {
                    if(fsm.docBotDetails.docBotHardware.DurabilityGain(Time.deltaTime) >= 100) // if its 100% already or more (might be more because its a float so just a secure check)
                    {

         

              
                        fsm.stateManager.ChangeState("WANDER"); // back to wandering state.
                

               
                    }
                }
            }

          
        }

        private IEnumerator DoAdvancedRepairs()
        {
            yield return new WaitForSeconds(3);


            if (!fsm.GetCurrentStateName().Equals("BROKEN")) // not broken after 3 seconds
            {
                
                fsm.StopCarryingBot(); // place bot, stop carrying because repairs are done.
                
                if(fsm.BrokenBotLocation != null)
                    fsm.BrokenBotLocation.ChangeState("WANDER");
                fsm.stateManager.ChangeState("WANDER");

           
            }
      
  
        }

        public override void Exit()
        {
            base.Exit();
            
         
        }


    }

}