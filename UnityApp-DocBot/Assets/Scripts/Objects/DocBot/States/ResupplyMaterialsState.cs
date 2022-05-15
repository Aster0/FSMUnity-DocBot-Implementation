using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class ResupplyMaterialsState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public ResupplyMaterialsState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            base.Enter();
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.RESUPPLY_MATERIALS + ": Resupplying repair materials.");

            fsm.docBotDetails.docBotSupplies.FullResupply(); // resupply
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            
            fsm.StartCoroutine(fsm.ChangeDelayedState("RETURN_BOT_LOCATION"));


        }

        public override void Update()
        {
           
        }

        public override void Exit()
        {
            base.Exit();
      
        }


    }

}