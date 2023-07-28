namespace ProtectTheCastle.UI
{
    public interface IUIManager
    {
        void StartGame();
        void PauseGame(bool paused);
        void ShowPlayerDirectionControls(bool show);
        void ShowPlayerBattleControls(bool show);
        void DirectionClicked(string direction);
        void AttackClicked();
        void ShowWinner(string winner);
        void PlayAgain();
    }
}
