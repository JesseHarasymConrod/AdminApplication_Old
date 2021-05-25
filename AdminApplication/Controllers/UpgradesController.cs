using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdminApplication.Data;
using AdminApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdminApplication.Controllers
{
    [Authorize]
    public class UpgradesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UpgradesController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Upgrades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Upgrade.ToListAsync());
        }

        // GET: Upgrades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upgrade = await _context.Upgrade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upgrade == null)
            {
                return NotFound();
            }

            return View(upgrade);
        }

        // GET: Upgrades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Upgrades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Create([Bind("Id,CreationDate,Name,Version,FileName,FilePath,ZipFile")] Upgrade upgrade)
        {
            if (ModelState.IsValid)
            {
                string fileName = upgrade.ZipFile.FileName.ToLower();

                upgrade.FileName = Path.GetFileNameWithoutExtension(fileName);
                
                upgrade.FilePath = Path.Combine(_config.GetValue<string>("UpgradeFilePath"), fileName);

                using (var fileStream = new FileStream(upgrade.FilePath, FileMode.Create))
                {
                    await upgrade.ZipFile.CopyToAsync(fileStream);
                }
                
                _context.Add(upgrade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(upgrade);
        }

        // GET: Upgrades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upgrade = await _context.Upgrade.FindAsync(id);
            if (upgrade == null)
            {
                return NotFound();
            }
            return View(upgrade);
        }


        //TODO: Make sure this doesn't break the file pathing
        // POST: Upgrades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreationDate,Name,Version")] Upgrade upgrade)
        {
            if (id != upgrade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(upgrade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpgradeExists(upgrade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(upgrade);
        }

        // GET: Upgrades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upgrade = await _context.Upgrade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upgrade == null)
            {
                return NotFound();
            }

            return View(upgrade);
        }

        // POST: Upgrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var upgrade = await _context.Upgrade.FindAsync(id);
            string filePathToRemove = upgrade.FilePath;

            try
            {
                System.IO.File.Delete(filePathToRemove);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }

            _context.Upgrade.Remove(upgrade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpgradeExists(int id)
        {
            return _context.Upgrade.Any(e => e.Id == id);
        }
    }
}
