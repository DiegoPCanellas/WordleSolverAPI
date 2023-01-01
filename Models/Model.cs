using Response = WordleAPI.Models.ResponseModel;

namespace WordleAPI.Models
{
    public class Model
    {
        public List<Response>? ListGuesses { get; set; }
        public List<string>? ListPossibleAttempts { get; set; }
        public string? Answer { get; set; }

    }
}
