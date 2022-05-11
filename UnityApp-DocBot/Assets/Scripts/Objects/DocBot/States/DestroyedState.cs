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

        
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.DESTROYED + ": Bot is unrepairable (determined by the repairing doc-bot) - if it's destroyed, meaning the other doc-bot recycled this broken bot.");

            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            
            DocBotsManager.Instance.docBotsAlive -= 1; // minus one to the total alive.

            fsm.DestroyThisBot(); // destroy the bot in a few seconds.


        }



 


    }

}