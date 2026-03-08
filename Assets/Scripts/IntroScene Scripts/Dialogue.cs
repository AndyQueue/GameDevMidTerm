using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

// Source: copied with some adaptations from https://www.youtube.com/watch?v=8oTYabhj248

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    [SerializeField] private AudioSource doorEnterSound;
    [SerializeField] private AudioSource dialogueTypingSound;


    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;

    private LevelManager levelManager;

    void Awake()
    {
        textComponent.text = string.Empty;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogWarning("LevelButton: No LevelManager found in the scene.");
        }

        index = 0;
        isTyping = false;
        textComponent.text = string.Empty;

       if (typingCoroutine != null) 
       {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
       }

       StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        bool previousTypingState = isTyping;
        isTyping= true;

        yield return new WaitForSeconds(2.5f);

        isTyping = false;
        index = 0;
        textComponent.text = string.Empty;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    public void OnDialogueNext()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            dialogueTypingSound.Stop();

            //textComponent.text = string.Empty;
            isTyping = false;
            textComponent.text = lines[index];
        }
        else
        {
            NextLine();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;//
        dialogueTypingSound.Play();
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        dialogueTypingSound.Stop();
        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("End of dialogue reached, loading next scene");
            levelManager.LoadMainMenu();
        }
    }
}