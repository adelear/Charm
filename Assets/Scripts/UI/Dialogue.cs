using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;
    public bool isOutro = false;
    public AudioClip dialogueSound;
    [SerializeField] AudioManager audioManager; 
    

    private int index;
    private Coroutine typingCoroutine;

    [SerializeField] GameObject epilogues; 
    private void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        StartTyping();
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty; // Clear text before typing
        bool playSound = false; // Flag to determine whether to play sound for the current letter
        foreach (char c in lines[index].ToCharArray())
        {
            if (playSound)
            {
                audioManager.PlayOneShot(dialogueSound, false); // Play dialogue sound for every other letter
            }
            textComponent.text += c;
            playSound = !playSound; // Toggle the flag
            yield return new WaitForSeconds(textSpeed);
        } 
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartTyping();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if (isOutro)
        {
            gameObject.SetActive(false); 
            if (epilogues != null) epilogues.SetActive(true); 
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /*
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameOver");
        gameObject.SetActive(false);
    }
    */ 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
}
