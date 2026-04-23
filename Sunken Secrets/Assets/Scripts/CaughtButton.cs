using UnityEngine;

public class CaughtButton : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject caughtScreen;

    public void OnRespawnClicked()
    {
        if (PlayerCaught.respawnLocation != null)
        {
            playerTransform.position = PlayerCaught.respawnLocation.position;
        }

        PlayerCaught.isCaught = false;

        if (caughtScreen != null)
        {
            caughtScreen.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
