using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoveCouple
{
    public NPCController npc1;
    public NPCController npc2;

    public LoveCouple(NPCController npc1, NPCController npc2)
    {
        this.npc1 = npc1;
        this.npc2 = npc2;
    }
} 
