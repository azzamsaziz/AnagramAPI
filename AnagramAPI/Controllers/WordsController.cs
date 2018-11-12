using AnagramAPI.Models;
using AnagramAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;

        public WordsController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        // GET api/words
        [HttpGet(Name = "Get All Words")]
        public async Task<IActionResult> Get()
        {
            var words = await _wordRepository.GetAllWords();

            if (words == null || !words.Any())
                return new NoContentResult();

            return new ObjectResult(words);
        }

        // GET api/words/WordText
        [HttpGet("{text}", Name = "Get One Word")]
        public async Task<ActionResult> Get([FromQuery] string text)
        {
            var word = await _wordRepository.GetWord(text);

            if (word == null)
                return StatusCode(204);

            return StatusCode(200, word);
        }

        // POST api/words
        [HttpPost(Name = "Add List of Words")]
        [Route("words")]
        [ProducesResponseType(typeof(IEnumerable<Word>), 201)]
        public async Task<IActionResult> Post([FromBody] List<Word> words)
        {
            var addedWords = new List<Word>();

            foreach (var word in words)
            {
                // If the word does not exist.
                if (await _wordRepository.GetWord(word.Text) == null)
                {
                    await _wordRepository.Create(word);
                    addedWords.Add(word);
                }
            }

            if (!addedWords.Any())
                return StatusCode(204);

            return StatusCode(201, addedWords);
        }

        // DELETE api/words/WordText
        [HttpDelete(Name = "Delete a Word")]
        [Route("words/{text}")]
        [ProducesResponseType(typeof(Word), 200)]
        public async Task<IActionResult> Delete([FromBody] Word word)
        {
            var wordFromDb = await _wordRepository.GetWord(word.Text);
            if (wordFromDb == null)
                return StatusCode(204);

            await _wordRepository.Delete(word.Text);

            return StatusCode(200, wordFromDb);
        }
    }
}