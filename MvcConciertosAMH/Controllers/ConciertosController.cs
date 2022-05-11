using Microsoft.AspNetCore.Mvc;
using MvcConciertosAMH.Models;
using MvcConciertosAMH.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcConciertosAMH.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceApiConciertos service;

        public ConciertosController(ServiceApiConciertos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            return View(eventos);
        }

        public async Task<IActionResult> Categorias()
        {
            List<Categoria> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> EventosPorCategoria(int idcategoria)
        {
            List<Evento> eventos = await this.service.GetEventosCategoriaAsync(idcategoria);
            return View(eventos);
        }

        public async Task <IActionResult> CrearEvento()
        {
            ViewData["CATEGORIAS"] = await this.service.GetCategoriasAsync();
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CrearEvento(string nombre, string artista,int idcategoria)
        {
            Evento e = new Evento();
            e.IdEvento = 0;
            e.Nombre = nombre;
            e.Artista = artista;
            e.IdCategoria = idcategoria;

            await this.service.InsertEventoAsync(e);
            return RedirectToAction("Index");
        }

        public async Task <IActionResult> Delete(int idevento)
        {
            await this.service.DeleteEventoAsync(idevento);
            return RedirectToAction("Index");
        }

        public async Task <IActionResult> CambiarCategoria(int idevento)
        {
            ViewData["CATEGORIAS"] = await this.service.GetCategoriasAsync();
            Evento evento = await this.service.FindEventoAsync(idevento);
            return View(evento);
        }

        [HttpPost]
        public async Task <IActionResult> CambiarCategoria(int idevento,string nombre,string artista,int idcategoria)
        {
            Evento evento = new Evento();
            evento.IdEvento = idevento;
            evento.Nombre = nombre;
            evento.Artista = artista;
            evento.IdCategoria = idcategoria;

            await this.service.CambiarCategoriaAsync(evento);
            return RedirectToAction("Index");
        }
    }
}
