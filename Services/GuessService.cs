using WordleAPI.Models;
using WordleAPI.Services.Interfaces;

namespace WordleAPI.Services
{
    public class GuessService : IGuessService
    {
        public Dictionary<char, int> LetterPoints { get; set; }
        public GuessService()
        {
            LetterPoints = new Dictionary<char, int>
            {
                {'A', 0},{'B', 0},{'C', 0},{'D', 0},{'E', 0},{'F', 0},{'G', 0},{'H', 0},{'I', 0},{'J', 0},{'K', 0},{'L', 0},{'M', 0},
                {'N', 0},{'O', 0},{'P', 0},{'Q', 0},{'R', 0},{'S', 0},{'T', 0},{'U', 0},{'V', 0},{'W', 0},{'X', 0},{'Y', 0},{'Z', 0}
            };
        }

        public void ComputePointsForLetters(List<string> listPossibleAttempts)
        {
            string aux = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int count;
            foreach (char letter in aux)
            {
                foreach (string words in listPossibleAttempts)
                {
                    foreach (char letterWords in words)
                    {
                        if (letter == letterWords)
                        {
                            LetterPoints.TryGetValue(letter, out count);
                            LetterPoints[letter] = count + 1;
                            break;
                        }
                    }
                }
            }
        }

        public string MakeGuess(List<string> listPossibleAttempts)
        {
            return GetBestPossibleGuess(listPossibleAttempts);
        }

        public string CompareGuess(string guess, string answer)
        {
            int index;
            var outputString = "00000";
            var outputArray = outputString.ToCharArray();
            var cloneAnswer = answer.ToCharArray();
            var cloneGuess = guess.ToCharArray();
            for (int i = 0; i < guess.Length; i++)
            {
                if (cloneAnswer[i] == cloneGuess[i])
                {
                    outputArray[i] = '2';
                    cloneAnswer[i] = ' ';
                    cloneGuess[i] = ' ';
                }

            }
            for (int i = 0; i < guess.Length; i++)
            {
                index = Array.IndexOf(cloneAnswer, cloneGuess[i]);
                if (index != -1 && cloneGuess[i] != ' ')
                {
                    outputArray[i] = '1';
                    cloneAnswer[index] = ' ';
                    cloneGuess[i] = ' ';
                }

            }
            return new string(outputArray);
        }

        #region private methods
        private string GetBestPossibleGuess(List<string> listPossibleAttempts)
        {
            int mostPoints = 0;
            string bestGuess = listPossibleAttempts.FirstOrDefault();

            foreach (string word in listPossibleAttempts)
            {
                int value = 0;
                string noRep = new(word.ToCharArray().Distinct().ToArray());
                foreach (char letter in noRep)
                {
                    LetterPoints.TryGetValue(letter, out int count);
                    value += count;
                }

                if (value > mostPoints)
                {
                    bestGuess = word;
                    mostPoints = value;
                }
            }
            return bestGuess;
        }
        #endregion
    }
}
