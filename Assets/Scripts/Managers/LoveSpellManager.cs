using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoveSpellManager : MonoBehaviour
{
    public static LoveSpellManager Instance;

    public List<NPCController> npcsUnderLoveSpell = new List<NPCController>();
    public List<LoveCouple> loveCouples = new List<LoveCouple>(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNPCUnderLoveSpell(NPCController npc)
    {
        if (!npcsUnderLoveSpell.Contains(npc))
        {
            npcsUnderLoveSpell.Add(npc);
        }
    }

    public void RemoveNPCUnderLoveSpell(NPCController npc)
    {
        npcsUnderLoveSpell.Remove(npc);
    }

    public void AddLoveCouple(NPCController npc1, NPCController npc2)
    {
        // Check if the couple already exists
        if (!IsCouple(npc1, npc2))
        {
            LoveCouple couple = new LoveCouple(npc1, npc2);
            loveCouples.Add(couple);
        }
    }

    public bool IsCouple(NPCController npc1, NPCController npc2)
    {
        string name1 = npc1.GetName();
        string name2 = npc2.GetName();

        return loveCouples.Any(couple =>
            (couple.npc1.GetName() == name1 && couple.npc2.GetName() == name2) ||
            (couple.npc1.GetName() == name2 && couple.npc2.GetName() == name1));
    }


    public bool IsNPCInAnyCouple(NPCController npc)
    {
        return loveCouples.Any(couple => couple.npc1 == npc || couple.npc2 == npc);
    }

    public void DisplayCouples()
    {
        foreach (var couple in loveCouples)
        {
            Debug.Log($"Couple: {couple.npc1.gameObject.name} and {couple.npc2.gameObject.name}");
            // Add your logic to display the couples in your game
        }
    }

    public void DisplaySpecificCouple(int coupleIndex)
    {
        // Check if the index is valid
        if (coupleIndex >= 0 && coupleIndex < loveCouples.Count)
        {
            LoveCouple couple = loveCouples[coupleIndex];

            // Display information about the couple
            Debug.Log("Couple: " + couple.npc1.GetName() + " and " + couple.npc2.GetName());
        }
        else
        {
            Debug.LogWarning("Invalid couple index: " + coupleIndex);
        }
    }
}
