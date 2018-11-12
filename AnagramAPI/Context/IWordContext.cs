using AnagramAPI.Models;
using MongoDB.Driver;

namespace AnagramAPI.Context
{
    public interface IWordContext
    {
        IMongoCollection<Word> Words { get; }
    }
}