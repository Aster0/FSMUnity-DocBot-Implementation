using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class WanderState : State // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;


        private const int MAX_ROAM_X = 20, MAX_ROAM_Z = 20;


        private bool canMove;

        private float timeLastWandered; // time last the bot wandered.


        private Vector3 lastPositionSaved;

        private int randomWanderCooldown;
        
        public WanderState(DocBotFSM fsm, string typeName, GenericStateManager stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
            
    
            base.Enter();
            
            fsm.ChangeColor(Color.green); // green for working. when it hits wandering, means its working always.
            
            Debug.Log(fsm.docBotId + " - " + DocBotFSM.DocBotTypes.WANDER + ": Wanders around the field, finding broken bots.");

            randomWanderCooldown = Random.Range(1, 5);
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            lastPositionSaved = fsm.transform.position; // updates the current position when it enters into this state.
            canMove = true; // allow this docbot to move

            fsm.agent.isStopped = false; // reset movement for agent.

            fsm.carryingBot = false; // make sure we stop carrying any bots when we're wandering

            fsm.RemoveBrokenBot(); // make sure we stop carrying and reset values.
            
            fsm.docBotDetails.isTended = false; // make sure isTended is false so when it breaks again,
            // someone will tend (because now we can wander.)
      
        }

        public override void Update()
        {
            RandomWander();

            CheckSurroundings();
            
           
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void CheckSurroundings()
        {
         


            Collider[] colliders = Physics.OverlapSphere(fsm.transform.position, fsm.detectionRange);


            
          
            foreach (Collider collider in colliders)
            {
                
          
                
                // COMPARE TAG (AS LONG AS ITS A BOT AND ITS BROKEN, DOCBOT WILL TRY TO REPAIR)
                if (collider.CompareTag("Bot") && collider.gameObject != fsm.gameObject) // check if its not itself and it found another docbot
                {
                    GenericStateManager targetedDocFSM = collider.gameObject.GetComponent<GenericStateManager>();

                    DocBotDetails brokenBotDetails = targetedDocFSM.GetComponent<DocBotDetails>();
                    
                    // note that the doc bot can approach ANY BOTS that extends the GenericStateManager. 
                    // This is because it's so generic that the doc-bot can repair all other bots. 
                    // So, I have made a Random Bot in the scene for this proof of concept to work, u can see that
                    // it works fabulously and that the doc-bot can also access the other bot's FSM by accessing the 
                    // generic state manager to change its state, thus, repairing it when needed.

                    if (targetedDocFSM.GetCurrentStateName().Equals("BROKEN")
                        && !brokenBotDetails.isTended) // check if its broken, then approach. if not dont.
                        // and check if its currently tended by another bot, if it is not, we can tend to it.
                    {
                        
                        brokenBotDetails.isTended = true;  // is being tended by this bot.
                        canMove = false;
                   

                        fsm.BrokenBotLocation = targetedDocFSM;
                        fsm.BrokenBotDetails = brokenBotDetails;
                        
                    
                        fsm.stateManager.ChangeState("APPROACH_BOT");

                        break; // break out and only tend to the first bot it sees
                    }
                
                }
            }
            
            
            
            
            
        }


        private void RandomWander()
        {


            
        
        
            if (timeLastWandered >= randomWanderCooldown) // in x seconds since it just wandered (randomized so all bots will be wandering differently)
            {
               
                // we random wander again so its a new location so it doesnt get stuck.
                Wander();
            }

            timeLastWandered += Time.deltaTime;


        }

        private void Wander()
        {
            float randomX = Random.Range(-MAX_ROAM_X, MAX_ROAM_X);
            float randomZ = Random.Range(-MAX_ROAM_Z, MAX_ROAM_Z);

            lastPositionSaved = new Vector3(randomX, 0, randomZ); // save new position to go to.
             
            fsm.agent.SetDestination(lastPositionSaved); // go to the new position using NavMeshAgent.

            timeLastWandered = 0; // reset to 0 cos just wandered


        }
    }

}

