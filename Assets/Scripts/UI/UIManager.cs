using System;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players.Enums;
using UnityEngine;

namespace ProtectTheCastle.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {    
        public static UIManager Instance { get; private set; }
        
        public GameObject panel;
        public GameObject mainScreen;
        public GameObject chooseDirections;

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

        public void ShowPlayerControls(bool show)
        {
            chooseDirections.SetActive(show);
        }

        public void DirectionClicked(string direction)
        {
            if (direction.Equals("left", StringComparison.OrdinalIgnoreCase))
            {
                GameManager.Instance.DirectionClicked(EnumPlayerMoveDirection.Left);
            }
            else if (direction.Equals("right", StringComparison.OrdinalIgnoreCase))
            {
                GameManager.Instance.DirectionClicked(EnumPlayerMoveDirection.Right);
            }
            else if (direction.Equals("up", StringComparison.OrdinalIgnoreCase))
            {
                GameManager.Instance.DirectionClicked(EnumPlayerMoveDirection.Forward);
            }
        }
    }
}
