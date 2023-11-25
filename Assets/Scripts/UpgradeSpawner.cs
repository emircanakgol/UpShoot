using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UpgradeSpawner : MonoBehaviour
{
    public static UpgradeSpawner instance;

    private static Dictionary<Upgrade,int> _upgrades;

    [Header("References")] [SerializeField]
    private Transform ground;

    [Header("Prefabs")] [SerializeField] private GameObject upgradePrefab;

    [FormerlySerializedAs("trackLength")] [Header("Settings")]
    public float TrackLength;

    [FormerlySerializedAs("upgradeCount")] public int UpgradeCount;

    private void Awake() {
        if (instance == null)
            instance = this;
        _upgrades = new Dictionary<Upgrade,int>();
    }

    private void Start() {
        ground.localScale = new Vector3(1.3f, 1, TrackLength);
        float spawnStep = TrackLength / UpgradeCount;
        for (int i = 1; i < UpgradeCount; i++) {
            List<UpgradeType> currentUpgradeTypes = new();
            for (int j = -1; j < 2; j++) {
                var spawnPos = new Vector3(1.1f * j, 0, spawnStep * i);
                var upgradeGO = Instantiate(upgradePrefab, spawnPos, Quaternion.identity, transform);
                var upgrade = upgradeGO.GetComponent<Upgrade>();
                var randUpgradeType = (UpgradeType)Random.Range(1, 5);
                while (currentUpgradeTypes.Contains(randUpgradeType)) {
                    randUpgradeType = (UpgradeType)Random.Range(1, 5);
                }

                upgrade.difficulty = i;
                currentUpgradeTypes.Add(randUpgradeType);
                upgrade.upgradeType = randUpgradeType;
                _upgrades.Add(upgrade,i);
            }
        }
    }

    public void DisableOtherUpgrades(Upgrade currentUpgrade) {
        foreach (var kvp in _upgrades) {
            if (kvp.Value == currentUpgrade.difficulty && kvp.Key != currentUpgrade) {
                Destroy(kvp.Key.gameObject);
            }
        }
    }
}