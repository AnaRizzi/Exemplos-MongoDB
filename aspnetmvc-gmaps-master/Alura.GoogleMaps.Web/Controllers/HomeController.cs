using Alura.GoogleMaps.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Alura.GoogleMaps.Web.Geocoding;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Alura.GoogleMaps.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //coordenadas quaisquer para mostrar o mapa
            var coordenadas = new Coordenada("São Paulo", "-23.64340873969638", "-46.730219057147224");
            return View(coordenadas);
        }

        public async Task<JsonResult> Localizar(HomeViewModel model)
        {
            Debug.WriteLine(model);

            //Captura a posição atual e adiciona a lista de pontos
            //é essa lista de Coordenadas que será desenhada no mapa, o primeiro elemento é o verde, o endereço original
            Coordenada coordLocal = ObterCoordenadasDaLocalizacao(model.Endereco);
            var aeroportosProximos = new List<Coordenada>();
            aeroportosProximos.Add(coordLocal);

            //Captura a latitude e longitude locais
            double lat = Convert.ToDouble(coordLocal.Latitude.Replace(".", ","));
            double lon = Convert.ToDouble(coordLocal.Longitude.Replace(".", ","));

            //Testa o tipo de aeroporto que será usado na consulta
            string tipoAero = "";

            if (model.Tipo == TipoAeroporto.Internacionais)
            {
                tipoAero = "International";
            }
            if (model.Tipo == TipoAeroporto.Municipais)
            {
                tipoAero = "Municipal";
            }

            //Captura o valor da distancia
            int distancia = model.Distancia * 1000;

            //Conecta MongoDB   
            var conexao = new conectandoMongoDBGeo();            

            //Configura o ponto atual no mapa           
            var ponto = new GeoJson2DGeographicCoordinates(lon, lat);
            var minhaLoc = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(ponto);


            // filtro
            var filtro = Builders<Aeroporto>.Filter;
            FilterDefinition<Aeroporto> condicao;

            if (tipoAero == "")
            {
                condicao = filtro.NearSphere(x => x.loc, minhaLoc, distancia);
            }
            else
            {
                condicao = filtro.NearSphere(x => x.loc, minhaLoc, distancia) & filtro.Eq(x => x.type, tipoAero);
            }


            //Captura  a lista
            var lista = await conexao.Airports.Find(condicao).ToListAsync();

            //Escreve os pontos
            foreach(var item in lista)
            {
                var coordenadas = new Coordenada(item.name, item.loc.Coordinates.Latitude.ToString().Replace(",", "."), item.loc.Coordinates.Longitude.ToString().Replace(",", "."));

                aeroportosProximos.Add(coordenadas);
            }

            return Json(aeroportosProximos);
        }

        private Coordenada ObterCoordenadasDaLocalizacao(string endereco)
        {
            //busca na api do google o endereço e retorna a latitude e longitude

            string url = $"http://maps.google.com/maps/api/geocode/json?address={endereco}";
            Debug.WriteLine(url);

            var coord = new Coordenada("Não Localizado", "-10", "-10");
            var response = new WebClient().DownloadString(url);
            var googleGeocode = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(response);
            //Debug.WriteLine(googleGeocode);

            if (googleGeocode.status == "OK")
            {
                coord.Nome = googleGeocode.results[0].formatted_address;
                coord.Latitude = googleGeocode.results[0].geometry.location.lat;
                coord.Longitude = googleGeocode.results[0].geometry.location.lng;
            }

            return coord;
        }
    }
}