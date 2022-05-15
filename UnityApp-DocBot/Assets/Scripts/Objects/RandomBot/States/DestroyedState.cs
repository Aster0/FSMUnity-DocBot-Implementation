using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;



namespace Objects.RandomBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    public class DestroyedState : State // this is just an empty state for a random bot I 
    // made for the doc-bot to interact with. NOTHING SPECIAL
    {
        private RandomBotFSM fsm;
        
        public DestroyedState(RandomBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }


        public override void Enter()
        {
            base.Enter();

            fsm.DestroyThisObject(); // destroy the bot in a few seconds.
            fsm.UpdateDocBotText( GetTypeName().ToString()); // do nothing except show BROKEN STATE for random bot 
        }
    }
}


