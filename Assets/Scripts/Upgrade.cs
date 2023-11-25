using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum UpgradeType {
    Empty, FireRate, GunCount, FireRange, Damage
}

public class Upgrade : MonoBehaviour
{
    private bool _playerEntered = false;
    private Action OnHealthChangedCallback;
    
    private UpgradeType _upgradeType = UpgradeType.Empty;
    public UpgradeType upgradeType {
        get { return _upgradeType;}
        set {
            if (_upgradeType != value) {
                _upgradeType = value;
                ApplyUpgradeType();
            }
        }
    }

    private int multiplier = 2;
    public int difficulty;
    private int _health = 30;
    public int Health {
        get { return _health; }
        set {
            if (value != _health) {
                _health = value;
                OnHealthChangedCallback();
            }
        }
    }

    private TextMeshPro textTMP;

    private void Awake() {
        textTMP = GetComponentInChildren<TextMeshPro>();
    }

    private void Start() {
        Health = 30 * difficulty;
    }

    private void OnEnable() {
        OnHealthChangedCallback += OnHealthChanged;
    }
    
    private void OnDisable() {
        OnHealthChangedCallback -= OnHealthChanged;
    }

    void ApplyUpgradeType() {
        switch (upgradeType) {
            case UpgradeType.FireRate:
                textTMP.text = "x" + multiplier + " Fire Rate";
                break;
            case UpgradeType.GunCount:
                textTMP.text = "+" + (multiplier - 1) + " Gun Count";
                break;
            case UpgradeType.FireRange:
                textTMP.text = "x" + multiplier + " Fire Range";
                break;
            case UpgradeType.Damage:
                textTMP.text = "+" + (multiplier - 1) + " Damage";
                break;
        }
    }

    private void OnHealthChanged() {
        if (Health <= 0) {
            Health = difficulty * 30 * multiplier;
            multiplier++;
            ApplyUpgradeType();
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.2f, 0));
        sequence.Append(transform.DOScale(1, ObstacleSpawner.instance.hitTweenDuration));
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {
            Health -= Shooting.instance.damage;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (_playerEntered) return;
        if (!other.gameObject.CompareTag("Player")) return;
        
        _playerEntered = true;
        UpgradeSpawner.instance.DisableOtherUpgrades(this);
        ApplyUpgrade();
        other.GetComponentInChildren<Animator>().SetTrigger("Reload");
        StartCoroutine(WaitForReloadToFinish(other.GetComponentInChildren<Animator>()));
        Shooting.instance.reloadSound.Play();
    }

    private void ApplyUpgrade() {
        switch (upgradeType) {
            case UpgradeType.FireRate:
                Shooting.instance.shootingRate /= 1 + (float)multiplier*20/100;
                break;
            case UpgradeType.GunCount:
                Shooting.instance.gunCount += (multiplier - 1);
                break;
            case UpgradeType.FireRange:
                Shooting.instance.bulletLife += Shooting.instance.bulletLife * multiplier / 10;
                break;
            case UpgradeType.Damage:
                Shooting.instance.damage += (multiplier - 1);
                break;
        }
    }

    private IEnumerator WaitForReloadToFinish(Animator animator) {
        Shooting.instance.IsShooting = false;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        Shooting.instance.IsShooting = true;
    }
}