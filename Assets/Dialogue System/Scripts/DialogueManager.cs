using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private DialogueSettingsSO _dataSettings;

    private DialogueSO _data = null;
    private DialoguePanel _panel = null;
    private Coroutine _dialogueCoroutine = null;
    private DialogueTypes _dialogueType;

    private int _dialogueLine = 0;
    private int _currentLine = 0;
    private int _selectedButtonIndex = 0;
    private bool _isTalking = false;
    private bool _showTalkerName = false;
    private bool _jumpDialogueIndex = false;

    public DialoguePanel DialoguePanel { get => _panel; set => _panel = value; }
    public bool IsTalking => _isTalking;

    public static DialogueManager Instance { get; private set; }
    #endregion

    #region Events
    public static event System.Action OnAnswerSelected;
    #endregion


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _showTalkerName = _dataSettings.ShowTalkerName;
    }

    public void StartDialogues(DialogueTypes type)
    {
        _data = null;
        _data = _dataSettings.DialogueData.Find(x => x.Type == type).Dialogue;
        _dialogueType = type;

        if (_data == null)
        {
            Debug.LogWarning("Selected data not found!");
            return;
        }

        if (_dialogueCoroutine != null)
            StopCoroutine(_dialogueCoroutine);

        _isTalking = true;
        _panel.DialoguePanelActivity(true);

        if (_dialogueLine < _data.Dialogues.Count)
        {
            var currentDialogue = _data.Dialogues[_dialogueLine].Sentences[_currentLine];
            _dialogueCoroutine = StartCoroutine(PrintDialogues(currentDialogue));
        }
        else if (_data.Dialogues[^1].Loop)
        {
            _dialogueLine = _data.Dialogues.Count - 1;
            _currentLine = 0;
            var currentDialogue = _data.Dialogues[_dialogueLine].Sentences[_currentLine];
            _dialogueCoroutine = StartCoroutine(PrintDialogues(currentDialogue));
        }
    }

    public void StopDialogues()
    {
        if (_dialogueCoroutine != null)
            StopCoroutine(_dialogueCoroutine);
    }

    public void CalculateNxtDialogueIndex(int totalButtonValue)
    {
        _selectedButtonIndex = Mathf.Abs(_selectedButtonIndex - totalButtonValue);
        _jumpDialogueIndex = true;
    }

    public void ApplyChoice(int dialogueIndex, DialogueTypes dialogueType)
    {
        OnAnswerSelected.Invoke();
        _selectedButtonIndex = dialogueIndex;
        SkipToNextDialogue(dialogueIndex);
        StartCoroutine(ApplyChoice(dialogueType));
    }

    private IEnumerator ApplyChoice(DialogueTypes dialogueType)
    {
        yield return new WaitForSeconds(_dataSettings.WaitTimeAfterAnswer);
        StartDialogues(dialogueType);
        _panel.AnswerPanelActivity(false);
    }

    private void SkipToNextDialogue(int dialogueIndex = 1)
    {
        _dialogueLine += dialogueIndex;
        _currentLine = 0;
    }

    private IEnumerator PrintDialogues(SentenceVariables currentDialogue)
    {
        float standbyTime = currentDialogue.Type == SentenceTypes.Answer ? 0 :
            currentDialogue.StandbyTime > 0 ? currentDialogue.StandbyTime :
            GetStandbyTime(currentDialogue.Text);

        _panel.ShowDialog(
            GetTalkerName(currentDialogue.Talker),
            currentDialogue.Text,
            currentDialogue.Type,
            _showTalkerName,
            currentDialogue.StandbyTimeAfterSentence);

        yield return new WaitForSeconds(standbyTime);
        if (currentDialogue.StandbyTimeAfterSentence > 0 && currentDialogue.Type != SentenceTypes.Answer)
        {
            _panel.DialoguePanelActivity(false);
            yield return new WaitForSeconds(currentDialogue.StandbyTimeAfterSentence);
            _panel.DialoguePanelActivity(true);
        }

        _currentLine++;

        if (_currentLine >= _data.Dialogues[_dialogueLine].Sentences.Count)
        {
            if (_jumpDialogueIndex)
            {
                _jumpDialogueIndex = false;
                SkipToNextDialogue(_selectedButtonIndex);
            }
            else
                SkipToNextDialogue();

            if (currentDialogue.Type != SentenceTypes.Answer)
            {
                if (_dialogueLine < _data.Dialogues.Count)
                {
                    var nextCurrentDialogue = _data.Dialogues[_dialogueLine].Sentences[_currentLine];
                    StartCoroutine(PrintDialogues(nextCurrentDialogue));
                }
                else
                {
                    _isTalking = false;
                    _panel.DialoguePanelActivity(false);
                }
            }
        }
        else
        {
            if (_dialogueLine <= _data.Dialogues.Count)
            {
                currentDialogue = _data.Dialogues[_dialogueLine].Sentences[_currentLine];
                StartCoroutine(PrintDialogues(currentDialogue));
            }
        }
    }

    private string GetTalkerName(TalkerTypes type)
    {
        string talkerName = _dataSettings.Talkers.Find(x => x.Type == type)?.Name;
        return talkerName != null ? talkerName : type.ToString();
    }

    private float GetStandbyTime(string text)
    {
        int wordValue = text.Split(' ').Length - 1;
        return 1.5f + wordValue * _dataSettings.DefaultStandbyTimeRate;
    }
}

// ed 20.05.23