using AnagramAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramAPI.Repositories
{
    public interface IWordRepository
    {
        Task<IEnumerable<Word>> GetAllWords();
        Task<Word> GetWord(string text);
        Task Create(Word word);
        Task<bool> Update(Word word);
        Task<bool> Delete(string text);
    }
}