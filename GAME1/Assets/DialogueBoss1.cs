using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBoss1 : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;

    public bool isOpen { get; private set; }
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
        showDialogue(testDialogue);
    }

    public void showDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, textLabel);
            yield return new WaitForSeconds(1);
        }

        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        DialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
