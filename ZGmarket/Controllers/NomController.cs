﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using ZGmarket.Data;
using ZGmarket.Models;
using ZGmarket.Models.Repository;

namespace ZGmarket.Controllers
{
    [Authorize(Roles = $"{Positions.root},{Positions.Admin},{Positions.Director},{Positions.Salesman}")]
    public class NomController : Controller
    {
        private readonly NomRepository _nomRepo;
        private readonly NomTypeRepository _typeRepo;
        public NomController(NomRepository nomRepo, NomTypeRepository typeRepo)
        {
            _nomRepo = nomRepo;
            _typeRepo = typeRepo;
        }
        // GET: Nom/index
        public async Task<IActionResult> Index()
        {
            IEnumerable<Nom> Noms = await _nomRepo.GetNom();
            return View(Noms);
        }

/*        public async Task<IActionResult> Index(IndexViewModel filtr)
        {
            IEnumerable<Nom> Noms = await _nomRepo.GetNom();
            return View(Noms);
        }*/

        // GET: Nom/Create
        public async Task<IActionResult> Create()
        {
            var _types = await _typeRepo.GetTypes();
            ViewBag.Types = _types.ToList();
            ViewBag.FirstType = _types.First().Title;
            return View();
        }

        // POST: Nom/Create
        [HttpPost]
        public async Task<IActionResult> Create(Nom newNom)
        {
            try
            {
                await _nomRepo.AddNom(newNom);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Некорректные данные " + e);
                return View(newNom);
            }
        }

        // GET: Nom/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Nom dbNom;
            try
            {
                dbNom = await _nomRepo.GetNom(id);
                return View(dbNom);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Некорректные данные " + e);
                return View();
            }
        }

        // POST: Nom/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Nom updateNom)
        {
            if (!ModelState.IsValid) return View(updateNom);
            try
            {
                var dbNom = await _nomRepo.EditNom(id, updateNom);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Некорректные данные " + e);
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _nomRepo.DeleteNom(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }

}
