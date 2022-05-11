using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEditor;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
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

            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.BROKEN + ": Bot broke down.. Waiting for a doc-bot to perform diagnostics on it.");

            
            base.Enter();
            
            fsm.ChangeColor(Color.red); // red for broken

            fsm.agent.isStopped = true; // stop the agent from moving immediately. 
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            fsm.docBotDetails.docBotHardware.OnBreakDown(); // call on the on break down event
            
            DocBotsManager.Instance.docBotsAlive -= 1; // minus one to the total alive.


            fsm.RemoveBrokenBot();
         
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