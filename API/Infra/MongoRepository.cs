using API.Entities;
using MongoDB.Driver;

namespace API.Infra
{
    public interface IMongoRepository<T>
    {
        List<T> Get();
        T Get(string id);
        T Create(T news);
        void Update(string id, T news);
        void Remove(String id);
    }
    
    public class MongoRepository<T> : IMongoRepository<T> where T: BaseEntity
    {
        private readonly IMongoCollection<T> _model;

        public MongoRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _model = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public List<T> Get() => _model.Find<T>((active => true)).ToList();

        public T Get(string id) => _model.Find<T>(news => news.Id == id).FirstOrDefault();

        public T Create(T news)
        {
            _model.InsertOne(news);
            return news;
        }

        public void Update(string id, T news) => _model.ReplaceOne(news => news.Id == id, news);

        public void Remove(string id) => _model.DeleteOne( news => news.Id == id);
    }
}

