using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;



namespace Objects.RandomBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    public class MachineRepairState : State<string> // this is just an empty state for a random bot I 
    // made for the doc-bot to interact with. NOTHING SPECIAL
    {
        private RandomBotFSM fsm;
        
        public MachineRepairState(RandomBotFSM fsm, string typeName, GenericStateManager<string> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }


        public override void Enter()
        {
            base.Enter();
            
            fsm.ChangeColor(Color.green);
            fsm.UpdateDocBotText( "MACHINE_REPAIR"); // do nothing except show that its been repaired by a doc-bot
        }
    }
}


