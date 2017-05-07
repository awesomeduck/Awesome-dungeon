using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public static class TextEventDefinitions
    {
        public static Dictionary<byte, TextEventDefinition> textEvents;

        static TextEventDefinitions()
        {            
            textEvents = new Dictionary<byte, TextEventDefinition>();
            textEvents.Add(1, new TextEventDefinition
            {
                Text = new string[] { "This is what text on a sign post looks like.",
                 "It can be read multiple times" },
                ReadPromptText = "There is a sign here. Would you like to read it?",               
                PromptApprovalText = "Read",
                ShowSignpost = true
            });

            textEvents.Add(2, new TextEventDefinition
            {
                Text = new string[] { "This is a one time story event. Once you close this you cannot view it again." },
                ViewOnce = true,                
            });

            textEvents.Add(3, new TextEventDefinition
            {
                Text = new string[] { "You're launched by the springboard." },
                ReadPromptText = "There is a spring board here, step on it?",
                PromptApprovalText = "Yes",
                Teleport = true,
                TeleportTarget = new Location { X = 9, Y = 8}
            });


        }
    }

    public class TextEventDefinition
    {
        public string[] Text { get; set; }
        // if the text event is not a sign post then this will be used to determine if the text can be triggered multiple times. 
        // a sign post text event is always reusable.
        public bool ViewOnce { get; set; }      
        // providing text here will prompt the user if they want to read the text or not. This will only appear for sign post text events.  
        public string ReadPromptText { get; set; }
        public string PromptApprovalText { get; set; }
        public bool ShowSignpost { get; set; }        
        public bool Teleport { get; set; }
        public Location TeleportTarget { get; set; }
    }
}