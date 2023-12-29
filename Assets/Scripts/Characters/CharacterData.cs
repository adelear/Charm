using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Characters")] 
public class CharacterData : ScriptableObject
{
    public new string name;
    public string backstory;
    //public string loveInterest;

    public Sprite portrait; 
}
 