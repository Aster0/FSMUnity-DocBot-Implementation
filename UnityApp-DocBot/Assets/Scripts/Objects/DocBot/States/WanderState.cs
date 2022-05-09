using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Objects.DocBot.States // PROPER HIERARCHY (Stores all of DocBot's states)
{
    
    public class WanderState<TNm> : State<TNm> // TNm determines the datatype of the name (key)
    {

        private DocBotFSM fsm;


        private const int MAX_ROAM_X = 20, MAX_ROAM_Z = 20, DETECTION_RANGE = 5;


        private bool canMove;


        private Vector3 lastPositionSaved;
        
        public WanderState(DocBotFSM fsm, TNm typeName, GenericState<TNm> stateManager) : base(stateManager, typeName) 
        // these variables are assigned
        // in the super class' variables that we can access (as protected and public vars)
        {
            this.fsm = fsm; // the specific fsm, we will save it.
        }

        public override void Enter()
        {
            
    
            base.Enter();

            fsm.UpdateDocBotText( GetTypeName().ToString());
            lastPositionSaved = fsm.transform.position; // updates the current position when it enters into this state.
            canMove = true; // allow this docbot to move
        }

        public override void Update()
        {
            RandomWander();

            CheckSurroundings();
            
            Debug.Log("Wandering..");
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void CheckSurroundings()
        {
            RaycastHit hit;


            Collider[] colliders = Physics.OverlapSphere(fsm.transform.position, DETECTION_RANGE);


            
          
            foreach (Collider collider in colliders)
            {
                
                Debug.Log(collider.tag);
                if (collider.CompareTag("DocBot") && collider.gameObject != fsm.gameObject) // check if its not itself and it found another docbot
                {
                    DocBotFSM targetedDocFSM = collider.gameObject.GetComponent<DocBotFSM>();

                    if (targetedDocFSM.stateManager.GetCurrentStateName() == DocBotFSM.DocBotTypes.BROKEN) // check if its broken, then approach. if not dont.
                    {
                        canMove = false;
                        Debug.Log("Yes");

                        fsm.BrokenBotLocation = collider.transform.position;
                    
                        fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.APPROACH_BOT);
                    }
                
                }
            }
            
            
            
            
            
        }


        private void RandomWander()
        {
            if (fsm.transform.position.x == lastPositionSaved.x 
                && fsm.transform.position.z == lastPositionSaved.z && canMove) // check the x and z position if its the same. ignoring the y (because not needed)
            {

                float randomX = Random.Range(-MAX_ROAM_X, MAX_ROAM_X);
                float randomZ = Random.Range(-MAX_ROAM_Z, MAX_ROAM_Z);

                lastPositionSaved = new Vector3(randomX, 0, randomZ); // save new position to go to.
             
                fsm.agent.SetDestination(lastPositionSaved); // go to the new position using NavMeshAgent.
            }
            
        }
    }

}

