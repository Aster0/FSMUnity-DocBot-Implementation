using System.Collections;
using System.Collections.Generic;
using FSM;
using Objects.DocBot;
using Objects.RandomBot.States;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RandomBotFSM : GenericStateManager // extending GenericStateManager so DocBotFSM can be classified under GenericStateManager datatype. Makes it easier for us to detect bots that can be repaired.
    // extending also creates a new instance of GenericStateManager, thus, all specific FSM will tie to a new instance of the GenericStateManager.
{
    
    // NOTE THAT THIS BOT IS ONLY MADE FOR A PROOF OF CONCEPT THAT THE DOC-BOT CAN INTERACT
    // WITH AS MANY OTHER BOTS.
    
    
    public NavMeshAgent agent;
    
    [SerializeField]
    private TextMeshProUGUI headerText;
    
    
    public DocBotDetails BotDetails { get; set; }
    
    private MeshRenderer _renderer;
    
    public void UpdateDocBotText(string currentStateType)
    {
        headerText.text = name + " : " + currentStateType;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        BotDetails = gameObject.AddComponent<DocBotDetails>(); // for the doc bot to read later
        agent = GetComponent<NavMeshAgent>();
        
        AddState("WANDER", new WanderState(this, 
            "WANDER", this));
        
        AddState("BROKEN", new BrokenState(this, 
            "BROKEN", this));
        
                
        AddState("DESTROYED", new DestroyedState(this, 
            "DESTROYED", this));
        
        AddState("MACHINE_REPAIR", new MachineRepairState(this, 
            "MACHINE_REPAIR", this));
        
        ChangeState("BROKEN"); // start at broken for the doc-bot to repair.


        
     
    }
    
    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    

       
            
      
    
}
