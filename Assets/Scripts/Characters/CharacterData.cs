using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Characters")] 
public class CharacterData : ScriptableObject
{
    public new string name;
    public string backstory;
    public string loveInterest;

    public Sprite portrait;

    public string char1;
    public string char2;
    public string char3;
    public string char4;
    public string char5;

    public string relationship1; 
    public string relationship2;
    public string relationship3;
    public string relationship4;
    public string relationship5;    
}
 