using AnagramAPI.Models;
using AnagramAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnagramsController : Controller
    {
        private readonly IWordRepository _wordRepository;

        public AnagramsController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        // GET api/anagrams?text=WordText&resultCount=AmountInteger
        [HttpGet(Name = "Get Anagrams for Given Word")]
        [Route("anagrams/{text}/{resultCount}")]
        [ProducesResponseType(typeof(IEnumerable<Word>), 200)]
        public async Task<ActionResult> Get([FromQuery] string text, [FromQuery] int resultCount = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text), "Parameter was null or white space.");
            else if (resultCount == 0)
                throw new ArgumentException(nameof(resultCount), "Parameter should be a positive integer.");

            var words = await _wordRepository.GetAllWords();
            if (!words.Any())
                throw new Exception($"The Words DB does not contain any words."); // This could be a potential custom exception.

            var wordsWithSameLength = words.Where(word => word.Text.Length == text.Length
                                                        && !word.Text.Equals(text, StringComparison.InvariantCultureIgnoreCase));
            if (!wordsWithSameLength.Any())
            {
                return StatusCode(204);
            }
                

            var textOrdered = text.ToLowerInvariant().OrderBy(c => c).ToString();
            var orderedWords = wordsWithSameLength.OrderBy(word => string.Concat(word.Text.ToLowerInvariant().OrderBy(c => c)));

            var results = new List<Word>();

            foreach (var wordWithSameLength in wordsWithSameLength)
            {
                var wordCharactersOrdered = text.ToLowerInvariant().OrderBy(c => c).ToString();
                if (textOrdered.Equals(wordCharactersOrdered, StringComparison.InvariantCultureIgnoreCase))
                    results.Add(wordWithSameLength);
            }

            return StatusCode(200, results);
        }
    }
}