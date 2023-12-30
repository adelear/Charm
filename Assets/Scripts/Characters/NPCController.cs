using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class NPCController : MonoBehaviour
{
    [SerializeField] ProfileManager profileManager;
    [SerializeField] private CharacterData characterData;
    [SerializeField] private GameObject characterProfile;
    [SerializeField] private GameObject introDialogue;
    [SerializeField] private GameObject outroDialogue;
    [SerializeField] private GameObject epilogues; 

    [Header("Navigation")]
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex;
    private int direction = 1; // 1 for forward, -1 for backward 
    public bool isWaiting = false; 
    public bool isMouseHovering = false;

    [Header("Character Profile Elements")]
    public TMP_Text characterName;
    public TMP_Text characterBackstory;
    public Image characterPortrait;
    public string loveInterest; 

    [Header("Relationship Profile Elements")]
    public TMP_Text character1;
    public TMP_Text character2;
    public TMP_Text character3;
    public TMP_Text character4;
    public TMP_Text character5;

    public TMP_Text relationship1;
    public TMP_Text relationship2;
    public TMP_Text relationship3;
    public TMP_Text relationship4;
    public TMP_Text relationship5;

    [Header("Love Spell")]
    public bool inLove = false;
    public bool isTaken = false;
    private NPCController lovePartner;

    private void Awake()
    {
        CheckReferences();
    }

    private void CheckReferences()
    {
        if (profileManager == null)
        {
            if (profileManager == null)
            {
                Debug.LogError("ProfileManager is not assigned on " + gameObject.name);
            }

            if (characterData == null)
            {
                Debug.LogError("CharacterData is not assigned on " + gameObject.name);
            }

            if (characterProfile == null)
            {
                Debug.LogError("CharacterProfile is not assigned on " + gameObject.name);
            }
        }
    }
    public void SetCharacterData(CharacterData data)
    {
        if (data != null)
        {
            characterName.text = data.name;
            characterBackstory.text = data.backstory;
            characterPortrait.sprite = data.portrait;
            loveInterest = data.loveInterest;

            character1.text = "Regarding " + data.char1;
            character2.text = "Regarding " + data.char2; 
            character3.text = "Regarding " + data.char3;
            character4.text = "Regarding " + data.char4;
            character5.text = "Regarding " + data.char5;

            relationship1.text = data.relationship1;
            relationship2.text = data.relationship2;
            relationship3.text = data.relationship3;
            relationship4.text = data.relationship4;
            relationship5.text = data.relationship5;
        }
        else
        {
            Debug.LogError("CharacterData is null. Make sure to assign it in the Unity Editor.");
        }
        UpdateDisplay(); 
    }

    private void UpdateDisplay()
    {
        characterName.text = characterData.name;
        characterBackstory.text = characterData.backstory;
        characterPortrait.sprite = characterData.portrait;
        loveInterest = characterData.loveInterest;

        character1.text = "Regarding " + characterData.char1;
        character2.text = "Regarding " + characterData.char2;
        character3.text = "Regarding " + characterData.char3;
        character4.text = "Regarding " + characterData.char4;
        character5.text = "Regarding " + characterData.char5;

        relationship1.text = characterData.relationship1;
        relationship2.text = characterData.relationship2;
        relationship3.text = characterData.relationship3;
        relationship4.text = characterData.relationship4;
        relationship5.text = characterData.relationship5;
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (!isTaken)
        {
            // checking if npc has reached the current target waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                isWaiting = true;
                yield return new WaitForSeconds(2.0f);
                // After waiting, move to the next waypoint
                isWaiting = false;
                currentWaypointIndex += direction;

                // checking if npc has reached the last or first waypoint, then change direction
                if (currentWaypointIndex >= waypoints.Length || currentWaypointIndex < 0)
                {
                    direction *= -1;
                    currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Length - 1);
                }
            }

            yield return null;
        }
    }

    private IEnumerator DelayedSetLovePartner()
    {
        yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
        SetLovePartner();
    }

    private void SetLovePartner()
    {
        // look for another NPC that is in love, not taken, and does not have a partner
        NPCController partner = LoveSpellManager.Instance.npcsUnderLoveSpell
            .FirstOrDefault(npc => npc != this && npc.inLove && !npc.isTaken && npc.lovePartner == null);

        if (partner != null)
        {
            // Both NPCs are in love, call GetTaken for both
            GetTaken(partner);
            partner.GetTaken(this); // making sure the partner knows omgg im taken slay

            // Add the couple to the LoveSpellManager
            LoveSpellManager.Instance.AddLoveCouple(this, partner);
        }
    }


    public void FallUnderLoveSpell()
    {
        if (!inLove)
        {
            inLove = true;
            // Play Love spell VFX 
            // Play Love spell SFX

            // Add this NPC to the list of NPCs under the love spell
            LoveSpellManager.Instance.AddNPCUnderLoveSpell(this);

            // Set this NPC as the love partner for another NPC after a slight delay
            StartCoroutine(DelayedSetLovePartner());
        }
    }


    public void GetTaken(NPCController partner)
    {
        if (!isTaken && partner != null)
        {
            isTaken = true;
            Debug.Log(gameObject.name + " Taken by " + partner.gameObject.name);

            // Play Love spell VFX 
            // Play Love spell SFX 

            // Stop the NPC from moving to waypoints
            StopCoroutine(MoveToNextWaypoint());

            // Find the midpoint between the two NPCs
            Vector3 midpoint = (transform.position + partner.transform.position) / 2.0f;

            // Move both NPCs towards the midpoint
            StartCoroutine(MoveToMidpoint(midpoint, partner));

            // Add the couple to LoveSpellManager
            LoveSpellManager.Instance.AddLoveCouple(this, partner);
        }
        else
        {
            Debug.LogWarning("GetTaken: Partner is null or already taken.");
        }
    }
     


    private IEnumerator MoveToMidpoint(Vector3 midpoint, NPCController partner)
    {
        float moveSpeed = 2.0f; // Adjust the speed as needed

        while (Vector3.Distance(transform.position, midpoint) > 0.1f)
        {
            // Move towards the midpoint
            transform.position = Vector3.MoveTowards(transform.position, midpoint, moveSpeed * Time.deltaTime);
            partner.transform.position = Vector3.MoveTowards(partner.transform.position, midpoint, moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        if (characterProfile != null && !characterProfile.activeSelf)
        {
            if (introDialogue != null && !introDialogue.activeSelf)
            {
                if (outroDialogue != null && !outroDialogue.activeSelf)
                {
                    if (epilogues != null && !epilogues.activeSelf)
                    {
                        characterProfile.SetActive(true);
                        SetCharacterData(characterData);
                        isMouseHovering = true;

                        if (profileManager != null)
                        {
                            profileManager.SetPage(0);
                            profileManager.ShowProfile();
                        }
                        else
                        {
                            Debug.Log("Profile Manager does not exist");
                        }
                    }
                } 
            }
            else
            {
                Debug.Log("Intro Dialogue is active");
            }
        }
    }


    public string GetName()
    {
        return gameObject.name; 
    }

    private void Start()
    {
        CheckReferences(); 
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = 0;
        }
        else
        {
            Debug.LogError("No patrol waypoints assigned to the NPC.");
        }

        // Start the movement coroutine
        //StartCoroutine(MoveToNextWaypoint());
        SetCharacterData(characterData);
    }

    void Update()
    {
        if (characterProfile.activeSelf)
        {
            isMouseHovering = true;
        }
        else
        {
            isMouseHovering = false;
        }

        // Add the following line to make sure SetLovePartner is called continuously
        if (inLove && !isTaken && lovePartner == null && !LoveSpellManager.Instance.IsNPCInAnyCouple(this))
        {
            SetLovePartner();
        }
    }

}
