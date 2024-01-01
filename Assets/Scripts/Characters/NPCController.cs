using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using static System.Net.WebRequestMethods;

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
    private float speed = 1f;
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
    private Coroutine movementCoroutine;

    [Header("Interaction Buttons")]
    public GameObject interactButton; 
    public TMP_Text nameForButton; 

    private SpriteRenderer spriteRenderer; 

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
            if (GameManager.Instance.GetGameState() != GameManager.GameState.PAUSE)
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
                // Move towards the current waypoint
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

                
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
        StopMovementCoroutine();
        float speed = 1.0f; // Adjust the speed as needed
        float collisionCooldown = 2.0f; // Adjust the cooldown duration as needed

        float timeSinceLastCollision = 0f;

        while (Vector3.Distance(transform.position, midpoint) > 0.1f)
        {
            // Calculate the direction to the midpoint
            Vector3 directionToMidpoint = (midpoint - transform.position).normalized;

            // Check for obstacles in the path
            if (Physics.Raycast(transform.position, directionToMidpoint, out RaycastHit hit, 1.0f))
            {
                if ((hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("NPC")) && hit.collider.gameObject != partner.gameObject)
                {
                    // Check if enough time has passed since the last collision
                    if (Time.time - timeSinceLastCollision > collisionCooldown)
                    {
                        // Calculate a new direction that avoids the obstacle
                        Vector3 avoidanceDirection = Vector3.Slerp(directionToMidpoint, Vector3.Cross(Vector3.up, hit.normal).normalized, 0.5f);

                        // Move towards the adjusted direction
                        transform.position = Vector3.MoveTowards(transform.position, transform.position + avoidanceDirection, speed * Time.deltaTime);
                        partner.transform.position = Vector3.MoveTowards(partner.transform.position, partner.transform.position - avoidanceDirection, speed * Time.deltaTime);

                        // Update the time of the last collision
                        timeSinceLastCollision = Time.time;
                    }
                }
                else if (hit.collider.gameObject == partner.gameObject)
                {
                    speed = 0f; 
                    partner.speed = 0f; 
                }
            }
            else
            {
                // No obstacle, move directly towards the midpoint
                transform.position = Vector3.MoveTowards(transform.position, transform.position + directionToMidpoint, speed * Time.deltaTime);
                partner.transform.position = Vector3.MoveTowards(partner.transform.position, partner.transform.position - directionToMidpoint, speed * Time.deltaTime);

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
                foreach (var collider in hitColliders)
                {
                    if (collider.gameObject == partner.gameObject)
                    {
                        partner.speed = 0f;
                        speed = 0f;
                    }
                } 

            }

            yield return null;
        }
    }


    private void OnMouseEnter()
    {
        isMouseHovering = true;
        speed = 0; 
        StopMovementCoroutine();
        if (interactButton != null)
        {
            if (characterProfile != null && !characterProfile.activeSelf)
            {
                if (introDialogue != null && !introDialogue.activeSelf)
                {
                    if (outroDialogue != null && !outroDialogue.activeSelf)
                    {
                        if (epilogues != null && !epilogues.activeSelf)
                        {
                            interactButton.SetActive(true);
                            nameForButton.text = "Press E to observe " + characterData.name;
                        }
                    }
                }
            }
            
        } 
    }

    private void OnMouseExit()
    {
        isMouseHovering = false;
        speed = 1; 
        StartMovementCoroutine(); 
        interactButton.SetActive(false);
    }

    private void ShowProfile()
    {
        interactButton.SetActive(false); 
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
    private void StartMovementCoroutine()
    {
        if (movementCoroutine == null)
        {
            // Start the movement coroutine if it's not already running
            movementCoroutine = StartCoroutine(MoveToNextWaypoint());
        }
    }

    private void StopMovementCoroutine()
    {
        if (movementCoroutine != null)
        {
            // Stop the movement coroutine if it's running
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }


    public string GetName()
    {
        return gameObject.name; 
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        CheckReferences(); 
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = 0;
            StartCoroutine(MoveToNextWaypoint());
        }
        else
        {
            Debug.LogError("No patrol waypoints assigned to the NPC.");
        }

        SetCharacterData(characterData);
    }


    void Update()
    {
        // Add the following line to make sure SetLovePartner is called continuously
        if (inLove && !isTaken && lovePartner == null && !LoveSpellManager.Instance.IsNPCInAnyCouple(this))
        {
            SetLovePartner();
        }

        if (GameManager.Instance.GetGameState() == GameManager.GameState.PAUSE || isMouseHovering) 
        {
            StopMovementCoroutine();
        }
        else
        {
            StartMovementCoroutine();
        }



        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isMouseHovering && GameManager.Instance.GetGameState() != GameManager.GameState.PAUSE)
            {
                ShowProfile(); 
            }
        }

        // IF another npc above, spriterenderer is on layer 6
        // if another npc is below them, spriterenderer on layer 5

        RaycastHit2D hitUp;
        RaycastHit2D hitDown;

        int npcLayerMask = LayerMask.GetMask("NPCLayer");
        float hitRadius = 3f;

        hitUp = Physics2D.CircleCast(transform.position, hitRadius, transform.up, 0f, npcLayerMask);
        hitDown = Physics2D.CircleCast(transform.position, hitRadius, -transform.up, 0f, npcLayerMask);

        bool isAbove = (hitUp.collider != null && hitUp.collider.CompareTag("NPC") && hitUp.collider.gameObject != gameObject) || (hitUp.collider != null && hitUp.collider.CompareTag("Player"));
        bool isBelow = hitDown.collider != null && hitDown.collider.CompareTag("NPC") && hitDown.collider.gameObject != gameObject || (hitDown.collider != null && hitDown.collider.CompareTag("Player"));


        Debug.DrawRay(transform.position, Vector2.up * hitRadius, Color.red);  // Debug line for the up ray
        Debug.DrawRay(transform.position, Vector2.down * hitRadius, Color.blue);  // Debug line for the down ray

        if (isAbove)
        {
            Debug.Log(hitUp.collider.gameObject.name + " Is above " + gameObject.name);
            if (spriteRenderer != null) spriteRenderer.sortingOrder = 6;
            SpriteRenderer otherSpriteRenderer1 = hitUp.collider.GetComponent<SpriteRenderer>();
            if (otherSpriteRenderer1 != null) otherSpriteRenderer1.sortingOrder = 5;
        }
        else if (isBelow)
        {
            Debug.Log(hitDown.collider.gameObject.name + " Is below " + gameObject.name);
            if (spriteRenderer != null) spriteRenderer.sortingOrder = 4;
            SpriteRenderer otherSpriteRenderer = hitDown.collider.GetComponent<SpriteRenderer>();
            if (otherSpriteRenderer != null) otherSpriteRenderer.sortingOrder = 5;
        }

    }
}
