using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public static Shooting instance;

    [Header("References")] [SerializeField]
    private AudioSource shootSound;
    public AudioSource smashSound;
    public AudioSource reloadSound;

    [SerializeField] private Transform bulletParent;

    [Header("Prefabs")] [SerializeField] private GameObject bulletPrefab;

    [Header("Settings")] public float bulletForce;
    public float bulletLife;
    public float shootingRate;
    public int damage;
    public int gunCount;
    
    public bool IsShooting = true;

    [SerializeField] private Vector3 bulletSpawnOffset;
    [SerializeField] private float spawnOffsetBetweenBullets;

    private CharacterMovement _characterMovement;

    private void Awake() {
        if (instance == null)
            instance = this;
        
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start() {
        StartCoroutine(WaitForRun());
    }

    private IEnumerator WaitForRun() {
        yield return new WaitUntil(() => _characterMovement.IsMoving);
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot() {
        while (_characterMovement.IsMoving) {
            yield return new WaitForSeconds(shootingRate);
            if (IsShooting) {
                shootSound.pitch = UnityEngine.Random.Range(0.9f, 1f);
                shootSound.Play();
                if (gunCount % 2 == 1) {
                    int range = Mathf.FloorToInt((float)gunCount/2);
                    for (int i = -range; i < range + 1; i++) {
                        Vector3 spawnPos = transform.position + bulletSpawnOffset;
                        spawnPos.x += i * spawnOffsetBetweenBullets;
                        Instantiate(bulletPrefab, spawnPos, Quaternion.identity, bulletParent);
                    }
                }
                else {
                    int range = gunCount/2;
                    for (int i = -range; i < range; i++) {
                        Vector3 spawnPos = transform.position + bulletSpawnOffset;
                        spawnPos.x += ((float)i + 0.5f) * spawnOffsetBetweenBullets;
                        Instantiate(bulletPrefab, spawnPos, Quaternion.identity, bulletParent);
                    }
                }
            }
        }
    }
}