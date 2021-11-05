using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Threading.Tasks;

namespace Geolocalizacao_Mongo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Busca de aeroportos dos EUA!");
            Console.WriteLine("Exemplo de latitude e longitude: 38.826761, -93.522229");
            Console.WriteLine("Digite sua latitude");
            var a = Console.ReadLine(); 
            Console.WriteLine("Digite sua longitude");
            var b = Console.ReadLine();

            var latitude = Convert.ToDouble(a.Replace(".", ","));
            var longitude = Convert.ToDouble(b.Replace(".", ","));

            //salvar a sua localização para usar como referência:
            var ponto = new GeoJson2DGeographicCoordinates(longitude, latitude);
            var minhaLoc = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(ponto);

            var conexao = new AcessoMongo();

            var filtro = Builders<Airport>.Filter;
            //vai buscar num raio da sua localização, a distância é sempre em metros (1000m = 1km)
            //Obs: é preciso criar o índice de localização no banco de dados:
            //db.nomeDaCollection.ensureIndex({"campoComOGeoJson":"2dsphere"})
            var condicao = filtro.NearSphere(x => x.loc, minhaLoc, 100000);
            var lista = await conexao.Airports.Find(condicao).ToListAsync();

            Console.WriteLine("Aeroportos em um raio de 100km de você:");
            foreach(var item in lista)
            {
                Console.WriteLine(item.ToJson<Airport>());
            }

            Console.ReadKey();
        }
    }
}
