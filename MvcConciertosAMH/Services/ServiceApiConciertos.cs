using Microsoft.Extensions.Configuration;
using MvcConciertosAMH.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcConciertosAMH.Services
{
    public class ServiceApiConciertos
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiConciertos(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiConciertosAWS");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApi<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //api/categorias
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "/api/categorias";
            List<Categoria> categorias =
                await this.CallApi<List<Categoria>>(request);
            return categorias;
        }

        //api/eventos
        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "/api/eventos";
            List<Evento> eventos =
                await this.CallApi<List<Evento>>(request);
            return eventos;
        }

        //api/eventos/FindEvento/idevento
        public async Task<Evento> FindEventoAsync(int idevento)
        {
            string request = "/api/eventos/findevento/"+idevento;
            Evento evento =
                await this.CallApi<Evento>(request);
            return evento;
        }

        //api/eventos/geteventoscategoria/idcategoria
        public async Task<List<Evento>> GetEventosCategoriaAsync(int idcategoria)
        {
            string request = "/api/eventos/geteventoscategoria/"+idcategoria;
            List<Evento> eventos =
                await this.CallApi<List<Evento>>(request);
            return eventos;
        }

        //api/eventos
        public async Task InsertEventoAsync(Evento evento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/eventos";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string json = JsonConvert.SerializeObject(evento);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage reponse = await client.PostAsync(this.UrlApi+request, content);
            }
        }

        //api/eventos/delete/idevento
        public async Task DeleteEventoAsync(int idevento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/eventos/delete/" + idevento;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.DeleteAsync(this.UrlApi+request);
            }
        }

        //api/eventos
        public async Task CambiarCategoriaAsync(Evento evento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/eventos";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Evento e = new Evento();
                e.IdEvento = evento.IdEvento;
                e.Nombre = evento.Nombre;
                e.Artista = evento.Artista;
                e.IdCategoria = evento.IdCategoria;

                string json = JsonConvert.SerializeObject(e);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(this.UrlApi+request, content);
            }
        }

    }
}
