using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPCController npc = collision.gameObject.GetComponent<NPCController>();
            if (npc != null && !npc.inLove)
            {
                Debug.Log(npc.gameObject.name + " shot by Love Arrow");
                npc.FallUnderLoveSpell();
                Destroy(gameObject); 
            }
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPCController npc = collision.gameObject.GetComponent<NPCController>();
            if (npc != null && !npc.inLove)
            {
                Debug.Log(npc.gameObject.name + " shot by Love Arrow");
                npc.FallUnderLoveSpell();
                Destroy(gameObject); 
            }
        }
        
    }
}

