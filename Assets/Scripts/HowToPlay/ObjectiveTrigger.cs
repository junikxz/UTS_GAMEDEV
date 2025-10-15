using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the objective");
            TutorialManager.Instance.PlayerReachedObjective();

            gameObject.SetActive(false);
        }
    }
}
