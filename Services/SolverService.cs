using WordleAPI.Models;
using WordleAPI.Services.Interfaces;

namespace WordleAPI.Services
{
    public class SolverService : ISolverService
    {
        public void Solve(List<string> list, string guess, string output)
        {
            //handle repetition in guess
            var hasRepInGuess = guess.Distinct().Count() != guess.Count();

            if (hasRepInGuess)
            {
                HandleRep(list, guess, output);
            }

            int index = 0;
            foreach (var e in output)
            {
                int aux = short.Parse(e.ToString());
                switch (aux)
                {
                    //handle green
                    case (short)OutputEnum.green:
                        list.RemoveAll(x => x[index] != guess[index]);
                        break;
                    //handle yellow
                    case (short)OutputEnum.yellow:
                        list.RemoveAll(x => !x.Contains(guess[index]));
                        list.RemoveAll(x => x[index] == guess[index]);
                        break;
                    //handle grey
                    case (short)OutputEnum.grey:
                        list.RemoveAll(x => x.Contains(guess[index]));
                        break;
                }

                index++;
            }
        }

        private void HandleRep(List<string> list, string guess, string output)
        {
            var guessClone = guess.ToCharArray();
            var outputClone = output.ToCharArray();

            var duplicates = guessClone
                .Select((Name, Index) => new { Name, Index })
                .GroupBy(x => x.Name)
                .Select(xg => new {
                    Name = xg.Key,
                    Indices = xg.Select(x => x.Index)
                })
                .Where(x => x.Indices.Count() > 1);

            foreach (var duplicate in duplicates)
            {
                int countNoGrey = 0;
                foreach (var index in duplicate.Indices)
                {
                    int aux = short.Parse(outputClone[index].ToString());
                    if (aux != (short)OutputEnum.grey)
                    {
                        countNoGrey++;
                    }
                }
                if (countNoGrey > 1)
                {
                    list.RemoveAll(x => x.Count(c => c == duplicate.Name) != countNoGrey);

                }
                else
                {
                    if (countNoGrey == 0)
                    {
                        list.RemoveAll(x => x.Contains(duplicate.Name));
                    }
                }
            }
        }
    }
}
