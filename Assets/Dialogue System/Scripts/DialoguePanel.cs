using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private Text _dialogueText;
    [SerializeField] private GameObject _timerBar;
    [SerializeField] private GameObject _leftTimer;
    [SerializeField] private GameObject _rightTimer;
    [SerializeField] private AnswerButton[] _answerButtons;

    private Animator _animator = null;

    private int _buttonIndex = 0;
    private bool _timerActive = false;
    private bool _answerPanelActive = false;
    #endregion


    private void Awake()
    {
        DialogueManager.Instance.DialoguePanel = this;
        _animator = GetComponent<Animator>();
        
        _timerBar.SetActive(false);
        foreach (var button in _answerButtons)
            button.gameObject.SetActive(false);
    }

    public void DialoguePanelActivity(bool activity)
    {
        _animator.SetBool("Text Open", activity);
    }

    public void AnswerPanelActivity(bool activity)
    {
        if(_answerPanelActive == activity)
            return;

        _answerPanelActive = activity;
        _animator.SetBool("Answer Open", _answerPanelActive);
        if (!_answerPanelActive)
        {
            DialogueManager.Instance.CalculateNxtDialogueIndex(_buttonIndex);
            _buttonIndex = 0;
            _timerBar.SetActive(false);
            foreach (var button in _answerButtons)
                button.gameObject.SetActive(false);
        }
    }

    public void ShowDialog(string talkerName, string text, SentenceTypes sentenceType, bool showName, float time)
    {
        switch (sentenceType)
        {
            case SentenceTypes.Normal:
                if (showName)
                    _dialogueText.text = $"{talkerName}: {text}";
                else
                    _dialogueText.text = text;
                break;

            case SentenceTypes.Question:
                if (showName)
                    _dialogueText.text = $"{talkerName}: {text}";
                else
                    _dialogueText.text = text;
                break;

            case SentenceTypes.Answer:
                AnswerPanelActivity(true);
                TimerBarUnloading(time);
                _answerButtons[_buttonIndex].gameObject.SetActive(true);
                _answerButtons[_buttonIndex].OpenButton(_buttonIndex, text);
                _buttonIndex++;
                break;
        }
    }

    private void TimerBarUnloading(float time)
    {
        if (time <= 0 || _timerActive)
            return;

        _leftTimer.transform.localScale = Vector3.one;
        _rightTimer.transform.localScale = Vector3.one;

        _timerActive = true;
        _timerBar.SetActive(true);
        _leftTimer.transform.DOScaleX(0, time).SetEase(Ease.Linear).SetId(2905);
        _rightTimer.transform.DOScaleX(0, time).SetEase(Ease.Linear).SetId(2905).
        OnComplete(() =>
            {
                _timerActive = false;
                _answerButtons[0].ButtonClicked();
            }
        );
    }
}
