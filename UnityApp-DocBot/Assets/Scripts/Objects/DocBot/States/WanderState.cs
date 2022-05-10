using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class WanderState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;


        private const int MAX_ROAM_X = 20, MAX_ROAM_Z = 20;


        private bool canMove;

        private float timeLastWandered; // time last the bot wandered.


        private Vector3 lastPositionSaved;

        private int randomWanderCooldown;
        
        public WanderState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
            
    
            base.Enter();

            randomWanderCooldown = Random.Range(1, 3);
            
            fsm.UpdateDocBotText( GetTypeName().ToString());
            lastPositionSaved = fsm.transform.position; // updates the current position when it enters into this state.
            canMove = true; // allow this docbot to move

            fsm.agent.isStopped = false; // reset movement for agent.


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
                
          
                if (collider.CompareTag("DocBot") && collider.gameObject != fsm.gameObject) // check if its not itself and it found another docbot
                {
                    DocBotFSM targetedDocFSM = collider.gameObject.GetComponent<DocBotFSM>();

                    if (targetedDocFSM.stateManager.GetCurrentStateName() == DocBotFSM.DocBotTypes.BROKEN
                        && !targetedDocFSM.docBotDetails.isTended) // check if its broken, then approach. if not dont.
                        // and check if its currently tended by another bot, if it is not, we can tend to it.
                    {
                        canMove = false;
                        targetedDocFSM.docBotDetails.isTended = true; 

                        fsm.BrokenBotLocation = targetedDocFSM;
                    
                        fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.APPROACH_BOT);

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

