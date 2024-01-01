using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject characterProfile;
    [SerializeField] GameObject introDialogue;
    [SerializeField] GameObject outroDialogue;
    [SerializeField] GameObject epilogues; 
    private Rigidbody2D rb;
    private float speed = 5.0f;
    private float mx;
    private float my; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        if (characterProfile != null || introDialogue !=null || outroDialogue != null)
        {
            if (!characterProfile.activeSelf || !introDialogue.activeSelf || !outroDialogue.activeSelf) 
            {
                if (GameManager.Instance.GetGameState() != GameManager.GameState.PAUSE)
                {
                    mx = Input.GetAxisRaw("Horizontal");
                    my = Input.GetAxisRaw("Vertical");
                    RotatePlayerTowardsCursor();
                }
            }
            else
            {
                rb.velocity = Vector3.zero; 
            }
        }
        else
        {
            Debug.Log("Character Profile not set!"); 
        }

        if (epilogues.activeSelf)
        {

            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.PAUSE) 
        {
            if (!characterProfile.activeSelf || !introDialogue.activeSelf)
            {
                rb.velocity = new Vector2(mx, my).normalized * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        
    }

    private void RotatePlayerTowardsCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
