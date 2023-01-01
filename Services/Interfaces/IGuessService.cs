namespace WordleAPI.Services.Interfaces
{
    public interface IGuessService
    {
        void ComputePointsForLetters(List<string> listPossibleAttempts);
        string MakeGuess(List<string> listPossibleAttempts);
        string CompareGuess(string guess, string answer);
    }
}
