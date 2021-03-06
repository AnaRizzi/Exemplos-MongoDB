using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projetoBlog.Models
{
    public class Publicacao
    {
        //no mongo o Id é desse tipo ObjectId, então precisa disso
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public List<string> Tags { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<Comentario> Comentarios { get; set; }




    }
}