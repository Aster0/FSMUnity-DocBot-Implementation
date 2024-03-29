using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;


namespace FSM // PROPER HIERARCHY
{
    
    public class GenericStateManager<TNm>: MonoBehaviour // note that t here is monobehavior here because the specific FSM like DocBotFSM will extend GenericStateManager, thus, getting MonoBehavior too.
        // the reason why DocBotFSM will extend this Monobehavior is so DocBotFSM can also be classified under a generic datatype which is GenericStateManager. So can all repairable bots be classified under this datatype.
        // so the repairing doc-bot can get GetComponent<GenericStateManager>() to repair other bots that are NOT doc-bots.
    // specific FSM (e.g. DocBotFSM) will need a new instance of GenericState as to get a new dictionary with its corresponding (new instance is created when extending)
    // State's instances.

    // TNm means Type Name. Generic Datatype for whatever we want to define each state as for keys.
    {   

        private Dictionary<TNm, FSM.State<TNm>> states = new Dictionary<TNm, FSM.State<TNm>>();

        public FSM.State<TNm> CurrentState { get; set; }


        // #AddState - Add a state to the dictonary
        // @param - receives a key and value
        public void AddState(TNm key, FSM.State<TNm> value)
        {
            if (value != null) // the adding state isn't null and is valid
            {
                states.Add(key, value); // add a new entry in the dictionary with the key and value.
            }
        }



        // #GetState - Get a state using a key.
        // @param - receives a generic DataType for the type name.
        // @return - returns a state that corresponds with the type name given.
        // private because it's only accessible in this class instance. 
        private FSM.State<TNm> GetState(TNm typeName)
        {
            if (states.ContainsKey(typeName)) // check if the typeName exists in the dictionary states.
            {
                return states[typeName]; // return the state that exists with the key. 
            }


            // Prompt the user that the key is invalid and can't be found. 
            // Return null so nothing. 
            Debug.Log("Invalid type name! State can't be found."); 
            return null;
        }


        public string GetCurrentStateName()
        {
            return CurrentState.GetTypeName();
        }


        // #ChangeState - changes the Current State into a new state.
        // @param - receives a state to change into
        public void ChangeState(TNm typeName)
        {

            FSM.State<TNm> newState = GetState(typeName);
            
            if (CurrentState != null) // if there is a CurrentState already,
            {
                CurrentState.Exit(); // let's exit the previous state before assigning a new current state.
            }

            CurrentState = newState; // assign the new current state.

            if (CurrentState != null) // we double check if the new current state is valid and not null.
            {
                CurrentState.Enter(); // if it's valid, we can enter it.
            }



        }


        // #StateUpdate - executes the update per frame for the current state.
        public void StateUpdate()
        {
            if (CurrentState != null) // if it's current state not null,
            {
                // let's update it.
                CurrentState.Update();
            }
            
                
        }
        
        public void DestroyThisObject()
        {
            Destroy(this.gameObject, 3);
        }
        
        
    }

    
}

