// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Logan Kemper

using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorlds.Dialogue
{
    // To create a speaker library, add a new asset in the project window, then select "Speaker Library"
    // Then use the inspector to add new speakers and their sprites

    /// <summary>
    /// A scriptable object that is used to supply DialogueManager with dialogue speakers and their associated sprites.
    /// </summary>
    [CreateAssetMenu(fileName = "New Speaker Library", menuName = "Speaker Library")]
    public class SpeakerLibrary : ScriptableObject
    {
        [System.Serializable]
        public class SpeakerInfo
        {
            public string name;
            public Sprite sprite;
            public Color nameColor = Color.black;
            public Color textColor = Color.white;
        }

        public List<SpeakerInfo> speakerList = new();

        private void OnValidate()
        {
            foreach (var speaker in speakerList)
            {
                if (speaker.nameColor.a == 0)
                {
                    speaker.nameColor = Color.black;
                }

                if (speaker.textColor.a == 0)
                {
                    speaker.textColor = Color.white;
                }
            }
        }
    }
}