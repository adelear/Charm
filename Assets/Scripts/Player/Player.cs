using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject characterProfile; 
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
        if (characterProfile != null)
        {
            if (!characterProfile.activeSelf)
            {
                mx = Input.GetAxisRaw("Horizontal");
                my = Input.GetAxisRaw("Vertical");
                RotatePlayerTowardsCursor();
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
        
        //if (Input.GetKeyDown(KeyCode.E)) LoveSpellManager.Instance.DisplayCouples();
    }
    private void FixedUpdate()
    {
        if (!characterProfile.activeSelf)
        {
            rb.velocity = new Vector2(mx, my).normalized * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
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
