using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class MachineRepairState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public MachineRepairState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.MACHINE_REPAIR + ": is using the machines in the workshop to either do maintenance on itself or advanced repairs on a broken bot.");

            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
         
        }

        public override void Update()
        {
            //  repair


            RaycastHit hitInfo;
            bool charger = Physics.Raycast(fsm.transform.position, -fsm.transform.up, out hitInfo);
            
            if (hitInfo.transform.gameObject == fsm.workshopTransform.gameObject) // double check if its actually at the workshop, then we repair using machines.
            {
                

                if (fsm.carryingBot) // if carrying bot to do advanced repairs at the workshop,
                {
                    DoAdvancedRepairs();
                }
                else // if not, maintenance on itself by increasing its own durability so it doesn't break that easily.
                {
                    if(fsm.docBotDetails.docBotHardware.DurabilityGain(Time.deltaTime) >= 100) // if its 100% already or more (might be more because its a float so just a secure check)
                    {

         

              
                        fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER); // back to wandering state.
                

               
                    }
                }
            }

          
        }

        private IEnumerator DoAdvancedRepairs()
        {
            yield return new WaitForSeconds(3);
            
            fsm.StopCarryingBot(); // place bot, stop carrying because repairs are done.
            fsm.BrokenBotLocation.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER);
            fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER);
        }

        public override void Exit()
        {
            base.Exit();
            
         
        }


    }

}