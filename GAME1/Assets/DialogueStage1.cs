using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueStage1 : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;

    public bool isOpen { get; private set; }
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        showDialogue(testDialogue);
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
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
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
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
