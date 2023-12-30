using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    [SerializeField] private List<Epilogues> allEpilogues;
    private List<Epilogues> chosenCouples;

    private int currentCoupleIndex = 0;

    public TMP_Text coupleName; 
    public TMP_Text outcomeText;
    public Image character1Image;
    public Image character2Image;
    [SerializeField] private Button nextCoupleButton;
    [SerializeField] private Button previousCoupleButton;

    void Start()
    {
        // Initialize chosenCouples
        chosenCouples = new List<Epilogues>();

        List<string> coupleNames = new List<string>
        {
            "Alex and Cory", "Cory and Alex",
            "Asaph and Skylar", "Skylar and Asaph",
            "Autumn and Rachel", "Rachel and Autumn",
            "Alex and Asaph", "Asaph and Alex",
            "Alex and Autumn", "Autumn and Alex",
            "Alex and Rachel", "Rachel and Alex",
            "Alex and Skylar", "Skylar and Alex",
            "Asaph and Autumn", "Autumn and Asaph",
            "Asaph and Cory", "Cory and Asaph",
            "Asaph and Rachel", "Rachel and Asaph",
            "Autumn and Cory", "Cory and Autumn",
            "Autumn and Skylar", "Skylar and Autumn",
            "Cory and Rachel", "Rachel and Cory",
            "Cory and Skylar", "Skylar and Cory",
            "Rachel and Skylar", "Skylar and Rachel"
        };

        foreach (var couple in coupleNames)
        {
            if (LoveSpellManager.Instance.specificCouple1 == couple
                || LoveSpellManager.Instance.specificCouple2 == couple
                || LoveSpellManager.Instance.specificCouple3 == couple)
            {
                // Find the corresponding scriptable object and add it to chosenCouples
                Epilogues epilogue = allEpilogues.Find(e => e.coupleName == couple);
                if (epilogue != null)
                {
                    chosenCouples.Add(epilogue);
                }
            }
        }

        DisplayCurrentEpilogue();
        nextCoupleButton.onClick.AddListener(ShowNextCouple);
        previousCoupleButton.onClick.AddListener(ShowPreviousCouple);
    }


    public void DisplayCurrentEpilogue()
    {
        if (chosenCouples.Count > 0)
        {
            currentCoupleIndex = Mathf.Clamp(currentCoupleIndex, 0, chosenCouples.Count - 1);
            Epilogues currentEpilogue = chosenCouples[currentCoupleIndex];
            coupleName.text = currentEpilogue.coupleName; 
            outcomeText.text = currentEpilogue.outcome;
            character1Image.sprite = currentEpilogue.character1;
            character2Image.sprite = currentEpilogue.character2;
        }
        else
        {
            Debug.LogError("No epilogues available.");
        }
    }


    public void ShowNextCouple()
    {
        currentCoupleIndex = (currentCoupleIndex + 1) % chosenCouples.Count;
        DisplayCurrentEpilogue();
    }

    public void ShowPreviousCouple()
    {
        currentCoupleIndex = (currentCoupleIndex - 1 + chosenCouples.Count) % chosenCouples.Count;
        DisplayCurrentEpilogue();
    }
}
