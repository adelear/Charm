using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        RotatePlayerTowardsCursor(); 

        if (Input.GetKeyDown(KeyCode.E)) LoveSpellManager.Instance.DisplayCouples();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(mx, my).normalized * speed; 
    }

    private void RotatePlayerTowardsCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
