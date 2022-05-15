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
            docBotsAlive = 0;
            foreach (DocBotFSM doc in GameObject.FindObjectsOfType<DocBotFSM>())
            {
                if (!doc.GetCurrentStateName().Equals("BROKEN") && !doc.GetCurrentStateName().Equals("DESTROYED"))
                {
                    docBotsAlive += 1;
                }
            }
        }
    }
}


