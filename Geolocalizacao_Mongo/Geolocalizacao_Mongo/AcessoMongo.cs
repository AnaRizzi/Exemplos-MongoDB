using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geolocalizacao_Mongo
{
    public class AcessoMongo
    {

        private readonly string _stringConnection;
        private readonly string _base;
        private readonly string _colecao;

        private readonly IMongoClient client;
        private readonly IMongoDatabase bancoDados;

        public AcessoMongo()
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

        public IMongoCollection<Airport> Airports
        {
            get { return bancoDados.GetCollection<Airport>(_colecao); }
        }
    }
}
