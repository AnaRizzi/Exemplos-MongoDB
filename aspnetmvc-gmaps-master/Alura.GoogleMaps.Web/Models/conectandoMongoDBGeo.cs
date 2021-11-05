using MongoDB.Driver;

namespace Alura.GoogleMaps.Web.Models
{
    public class conectandoMongoDBGeo
    {
        private readonly string _stringConnection;
        private readonly string _base;
        private readonly string _colecao;

        private readonly IMongoClient client;
        private readonly IMongoDatabase bancoDados;

        public conectandoMongoDBGeo()
        {
            _stringConnection = "mongodb://localhost:27017";
            _base = "geo";
            _colecao = "airports";

            client = new MongoClient(_stringConnection);

            bancoDados = client.GetDatabase(_base);
        }

        public IMongoClient Cliente
        {
            get { return client; }
        }

        public IMongoCollection<Aeroporto> Airports
        {
            get { return bancoDados.GetCollection<Aeroporto>(_colecao); }
        }

    }
}