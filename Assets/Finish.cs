using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private CinemachineMixingCamera mixCamera;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.Float(1, 0, 1, (x) => mixCamera.m_Weight0 = x));
            sequence.Join(DOVirtual.Float(0, 1, 1, (x) => mixCamera.m_Weight1 = x));
            CharacterMovement.instance.GameOver = true;
            Shooting.instance.IsShooting = false;
            other.GetComponentInChildren<Animator>().SetFloat("RunSpeed", 0);
            other.GetComponentInChildren<Animator>().SetTrigger("Stop");
            ScoreController.instance.finishTMP.gameObject.SetActive(true);
            ScoreController.instance.finishTMP.text = "Your score is " + ScoreController.instance.Score.ToString();
        }
    }
}
