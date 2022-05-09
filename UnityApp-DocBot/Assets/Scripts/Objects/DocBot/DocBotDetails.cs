using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Objects.DocBot // PROPER HIERARCHY
{
    
    [Serializable] // so we can edit in the inspector and gives a new instnce without us needing to specify = new DocBotDetils();
    public class DocBotDetails // Going to be an Object Class that is initialized by each DocBotFSM to store individual doc bot details
    {




        public bool isTended; // is it tended by another bot if its broken?


        public DocBotHardware docBotHardware = new DocBotHardware();



        public class DocBotHardware // the internal of the doc bot, the hardware.
        {
            private Dictionary<String, bool> _hardwareStatuses = new Dictionary<string, bool>();

            public bool totalFailure = false; // whether the bot is a total failure when it breaks down.


            private string[] booleans = {"true", "false" }; // to store true and false as 0 and 1, so we can
            // random range it so sometimes its true, sometimes its false using Boolean.Parse(str)


            // a variety of issues possible to happen when it breaks down.
            private string[] hardwareFailures =
            {
                "battery", "system_error",
                "mother_board", "motor", "display_lcd"
            };

            public void OnBreakDown() // on bot break down event
            {

                if (Random.Range(0, 100) < 20) //20% chance to have a total failure resulting in an unfixable bot.
                {
                    totalFailure = true; // total failure.
                    
                    return;
                }
                
                
                // each hardware breakdown will randomize between 0 and 1, thus getting from the booleans string array above.
                // and being converted into a boolean.

                foreach (string failureName in hardwareFailures) // loop through all the hardware failures array to add
                {

                    bool status = Boolean.Parse(booleans[Random.Range(0, 2)]);
                    _hardwareStatuses.Add(failureName, status);
                    // randomize between true and false as the value, hardware as the key


                    if (!status) // if specific hardware break down, we print to keep track
                    {
                        Debug.Log(failureName + " has broke down!");
                    }
                }
               
          
  
                    
             
            }

        }
    

    }
}


