using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class DestroyedState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public DestroyedState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

        
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            
            

            fsm.DestroyThisBot(); // destroy the bot in a few seconds.


        }



 


    }

}