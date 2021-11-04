using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projetoBlog.Models
{
    public class AcessoMongoDB
    {
        private readonly string _stringConnection;
        private readonly string _base;
        private readonly string _colecaoPublicacao;
        private readonly string _colecaoUsuarios;

        private readonly IMongoClient client;
        private readonly IMongoDatabase bancoDados;

        public AcessoMongoDB()
        {
            _stringConnection = "mongodb://localhost:27017";
            _base = "Blog";
            _colecaoPublicacao = "Publicacoes";
            _colecaoUsuarios = "Usuarios";

            client = new MongoClient(_stringConnection);

            //vai buscar o banco de dados, se não existir, ele cria
            bancoDados = client.GetDatabase(_base);
        }

        public IMongoClient Cliente
        {
            get { return client; }
        }

        public IMongoCollection<Usuario> Usuarios
        {
            get { return bancoDados.GetCollection<Usuario>(_colecaoUsuarios); }
        }

        public IMongoCollection<Publicacao> Publicacoes
        {
            get { return bancoDados.GetCollection<Publicacao>(_colecaoPublicacao); }
        }

    }
}
