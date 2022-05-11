using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class RecycleBotState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public RecycleBotState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();

        
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RECYCLE_BOT + ": Recycling an unrepairable broken bot.");


            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            
            

            fsm.BrokenBotLocation.stateManager.ChangeState(DocBotFSM.DocBotTypes.DESTROYED); // change to destroyed
            // cos its recycled.

            fsm.StartCoroutine(fsm.ChangeDelayedState(DocBotFSM.DocBotTypes.WANDER)); 
            // delay to a wander state as its recycling. 



        }

        public override void Update()
        {
            
    
            
        }

        public override void Exit()
        {
            base.Exit();

            fsm.carryingBot = false; // no longer carrying, recycled. 

        }


    }

}