using UnityEngine;

public class DemoTalking : MonoBehaviour
{
    public GameObject PressEIcon;
    public DialogueTypes DialogueTypes;

    private DialogueManager dialogueManager;


    private void Start()
    {
        dialogueManager = DialogueManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.IsTalking)
        {
            PressEIcon.SetActive(false);
            dialogueManager.StartDialogues(DialogueTypes);
        }

        if (!PressEIcon.activeSelf && !dialogueManager.IsTalking)
        {
            PressEIcon.SetActive(true);
        }
    }

}
