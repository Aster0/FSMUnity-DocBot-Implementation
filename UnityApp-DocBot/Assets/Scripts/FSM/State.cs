
using UnityEngine;

namespace FSM // PROPER HIERARCHY
{
    public abstract class State<TNm>  // Generic abstract super class for all States to implement. generic TNm = Type Name.
    {

        // PROTECTED TYPES SO ONLY HIERARCHY CLASSES (SUB CLASSES) THAT IMPLEMENT THIS SUPER CLASS
        // CAN ACCESS. AND NOT CLASSES THAT HAVE AN INSTANCE. 
        
        #region Variables
        protected GenericState<TNm> _genericState;
        


        private TNm name;
        #endregion
        
        protected State(GenericState<TNm> state, TNm name)
        {
            _genericState = state;


            this.name = name; // this refers to this current instance's variable called name.




            
        }



        public TNm GetTypeName() // getter for name var.
        {
            return name;
        }



        #region Virtual Methods

        public virtual void Update()  // virtual method to be overridden by sub class, Update() will update per frame. 
        {
            
        }
        
        public virtual void Enter()  // virtual method to be overridden by sub class, Enter() will be executed when State is entered 
        {
            Debug.Log("Entering the " + name + " State now!");
        }
        
                
        public virtual void Exit()  // virtual method to be overridden by sub class, Enter() will be executed when State is exited. 
        {
            Debug.Log("Exiting the " + name + " State now!");
        }

        #endregion

        
        



    }
}

