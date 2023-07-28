namespace ProtectTheCastle.UI
{
    public interface IUIManager
    {
        void StartGame();
        void PauseGame(bool paused);
        void ShowPlayerControls(bool show);
        void DirectionClicked(string direction);
    }
}
