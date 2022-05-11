using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Objects.DocBot.States;
using Objects.DocBot.States.BrokenState;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Objects.DocBot // PROPER HIERARCHY
{
    public class DocBotFSM : MonoBehaviour
    {


   
        public string docBotId;

        public Transform resupplyTransform, recyclingTransform, workshopTransform;


        [SerializeField]
        private TextMeshProUGUI headerText;
        
        public NavMeshAgent agent { get; set; }
        
        public bool carryingBot { get; set; }


        public DocBotDetails docBotDetails;


        private float cooldownDestroy;


        public int detectionRange = 8;
        
        public DocBotFSM BrokenBotLocation { get; set; } // save the broken bot FSM that we are going to repair
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); // let's get the NavMeshAgent from the current game object.

            
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
            
            stateManager.AddState(DocBotTypes.RETURN_WORKSHOP, new ReturnWorkshopState<DocBotTypes>(this, 
                DocBotTypes.RETURN_WORKSHOP, stateManager));
            
            stateManager.AddState(DocBotTypes.MACHINE_REPAIR, new MachineRepairState<DocBotTypes>(this, 
                DocBotTypes.MACHINE_REPAIR, stateManager));
            
            stateManager.AddState(DocBotTypes.PLACE_BOT, new PlaceBotState<DocBotTypes>(this, 
                DocBotTypes.PLACE_BOT, stateManager));
            
            
            stateManager.AddState(DocBotTypes.DISMANTLE_BOT, new DismantleBotState<DocBotTypes>(this, 
                DocBotTypes.DISMANTLE_BOT, stateManager));
            
            stateManager.AddState(DocBotTypes.RETURN_RECYCLE, new ReturnRecycleState<DocBotTypes>(this, 
                DocBotTypes.RETURN_RECYCLE, stateManager));
            
            stateManager.AddState(DocBotTypes.RECYCLE_BOT, new RecycleBotState<DocBotTypes>(this, 
                DocBotTypes.RECYCLE_BOT, stateManager));
            
            stateManager.AddState(DocBotTypes.DESTROYED, new DestroyedState<DocBotTypes>(this, 
                DocBotTypes.DESTROYED, stateManager));
            
            
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
                    if(DocBotsManager.Instance.docBotsAlive != 1) // if there are more than 1 currently alive doc bots. essentially telling it to not kill the last doc bot.
                        stateManager.ChangeState(DocBotTypes.BROKEN);
                }

                cooldownDestroy = 5f; // every 5 seconds, it'll have a random chance of being destroyed.
                // enough time for them to do things like repairing, etc
            }


            if (docBotDetails.docBotHardware.DurabilityReduce(Time.deltaTime) <= 15 &&
                stateManager.GetCurrentStateName() != DocBotTypes.MACHINE_REPAIR 
                && stateManager.GetCurrentStateName() != DocBotTypes.RETURN_WORKSHOP 
                &&  stateManager.GetCurrentStateName() != DocBotTypes.BROKEN &&
                stateManager.GetCurrentStateName() != DocBotTypes.DESTROYED) // reduce durability each time.
                                                                          // and check if its under 15%
                                                                          // and make sure it isnt already charging and not broken and not being destroyed.
            {
                // if so, we change to charging state. THIS WORKS FOR ANY STATE
                
                stateManager.ChangeState(DocBotTypes.RETURN_WORKSHOP); // return to charger.
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


        public void CarryTargetBot()
        {
            if (BrokenBotLocation != null) // double check if its targeting a broken bot
            {
                BrokenBotLocation.agent.isStopped = true; // make sure its stopped
                
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


                BrokenBotLocation.docBotDetails.isTended = false; // we leave the bot untended so it can be tended
                // by another bot later.
                
                StopCarryingBot();

                BrokenBotLocation = null; // reset as we're no longer tending to anything.
            }
        }

        public void StopCarryingBot()
        {
     
            BrokenBotLocation.transform.SetParent(null); // set to no parents.

        }

        public void DestroyThisBot()
        {
            Destroy(this.gameObject, 3);
        }
    }




}
