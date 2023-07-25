﻿using AracKiralama.Entities;
using AracKiralama.Services.Abstract;
using AracKiralama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AracKiralama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SlidersController : Controller
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> service)
        {
            _service = service;
        }

        // GET: SlidersController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
            return View(model);
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider slider, IFormFile? Resim)
        {
            try
            {
                slider.Resim= await FileHelper.FileLoaderAsync(Resim, "/Img/Slider/");
                await _service.AddAsync(slider);
                await _service.saveAsync(); 

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var data = await _service.FindAsync(id);
            return View(data);
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Slider slider, IFormFile? Resim)
        {
            try
            {
                if(Resim is not null )
                    slider.Resim= await FileHelper.FileLoaderAsync(Resim, "/Img/Slider/");
                _service.Update(slider);
                _service.saveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _service.FindAsync(id);
            return View(data);
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider slider)
        {
            try
            {
                _service.Delete(slider);
                await _service.saveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
