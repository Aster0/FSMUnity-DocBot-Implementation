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

        public Transform resupplyTransform, recyclingTransform, chargingTransform, chargingMove;

        [SerializeField]
        private TextMeshProUGUI headerText;
        
        public NavMeshAgent agent { get; set; }


        public DocBotDetails docBotDetails;


        private float cooldownDestroy;


        public int detectionRange = 8;
        
        public DocBotFSM BrokenBotLocation { get; set; } // save the broken bot FSM that we are going to repair
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); // let's get the NavMeshAgent from the current game object.

            
            docBotDetails.docBotSupplies.StartAmountResupply(); // resupply on start so the bot has all the components
           
            
            LoadAllStates();
            
            
            
        }





        public enum DocBotTypes // constant DocBotTypes Value as enums. Enum will be the name of the States,
            // it is an Enum because it most likely will not change or have minor State additions in the future.
        {
            WANDER,
            BROKEN,
            APPROACH_BOT,
            DIAGNOSE_BOT,
            REPAIR_BOT,
            RETURN_SUPPLY,
            RESUPPLY_MATERIALS,
            RETURN_BOT_LOCATION,
            RETURN_CHARGE,
            CHARGE
            
        }
    
    
        public GenericState<DocBotTypes> stateManager { get; } = new GenericState<DocBotTypes>();
        // get a new instance of the generic state for this DocBotFSM to manage.
        // getter only, as we only
        // need to get the instance, not set it as it's already been initialized as a new instance when this FSM script begins.
    
        
        public IEnumerator ChangeDelayedState(DocBotTypes state)
        {
     

         
            yield return new WaitForSeconds(2); // wait 2 seconds to change states after diagnosing
            // so it's more realistic as diagnostics doesn't take instantly in real life.

 
            if (stateManager.GetCurrentStateName() != DocBotTypes.BROKEN) // make sure when after we wait 2 seconds, the state didnt change to broken.
            // if not, we don't want to change state anymore.
            {
                stateManager.ChangeState(state);
            }
           
        }
        
        
        private void LoadAllStates()
        {
            stateManager.AddState(DocBotTypes.WANDER, new WanderState<DocBotTypes>(this, 
                DocBotTypes.WANDER, stateManager));
            
            stateManager.AddState(DocBotTypes.BROKEN, new BrokenState<DocBotTypes>(this, 
                DocBotTypes.BROKEN, stateManager));
            
            stateManager.AddState(DocBotTypes.APPROACH_BOT, new ApproachState<DocBotTypes>(this, 
                DocBotTypes.APPROACH_BOT, stateManager));
            
                        
            stateManager.AddState(DocBotTypes.DIAGNOSE_BOT, new DiagnoseBotState<DocBotTypes>(this, 
                DocBotTypes.DIAGNOSE_BOT, stateManager));
            
            stateManager.AddState(DocBotTypes.REPAIR_BOT, new RepairBotState<DocBotTypes>(this, 
                DocBotTypes.REPAIR_BOT, stateManager));
            
            stateManager.AddState(DocBotTypes.RETURN_SUPPLY, new ReturnSupplyState<DocBotTypes>(this, 
                DocBotTypes.RETURN_SUPPLY, stateManager));
            
                        
            stateManager.AddState(DocBotTypes.RESUPPLY_MATERIALS, new ResupplyMaterialsState<DocBotTypes>(this, 
                DocBotTypes.RESUPPLY_MATERIALS, stateManager));
            
            stateManager.AddState(DocBotTypes.RETURN_BOT_LOCATION, new ReturnBotLocationState<DocBotTypes>(this, 
                DocBotTypes.RETURN_BOT_LOCATION, stateManager));
            
            stateManager.AddState(DocBotTypes.RETURN_CHARGE, new ReturnChargerState<DocBotTypes>(this, 
                DocBotTypes.RETURN_CHARGE, stateManager));
            
            stateManager.AddState(DocBotTypes.CHARGE, new ChargeState<DocBotTypes>(this, 
                DocBotTypes.CHARGE, stateManager));
            
            
            
            
            stateManager.ChangeState(DocBotTypes.WANDER);
        }
        

        // Update is called once per frame
        void Update()
        {
            stateManager.StateUpdate(); // Update the current state using the Generic State's instance (specific to this FSM)


            // BELOW WORKS FOR ANY STATES. BROKEN AND CHARGING BECAUSE IT CAN HAPPEN AT ANY ANY ANY STATE,
            // NOT JUST SPECIFIC STATES.
            
            cooldownDestroy -= Time.deltaTime;

            if (cooldownDestroy < 0) // cooldown for destroy so it doesn't always check for the range.
            {
                if (Random.Range(0, 100) < 10 && stateManager.GetCurrentStateName() != DocBotTypes.BROKEN) //  10% chance to break (rare chance) and is not alr broken
                {
                    stateManager.ChangeState(DocBotTypes.BROKEN);
                }

                cooldownDestroy = 5f; // every 5 seconds, it'll have a random chance of being destroyed.
                // enough time for them to do things like repairing, etc
            }


            if (docBotDetails.docBotHardware.BatteryReduce(Time.deltaTime) <= 15 &&
                stateManager.GetCurrentStateName() != DocBotTypes.CHARGE
                &&  stateManager.GetCurrentStateName() != DocBotTypes.BROKEN) // reduce battery each time.
                                                                          // and check if its under 15%
                                                                          // and make sure it isnt already charging and not broken
            {
                // if so, we change to charging state. THIS WORKS FOR ANY STATE
                
                stateManager.ChangeState(DocBotTypes.RETURN_CHARGE); // return to charger.
            }
        
        }



        public void UpdateDocBotText(string currentStateType)
        {
            headerText.text = docBotId + " : " + currentStateType;
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }




}
