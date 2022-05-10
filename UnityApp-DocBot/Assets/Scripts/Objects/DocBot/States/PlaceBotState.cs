using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class PlaceBotState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public PlaceBotState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();


            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            fsm.StopCarryingBot(); // place bot, stop carrying
            
            fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.WANDER);
            // after placing bot down, we go back to wandering..
            
 


         
        }

        public override void Update()
        {
            // do nothing on broken
        }

        public override void Exit()
        {
            base.Exit();
            fsm.carryingBot = false; // we stopped carrying when we exit this state.

        }


    }

}