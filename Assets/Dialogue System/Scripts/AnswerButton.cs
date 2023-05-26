using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnswerButton : MonoBehaviour
{
    #region Fields
    [SerializeField] private Button _button;
    [SerializeField] private Text _text;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedTextColor;

    private int _index = 0;
    private bool _isClicked = false;
    #endregion


    private void OnEnable()
    {
        DialogueManager.OnAnswerSelected += AnswerSelected;
    }

    private void OnDisable()
    {
        DialogueManager.OnAnswerSelected -= AnswerSelected;
    }

    public void OpenButton(int index, string text)
    {
        _text.color = _defaultColor;
        _index = index;
        _text.text = text;
        _button.interactable = true;
        _isClicked = false;
    }

    private void AnswerSelected()
    {
        _button.interactable = false;
        if(!_isClicked)
            _text.DOFade(0, .2f);
    }

    public void ButtonClicked()
    {
        _isClicked = true;
        _text.color = _selectedTextColor;
        DOTween.Kill(2905);
        DialogueManager.Instance.ApplyChoice(_index, DialogueTypes.MainStory);
    }
}
