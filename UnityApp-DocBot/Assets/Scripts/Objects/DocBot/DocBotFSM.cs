using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Objects.DocBot.States;
using Objects.DocBot.States.BrokenState;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Objects.DocBot // PROPER HIERARCHY
{
    public class DocBotFSM : MonoBehaviour
    {


        [SerializeField]
        private string docBotId;

        [SerializeField]
        private TextMeshProUGUI headerText;
        
        public NavMeshAgent agent { get; set; }
        
        
        public Vector3 BrokenBotLocation { get; set; } // save the broken bot location that we are going to repair
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); // let's get the NavMeshAgent from the current game object.


            LoadAllStates();
        }





        public enum DocBotTypes // constant DocBotTypes Value as enums. Enum will be the name of the States,
            // it is an Enum because it most likely will not change or have minor State additions in the future.
        {
            WANDER,
            BROKEN,
            APPROACH_BOT
            
        }
    
    
        public GenericState<DocBotTypes> stateManager { get; } = new GenericState<DocBotTypes>();
        // get a new instance of the generic state for this DocBotFSM to manage.
        // getter only, as we only
        // need to get the instance, not set it as it's already been initialized as a new instance when this FSM script begins.
    
        
        
        private void LoadAllStates()
        {
            stateManager.AddState(DocBotTypes.WANDER, new WanderState<DocBotTypes>(this, 
                DocBotTypes.WANDER, stateManager));
            
            stateManager.AddState(DocBotTypes.BROKEN, new BrokenState<DocBotTypes>(this, 
                DocBotTypes.BROKEN, stateManager));
            
            stateManager.AddState(DocBotTypes.APPROACH_BOT, new ApproachState<DocBotTypes>(this, 
                DocBotTypes.APPROACH_BOT, stateManager));
            
            
            
            stateManager.ChangeState(DocBotTypes.WANDER);
        }
        

        // Update is called once per frame
        void Update()
        {
            stateManager.StateUpdate(); // Update the current state using the Generic State's instance (specific to this FSM)
            
            
            
            
            if (Random.Range(0, 9999) < 1) // 1 out of 9999 chance to break (rare chance)
            {
                stateManager.ChangeState(DocBotTypes.BROKEN);
            }
        }



        public void UpdateDocBotText(string currentStateType)
        {
            headerText.text = docBotId + " : " + currentStateType;
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, 5);
        }
    }




}
