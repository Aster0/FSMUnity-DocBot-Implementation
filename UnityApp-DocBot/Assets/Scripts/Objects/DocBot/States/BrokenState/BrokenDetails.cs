using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States.BrokenState // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class BrokenState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public BrokenState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

            fsm.agent.isStopped = true; // stop the agent from moving immediately. 
            fsm.UpdateDocBotText( GetTypeName().ToString());
         
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