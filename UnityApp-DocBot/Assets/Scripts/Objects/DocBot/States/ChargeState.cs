using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ChargeState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ChargeState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.CHARGE + ": Charging battery..");

            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
         
        }

        public override void Update()
        {
            //  charge


            RaycastHit hitInfo;
            bool charger = Physics.Raycast(fsm.transform.position, -fsm.transform.up, out hitInfo);
            
            if (hitInfo.transform.gameObject == fsm.chargingTransform.gameObject) // double check if its actually at the charging station, then we charge.
            {
                
                if(fsm.docBotDetails.docBotHardware.BatteryCharge(Time.deltaTime) >= 100) // if its 100% already or more (might be more because its a float so just a secure check)
                {

         

              
                    fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER); // back to wandering state.
                

               
                }
            }

          
        }

        public override void Exit()
        {
            base.Exit();
            
         
        }


    }

}