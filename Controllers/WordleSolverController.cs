using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using WordleAPI.Models;
using WordleAPI.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WordleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordleSolverController : ControllerBase
    {
        private readonly ISolverService _solverService;
        private readonly IGuessService _guessService;

        public WordleSolverController(ISolverService solverService, 
                                      IGuessService guessService)
        {
            _solverService = solverService;
            _guessService = guessService;
        }

        [HttpPost ("/post")]
        [Consumes ("application/json")]
        public ActionResult<List<ResponseModel>> Post([FromBody]ReceiveModel receive)
        {
            string output = "00000";
            Model model = new()
            {
                Answer = receive.Word,
                ListPossibleAttempts = PopulateList(@"words.txt"),
                ListGuesses = new List<ResponseModel>()
            };
            _guessService.ComputePointsForLetters(model.ListPossibleAttempts);

            while (output != "22222")
            {
                var bestGuess = _guessService.MakeGuess(model.ListPossibleAttempts);
                if (bestGuess == null)
                {
                    return model.ListGuesses;
                }

                output = _guessService.CompareGuess(bestGuess, receive.Word);
                var colors = FormatColors(output);
                _solverService.Solve(model.ListPossibleAttempts, bestGuess, output);

                ResponseModel response = new()
                {
                    Guess = bestGuess,
                    Colors = colors
                };
                model.ListGuesses.Add(response);
                Console.WriteLine(model.ListPossibleAttempts);
            }

            return model.ListGuesses;
        }

        #region private methods
        private List<string> FormatColors(string output)
        {
            List<string> colors = new();
            foreach(var l in output)
            {
                int aux = short.Parse(l.ToString());
                switch (aux)
                {
                    case (short)OutputEnum.green:
                        colors.Add("#538d4e");
                        break;
                    case (short)OutputEnum.yellow:
                        colors.Add("#b59f3b");
                        break;
                    case (short)OutputEnum.grey:
                        colors.Add("#3a3a3c");
                        break;
                }
            }
            return colors;
        }

        private static List<string> PopulateList(string fileName)
        {
            List<string> list = new();

            try
            {
                using StreamReader reader = new(fileName);

                string line = reader.ReadLine();

                while (line != null)
                {
                    list.Add(line.ToUpper());

                    line = reader.ReadLine();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Failed to read file: " + e.Message);
            }

            return list;
        }
        #endregion

    }
}
