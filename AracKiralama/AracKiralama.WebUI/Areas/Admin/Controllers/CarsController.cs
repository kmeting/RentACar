﻿using AracKiralama.Entities;
using AracKiralama.Services.Abstract;
using AracKiralama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AracKiralama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CarsController : Controller
    {
        private readonly IcarService _service;
        private readonly IService<Marka> _serviceMArka;
        private readonly IService<Slider> _sliderService;

        public CarsController(IcarService service, IService<Marka> serviceMArka, IService<Slider> sliderService)
        {
            _service = service;
            _serviceMArka = serviceMArka;
            _sliderService = sliderService;
        }

        // GET: CarsController
        public async Task<IActionResult> IndexAsync()
        {
            ViewData["Markalar"] = _serviceMArka
        .GetAll()
        .Select(c => new SelectListItem() { Text = c.Adi, Value = c.Id.ToString() })
        .ToList();
            var model = await _service.GetAllAsync();
            return View(model);
        }

        // GET: CarsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarsController/Create
        public ActionResult Create()
        {
            var markalar = _serviceMArka.GetAll();

            ViewData["Markalar"] = _serviceMArka
       .GetAll()
       .Select(c => new SelectListItem() { Text = c.Adi, Value = c.Id.ToString() })
       .ToList();
            return View();
        }

        // POST: CarsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Arac arac,IFormFile? Resim1, IFormFile? Resim2, IFormFile? Resim3)
        {
            var hasArac = _service.Get(a => a.Plaka == arac.Plaka);

            if (hasArac is not null)
            {
                ModelState.AddModelError("AynıPlaka", "Bu plakada bir arac zaten mevcut");
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    arac.Resim1= await FileHelper.FileLoaderAsync(Resim1, "/Img/Cars/");
                    arac.Resim2 = await FileHelper.FileLoaderAsync(Resim2, "/Img/Cars/");
                    arac.Resim3 = await FileHelper.FileLoaderAsync(Resim3, "/Img/Cars/");
                    if (arac.Anasayfa)
                    {
                        
                    }
                    await _service.AddAsync(arac);
                    await _service.saveAsync();     
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
               
            }
            ViewData["Markalar"] = _serviceMArka
       .GetAll()
       .Select(c => new SelectListItem() { Text = c.Adi, Value = c.Id.ToString() })
       .ToList();
            return View(arac);
        }

        // GET: CarsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewData["Markalar"] = _serviceMArka
       .GetAll()
       .Select(c => new SelectListItem() { Text = c.Adi, Value = c.Id.ToString() })
       .ToList();
            var model =await _service.FindAsync(id);
            return View(model);
        }

        // POST: CarsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Arac arac, IFormFile? Resim1, IFormFile? Resim2, IFormFile? Resim3)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Resim1 is not null)
                    {
                        arac.Resim1 = await FileHelper.FileLoaderAsync(Resim1, "/Img/Cars/");
                    }

                    if (Resim2 is not null)
                    {
                        arac.Resim2 = await FileHelper.FileLoaderAsync(Resim2, "/Img/Cars/");
                    }
                    if (Resim3 is not null)
                    {
                        arac.Resim3 = await FileHelper.FileLoaderAsync(Resim3, "/Img/Cars/");
                    }
                    _service.Update(arac);
                    _service.save();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }

            }
            ViewData["Markalar"] = _serviceMArka
       .GetAll()
       .Select(c => new SelectListItem() { Text = c.Adi, Value = c.Id.ToString() })
       .ToList();
            return View(arac);
        }

        // GET: CarsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _service.FindAsync(id);

            model.Marka = _serviceMArka.Get(m => m.Id == model.MarkaId);

            return View(model);
        }

        // POST: CarsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Arac arac)
        {
            try
            {
                _service.Delete(arac);
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
