using System;
using ProtectTheCastle.Game;
using ProtectTheCastle.Players.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProtectTheCastle.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {    
        public static UIManager Instance { get; private set; }
        
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private GameObject mainScreen;
        [SerializeField]
        private GameObject chooseDirections;
        [SerializeField]
        private GameObject battle;
        [SerializeField]
        private GameObject winner;
        [SerializeField]
        private GameObject winnerText;

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

        public void ShowPlayerDirectionControls(bool show)
        {
            chooseDirections.SetActive(show);
        }

        public void ShowPlayerBattleControls(bool show)
        {
            battle.SetActive(show);
        }

        public void AttackClicked()
        {
            GameManager.Instance.AttackClicked();
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

        public void ShowWinner(string whoWon)
        {
            winnerText.GetComponent<TextMeshProUGUI>().SetText(whoWon + " is the winner!");
            winner.SetActive(true);
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene("Forest Level");
        }
    }
}
