using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    private static int _obstacleCount;
    
    private Action OnHealthChangedCallback;
    
    [Header("Public Properties")]
    public int Difficulty;
    public int Index;
    
    private int _health;

    public int Health {
        get { return _health; }
        set {
            if (value != _health) {
                _health = value;
                OnHealthChangedCallback?.Invoke();
            }
        }
    }

    private int scoreValue;

    private TextMeshPro healthTMP;

    private void Awake() {
        healthTMP = GetComponentInChildren<TextMeshPro>();
    }

    private void OnEnable() {
        OnHealthChangedCallback += OnHealthChanged;
    }

    private void OnDisable() {
        OnHealthChangedCallback -= OnHealthChanged;
    }

    private void SetIndex() {
        Index = _obstacleCount;
        _obstacleCount++;
    }
    
    private void Start() {
        SetIndex();
        Health = Difficulty * 12 + Mathf.RoundToInt( Difficulty * 0.1f * Random.Range(0, 10));
        scoreValue = Health;
    }

    private void OnHealthChanged() {
        if (Health <= 0) {
            DOTween.Kill(healthTMP);
            gameObject.SetActive(false);
            ScoreController.instance.Score += scoreValue;
            Shooting.instance.smashSound.Play();
            return;
        }
        //ScoreController.instance.OnHealthChangedCallback?.Invoke();
        healthTMP.text = Health.ToString();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthTMP.transform.DOScale(1.2f, 0));
        sequence.Join(transform.DOScale(1.2f, 0));
        sequence.Join(healthTMP.DOColor(new Color(174f/255, 62f/255, 62f/255, 1), 0));
        sequence.Append(healthTMP.transform.DOScale(1, ObstacleSpawner.instance.hitTweenDuration));
        sequence.Join(transform.DOScale(1f, ObstacleSpawner.instance.hitTweenDuration));
        sequence.Join(healthTMP.DOColor(Color.white, ObstacleSpawner.instance.hitTweenDuration));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {
            Health-=Shooting.instance.damage;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player")) {
            other.GetComponentInChildren<Animator>().SetTrigger("Eat");
            other.GetComponentInChildren<Animator>().SetFloat("RunSpeed", 0);
            DOVirtual.Float(0, 1, 0.5f, (x) => CharacterMovement.instance.gameOverPpv.weight = x);
            ScoreController.instance.gameOverTextGO.SetActive(true);
            CharacterMovement.instance.GameOver = true;
            Shooting.instance.IsShooting = false;
        }
    }
}