using WordleAPI.Models;

namespace WordleAPI.Services.Interfaces
{
    public interface ISolverService
    {
        void Solve(List<string> list, string guess, string output);
    }
}
