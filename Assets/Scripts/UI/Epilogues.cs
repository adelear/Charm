using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEpilogueData", menuName = "Epilogues")]
public class Epilogues : ScriptableObject
{
    public string coupleName; 
    public string outcome;
     
    public Sprite character1; 
    public Sprite character2; 
}
