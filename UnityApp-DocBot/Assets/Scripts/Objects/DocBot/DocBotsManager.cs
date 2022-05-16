using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.DocBot // PROPER HIERARCHY
{
    
    public class DocBotsManager : MonoBehaviour // manages all doc bots, essentially used to view how many doc bots are alive on ythe field.
    {


        public int docBotsAlive = 0;
        public static DocBotsManager Instance { get; set; }

        private void Awake()
        {
            // singleton
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void Update()
        {
            try // try this code
            {
                docBotsAlive = 0;
                foreach (DocBotFSM doc in GameObject.FindObjectsOfType<DocBotFSM>()) // search all objects on the scene that is a doc bot
                {
                    if (!doc.GetCurrentStateName().Equals("BROKEN") && !doc.GetCurrentStateName().Equals("DESTROYED")) // if the doc bot is not in either broken or destroy
                    {
                        docBotsAlive += 1; // we + 1 to alive doc bots as its alive.
                    }
                }
            }
            catch (Exception e)
            {
                 // catch an error without stopping the game.
            }
           
        }
    }
}


