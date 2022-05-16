using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEditor;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class BrokenState : State<string> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public BrokenState(DocBotFSM fsm, string typeName, GenericStateManager<string> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {

            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.BROKEN + ": Bot broke down.. Waiting for a doc-bot to perform diagnostics on it.");

            
            base.Enter();
            
            if(fsm.BrokenBotLocation != null) // if we are tending to a broken bot
                fsm.BrokenBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
            
            fsm.StopCarryingBot();
            
            
            fsm.ChangeColor(Color.red); // red for broken

            fsm.agent.isStopped = true; // stop the agent from moving immediately. 
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            fsm.docBotDetails.docBotHardware.OnBreakDown(); // call on the on break down event
            
            DocBotsManager.Instance.docBotsAlive -= 1; // minus one to the total alive.


        
            
            // when another doc-bot repairs this broken bot, then it comes out of this state.
         
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