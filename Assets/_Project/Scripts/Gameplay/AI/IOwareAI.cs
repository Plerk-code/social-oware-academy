namespace SocialOwareAcademy.Gameplay.AI
{
    /// <summary>
    /// Interface for Oware AI opponents.
    /// Implement this interface to create different AI difficulty levels.
    /// </summary>
    public interface IOwareAI
    {
        /// <summary>
        /// Get the AI's chosen move for the current board state.
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <returns>Pit index to play, or -1 if no valid move</returns>
        int GetMove(OwareBoard board);

        /// <summary>
        /// Get the display name of this AI difficulty level.
        /// </summary>
        string GetDifficultyName();
    }
}

