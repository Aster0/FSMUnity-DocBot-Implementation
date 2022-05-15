using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class PlaceBotState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public PlaceBotState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.PLACE_BOT + ": Placing the broken bot down on the charging pad for it to charge.");


            fsm.UpdateDocBotText( GetTypeName().ToString());
            
         
            
            fsm.stateManager.ChangeState("MACHINE_REPAIR");
            // after placing bot down, we go back to wandering..
            
 


         
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