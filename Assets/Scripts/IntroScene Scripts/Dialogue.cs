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

    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    public void OnDialogueNext()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
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
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
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
            gameObject.SetActive(false);
        }
    }
}