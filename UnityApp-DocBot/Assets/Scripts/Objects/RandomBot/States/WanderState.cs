using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using UnityEngine.AI;


namespace Objects.RandomBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    public class WanderState : State // this is just an empty state for a random bot I 
        // made for the doc-bot to interact with. NOTHING SPECIAL 
    // THE WANDER STATE DOES NOTYHING EXCEPT TO DESTROY THE RANDOM BOT AFTER ITS BEING REPAIRED BY THE MAIN AGENT
    // WHICH IS THE DOC BOT!
    {
        private RandomBotFSM fsm;



        
        public WanderState(RandomBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }


        public override void Enter()
        {
            base.Enter();
            fsm.ChangeColor(Color.green);
            fsm.UpdateDocBotText("REPAIRED"); // do nothing except show that its been repaired by a doc-bot
        }
    }
}