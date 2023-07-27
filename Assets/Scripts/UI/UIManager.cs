using ProtectTheCastle.Game;
using UnityEngine;

namespace ProtectTheCastle.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {    
        public static UIManager Instance { get; private set; }
        
        public GameObject panel;
        public GameObject mainScreen;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }
        }
        
        public void StartGame()
        {
            if (!GameManager.Instance.gameStarted)
            {
                GameManager.Instance.StartGame();
            }
            else
            {
                GameManager.Instance.PauseOrResumeGame();
            }
        }

        public void PauseGame(bool paused)
        {
            panel.SetActive(paused);
            mainScreen.SetActive(paused);
        }
    }
}
