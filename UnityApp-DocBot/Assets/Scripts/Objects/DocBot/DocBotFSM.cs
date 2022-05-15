using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Objects.DocBot.States;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Objects.DocBot // PROPER HIERARCHY
{
    public class DocBotFSM : GenericStateManager
    {

        
        public DocBotFSM ReturnStateMachine()
        {


            return this; // return this instance
            
        }

   
        public string docBotId;

        public Transform resupplyTransform, recyclingTransform, workshopTransform;


        [SerializeField]
        private TextMeshProUGUI headerText;
        
        public NavMeshAgent agent { get; set; }
        
        public bool carryingBot { get; set; }


        public DocBotDetails docBotDetails;


        private float cooldownDestroy;


        public int detectionRange = 8;

        private MeshRenderer _renderer;
        
        public GenericStateManager BrokenBotLocation { get; set; } // save the broken bot FSM that we are going to repair

        public DocBotDetails BrokenBotDetails { get; set; } // holds the broken bot information that the docbot can read

        private void Awake()
        {
            stateManager = this; // this instance of generic state manager
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); // let's get the NavMeshAgent from the current game object.
            _renderer = GetComponent<MeshRenderer>();

            docBotDetails = gameObject.AddComponent<DocBotDetails>();
            
   
            
    
            docBotDetails.docBotSupplies.StartAmountResupply(); // resupply on start so the bot has all the components
           
            
            docBotDetails.docBotHardware.InitiateDurabilityPercentage();
            
            LoadAllStates();


            DocBotsManager.Instance.docBotsAlive += 1; // plus one to the total alive.



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
            RETURN_WORKSHOP,
            MACHINE_REPAIR,
            PLACE_BOT,
            DISMANTLE_BOT,
            RETURN_RECYCLE,
            RECYCLE_BOT,
            DESTROYED
            
        }
    
    
        public GenericStateManager stateManager { get; set; }
        // get a new instance of the generic state for this DocBotFSM to manage.
        // getter only, as we only
        // need to get the instance, not set it as it's already been initialized as a new instance when this FSM script begins.
    
        
        public IEnumerator ChangeDelayedState(string state)
        {
     

         
            yield return new WaitForSeconds(2); // wait 2 seconds to change states after diagnosing
            // so it's more realistic as diagnostics doesn't take instantly in real life.

 
            if (!stateManager.GetCurrentStateName().Equals("BROKEN")) // make sure when after we wait 2 seconds, the state didnt change to broken.
            // if not, we don't want to change state anymore.
            {
                stateManager.ChangeState(state);
            }
           
        }
        
        
        private void LoadAllStates()
        {
            stateManager.AddState("WANDER", new WanderState(this, 
                "WANDER", stateManager));
            
            stateManager.AddState("BROKEN", new BrokenState(this, 
                "BROKEN", stateManager));
            
            stateManager.AddState("APPROACH_BOT", new ApproachState(this, 
                "APPROACH_BOT", stateManager));
            
                        
            stateManager.AddState("DIAGNOSE_BOT", new DiagnoseBotState(this, 
                "DIAGNOSE_BOT", stateManager));
            
            stateManager.AddState("REPAIR_BOT", new RepairBotState(this, 
                "REPAIR_BOT", stateManager));
            
            stateManager.AddState("RETURN_SUPPLY", new ReturnSupplyState(this, 
                "RETURN_SUPPLY", stateManager));
            
                        
            stateManager.AddState("RESUPPLY_MATERIALS", new ResupplyMaterialsState(this, 
                "RESUPPLY_MATERIALS", stateManager));
            
            stateManager.AddState("RETURN_BOT_LOCATION", new ReturnBotLocationState(this, 
                "RETURN_BOT_LOCATION", stateManager));
            
            stateManager.AddState("RETURN_WORKSHOP", new ReturnWorkshopState(this, 
                "RETURN_WORKSHOP", stateManager));
            
            stateManager.AddState("MACHINE_REPAIR", new MachineRepairState(this, 
                "MACHINE_REPAIR", stateManager));
            
            stateManager.AddState("PLACE_BOT", new PlaceBotState(this, 
                "PLACE_BOT", stateManager));
            
            
            stateManager.AddState("DISMANTLE_BOT", new DismantleBotState(this, 
                "DISMANTLE_BOT", stateManager));
            
            stateManager.AddState("RETURN_RECYCLE", new ReturnRecycleState(this, 
                "RETURN_RECYCLE", stateManager));
            
            stateManager.AddState("RECYCLE_BOT", new RecycleBotState(this, 
                "RECYCLE_BOT", stateManager));
            
            stateManager.AddState("DESTROYED", new DestroyedState(this, 
                "DESTROYED", stateManager));
            
            
            stateManager.ChangeState("WANDER");
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
                if (Random.Range(0, 100) < 10 && !stateManager.GetCurrentStateName().Equals("BROKEN")) //  10% chance to break (rare chance) and is not alr broken
                {
                    if(DocBotsManager.Instance.docBotsAlive != 1) // if there are more than 1 currently alive doc bots. essentially telling it to not kill the last doc bot.
                        stateManager.ChangeState("BROKEN");
                }

                cooldownDestroy = 5f; // every 5 seconds, it'll have a random chance of being destroyed.
                // enough time for them to do things like repairing, etc
            }


            if (docBotDetails.docBotHardware.DurabilityReduce(Time.deltaTime) <= docBotDetails.docBotHardware.durabilityToRepairAt &&
                !stateManager.GetCurrentStateName().Equals("MACHINE_REPAIR")
                && !stateManager.GetCurrentStateName().Equals("RETURN_WORKSHOP")
                &&  !stateManager.GetCurrentStateName().Equals("BROKEN") &&
                !stateManager.GetCurrentStateName().Equals("DESTROYED")) // reduce durability each time.
                                                                          // and check if its under 10% (docBotDetails.docBotHardware.durabilityToRepairAt)
                                                                          // and make sure it isnt already charging and not broken and not being destroyed.
            {
                // if so, we change to return workshop state. THIS WORKS FOR ANY STATE
                
                stateManager.ChangeState("RETURN_WORKSHOP"); // return to workshop.
            }
        
        }



        public void ChangeColor(Color color)
        {
            _renderer.material.color = color;
        }
        public void UpdateDocBotText(string currentStateType)
        {
            headerText.text = docBotId + " : " + currentStateType;
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }


        public void CarryTargetBot()
        {
            if (BrokenBotLocation != null) // double check if its targeting a broken bot
            {
                
                //BrokenBotLocation.agent.isStopped = true; // make sure its stopped
                
                BrokenBotLocation.transform.SetParent(this.gameObject.transform); // set the broken bot parent to this, so it follows.

                BrokenBotLocation.transform.position =
                    this.gameObject.transform.position + (this.gameObject.transform.forward * 4.5f ); // * 4.5f so it's a little in front.
                                                                                                      // transform.forward to get the
                                                                                                      // blue axis of the robot looking
                                                                                                      // forward, so carrying is in front.
                
            }
        }

        public void RemoveBrokenBot()
        {
            if (BrokenBotLocation != null && !carryingBot) // also make sure we're not carrying a bot, if so, we're actually still tending.
            {
                // was tending to a bot when it needs charging,


             
                BrokenBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
                

                BrokenBotLocation = null; // reset as we're no longer tending to anything.
            }
        }

        public void StopCarryingBot()
        {

            if (carryingBot)
            {
                BrokenBotLocation.transform.SetParent(null); // set to no parents.
                carryingBot = false; // stop carrying
            }

        }

 
    }




}
