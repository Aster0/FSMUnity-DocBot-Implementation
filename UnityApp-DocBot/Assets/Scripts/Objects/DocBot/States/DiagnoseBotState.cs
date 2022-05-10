using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class DiagnoseBotState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;
        
        public DiagnoseBotState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
    
            Debug.Log(fsm.name + " " + DocBotFSM.DocBotTypes.DIAGNOSE_BOT + ": Diagnosing a broken bot");

            fsm.UpdateDocBotText( GetTypeName().ToString());

            fsm.BrokenBotLocation.docBotDetails.docBotHardware.DiagnoseIssue(fsm.name,
                fsm.BrokenBotLocation.name, DocBotFSM.DocBotTypes.DIAGNOSE_BOT);

            fsm.StartCoroutine(fsm.ChangeDelayedState(DocBotFSM.DocBotTypes.REPAIR_BOT));

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