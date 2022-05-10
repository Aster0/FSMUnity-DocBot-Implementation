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
        public DocBotSupplies docBotSupplies = new DocBotSupplies();

        public class DocBotSupplies // the supplies of each doc bot is saved here.
        {
            private const int MAX_AMOUNT_OF_SUPPLIES = 5, START_AMOUNT_OF_SUPPLIES = 0;
            
            private Dictionary<String, int> supplies = new Dictionary<string, int>(); // store the supplies details
            
            private string[] suppliesName = // all the supplies the doc bot will have access to
            {
                "battery", "mother_board", "motor", "display_lcd"
            };


            public void StartAmountResupply()
            {
                Resupply(START_AMOUNT_OF_SUPPLIES);
            }
            
            public void FullResupply()
            {
                Resupply(MAX_AMOUNT_OF_SUPPLIES);
            }
            private void Resupply(int amount)
            {
                foreach (string supplyName in suppliesName) // loop all hardware components 
                {
                    supplies[supplyName] = amount; // to resupply the max amount. 
                }
            }

            // false = ran out
            // true = still in stock
            public bool CheckSupply(string supplyName)
            {

                try // try the block below
                {
                    if (supplies[supplyName] == 0)
                    {
                        return false;
                    }
                }
                catch (KeyNotFoundException e) // catch the error
                                               // because those supplies not listed in resupply
                                               // like battery or system_error cause we don't replace those.
                                               // and is not found in the dictionary.
                {
                   // do nothing
                }
           
                
            

                return true;
            }

            public void UseSupply(string supplyName)
            {
                try // try the block below
                {
                    supplies[supplyName] -= 1; // minus one from the total supplies left.

                }
                catch (KeyNotFoundException e) // catch the error
                    // because those supplies not listed in resupply
                    // like battery or system_error cause we don't replace those.
                    // and is not found in the dictionary.
                {
                    // do nothing
                }
            }
            
        }

        public class DocBotHardware // the internal of the doc bot, the hardware.
        {
            private float batteryPercentage = 100; // battery %% that the bot is left with.
            
            private Dictionary<String, bool> _hardwareStatuses = new Dictionary<string, bool>();
            
            private Dictionary<String, String> _determinedHardwareFailures = new Dictionary<String, String>();
            // to store all the hardware failures to repair.

            public bool totalFailure = false; // whether the bot is a total failure when it breaks down.


            private string[] booleans = {"true", "false" }; // to store true and false as 0 and 1, so we can
            // random range it so sometimes its true, sometimes its false using Boolean.Parse(str)


            // a variety of issues possible to happen when it breaks down.
            private string[] hardwareFailures =
            {
                "battery", "system_error",
                "mother_board", "motor", "display_lcd"
            };

            // a variety of repair messages possible to happen when it gets repaired.
            private string[] hardwareRepairMessages =
            {
                "%targeted_botname%'s battery is at 0%... %botname% is now bringing this bot to the charging station..",
                "%targeted_botname%'s system has an error.. %botname% is now restarting the system..",
                "%targeted_botname%'s mother board is fried. %botname% is now replacing it now..",
                "%targeted_botname%'s motor isn't spinning.. %botname% is now replacing the motor now..", 
                "%targeted_botname%'s display LCD is cracked.. %botname% is now replacing the display LCD now.."
            };


            public float BatteryReduce(float timeDelta) // reduce battery when the bot is in use. 
            {
                batteryPercentage -= (timeDelta / Random.Range(1, 5)) * 2; 
                
     

                return batteryPercentage;
                // timeDelta to make it so it goes along with each update frame.
                // random from 1 to 10, to multiply like a multiplier (how fast it actually reduces each frame)



            }

            public float BatteryCharge(float timeDelta)
            {
                batteryPercentage += Random.Range(15, 30) * timeDelta; 
                
             

                return batteryPercentage;

                // more than reduce because charging should be faster
                // timeDelta to make it so it goes along with each update frame.
                // random from 15 to 30, to multiply like a multiplier (how fast it actually charges each frame)
            }
            
            
            

            public void OnBreakDown() // on bot break down event
            {

                
                if (Random.Range(0, 100) < 20) //20% chance to have a total failure resulting in an unfixable bot.
                {
                    totalFailure = true; // total failure.
                }

                
                _hardwareStatuses.Clear();

                int countTrue = 0;
                
                // each hardware breakdown will randomize between 0 and 1, thus getting from the booleans string array above.
                // and being converted into a boolean.
                
                // true = working hardware
                // false = not working hardware

                foreach (string failureName in hardwareFailures) // loop through all the hardware failures array to add
                {

                    bool status = Boolean.Parse(booleans[Random.Range(0, 2)]);
                    _hardwareStatuses.Add(failureName, status);
                    // randomize between true and false as the value, hardware as the key

                    if (status)
                        countTrue++; // count the number of trues (no hardware failures)

               
                    if (countTrue == hardwareFailures.Length) // if totally no hardware failures, we'll make one hardware failure 100%
                    {
                        _hardwareStatuses["system_error"] = false; // force system error to be false
                        // this situation is very unlikely, however, sometimes the Random.Range makes it so everything is true. 
                    }

                }
                
  
               
          
  
                    
             
            }

            // #DiagnoseIssue is for the tending bot to see what is the issue with the current bot.
            public void DiagnoseIssue(string botName, string targetedBotName, DocBotFSM.DocBotTypes state)
            {


                string allFailures = "";


                _determinedHardwareFailures.Clear();
                
                int count = 0;
                foreach (string key in _hardwareStatuses.Keys) // loop through all the hardware statuses
                {
                    if (!_hardwareStatuses[key]) // find which hardware status is false
                    {
                        _determinedHardwareFailures.Add(key, hardwareRepairMessages[count]); // we add it in a failure list.
                        // we also save the repair messages. 

                        allFailures += ", "+ key;
                    }

                    count++;
                }
                
                Debug.Log(botName + " "+ state + ": " + targetedBotName + " has these hardware that failed: " + allFailures );
                
            }
            
            // #RepairIssues is for the tending bot to repair the broken bot's issues.
            // @return false if the repair issues are too severe and can't be fixed. true returns when it can be fixed.
            public bool RepairIssues(string botName, string targetedBot, DocBotFSM.DocBotTypes state, DocBotFSM fsm)
            {
                bool outOfStock = false;
            
                if (OnTotalFailure(totalFailure))
                {
                    return false; // false means repair failed.
                }

                foreach (string key in _determinedHardwareFailures.Keys)
                {

                    if (!fsm.docBotDetails.docBotSupplies.CheckSupply(key)) // check if we have stock to repair
                    {
                        // if no stock, we
                        // resupply.
                        
                        fsm.stateManager.ChangeState(DocBotFSM.DocBotTypes.RETURN_SUPPLY);

                        outOfStock = true;
                        break; // break out of iteration
                        
                        
                    }


                    fsm.docBotDetails.docBotSupplies.UseSupply(key); // use 1 of the supply.
                    
                    
                    // for every repair, the bot might mess up the repair and make it
                    // totally unrepairable in the future (total failure)
                    // 5% chance for now

                    if (Random.Range(0, 100) < 5)
                    {
                        totalFailure = true; 
                        
                        
                    }
                    
                    
                    
                    Debug.Log( state + ": " + _determinedHardwareFailures[key].Replace(
                        "%botname%", botName)
                        .Replace("%targeted_botname%", targetedBot)); // print the repair messages
                                                    // replacing t he placeholder with the repairing botname's name.

                    _hardwareStatuses[key] = true; // set the hardware status to true because it's been fixed. 
                    
                }
                
                if (OnTotalFailure(totalFailure)) // cheeck if total failure again after doing repairs.
                {
                    return false; // false means repair failed.
                }


                

                return !outOfStock; // determine success.
                // if outOfStock is true, negated to false means repair has failed.
                // if outOfStock is false, negated to true. means repair is a success
            }
            
            private bool OnTotalFailure(bool totalFailure)
            {
                if (totalFailure) // can't be fixed.
                {
                    Debug.Log("Can't be fixed, scrapping now.");
                    return true; // so return true.
                }

                return false;
            }

        }



    

    }
}


