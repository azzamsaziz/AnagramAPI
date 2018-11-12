using AnagramAPI.Context;
using AnagramAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly IWordContext _context;

        public WordRepository(IWordContext context)
        {
            _context = context;
        }

        public Task Create(Word word)
        {
            return _context.Words.InsertOneAsync(word);
        }

        public async Task<bool> Delete(string text)
        {
            FilterDefinition<Word> filter = Builders<Word>.Filter.Eq(word => word.Text, text);
            DeleteResult deleteResult = await _context.Words.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Word>> GetAllWords()
        {
            if (_context.Words == null)
                return Enumerable.Empty<Word>();

            return await _context.Words?.Find(_ => true)?.ToListAsync();
        }

        public Task<Word> GetWord(string text)
        {
            FilterDefinition<Word> filter = Builders<Word>.Filter.Eq(w => w.Text, text);

            return _context.Words.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Word word)
        {
            ReplaceOneResult updateResult = await _context.Words.ReplaceOneAsync(filter: w => w.Id == word.Id, replacement: word);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}