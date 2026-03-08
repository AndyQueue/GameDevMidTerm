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

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogWarning("LevelButton: No LevelManager found in the scene.");
        }
        textComponent.text = string.Empty;



        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(2.5f);
        index = 0;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    public void OnDialogueNext()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueTypingSound.Stop();
            textComponent.text = string.Empty;
            textComponent.text = lines[index];
            isTyping = false;
        }
        else
        {
            NextLine();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
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