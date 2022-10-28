using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private InGameMenus inGameMenuHandler;
    public void Crash()
    {
        gameObject.SetActive(false);
        inGameMenuHandler.GameOver();

    }


}
