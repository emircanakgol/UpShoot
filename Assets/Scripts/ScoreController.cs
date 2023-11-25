using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController instance;

    public static event Action OnScoreChangedCallback;
    public Action OnHealthChangedCallback;
    
    private void Awake() {
        if (!instance)
            instance = this;
    }

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI scoreTMP;
    public GameObject gameOverTextGO;
    public TextMeshProUGUI finishTMP;
    
    private int _score;
    public int Score {
        get { return _score; }
        set {
            if (_score != value) {
                _score = value;
                OnScoreChangedCallback?.Invoke();
            }
        }
    }

    private void OnEnable() {
        OnScoreChangedCallback += OnScoreChanged;
        OnHealthChangedCallback += OnHealthChanged;
    }
    
    private void OnDisable() {
        OnScoreChangedCallback -= OnScoreChanged;
        OnHealthChangedCallback -= OnHealthChanged;
    }

    private void OnScoreChanged() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scoreTMP.rectTransform.DOScale(1.2f, 0.3f).OnComplete(()=>scoreTMP.text = Score.ToString()));
        sequence.Append(scoreTMP.rectTransform.DOScale(1, 0.3f));
    }
    
    private void OnHealthChanged() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scoreTMP.rectTransform.DOScale(1.1f, 0.05f));
        sequence.Append(scoreTMP.rectTransform.DOScale(1, 0.1f));
    }
}
