using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using QRCoder;
using Template.Context;
using Template.Models;

namespace Template.Controllers
{
    public class EventsController : Controller
    {
        private readonly CustomDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EventsController(CustomDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var customDbContext = _context.Events.Include(e => e.Category);
            return View(await customDbContext.ToListAsync());
        }

        // GET: Events
        public async Task<IActionResult> EventList()
        {
            var customDbContext = _context.Events.Include(e => e.Category);
            return View(await customDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // Post: Events/Details/5
        public async Task<IActionResult> Detailsubmit(int? id, int? userId = 1)
        {
            if (id == null || userId == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                 .Include(e => e.Category)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if(@event == null)
            {
                return NotFound();
            }

            var @userInEvent = new UserInEvent
            {
                EventId = Convert.ToInt32(id),
                UserId = Convert.ToInt32(userId),
                IsApproved = true,
                EventBarcode = Guid.NewGuid().ToString()
            };

            _context.Add(@userInEvent);
            await _context.SaveChangesAsync();

            int userEventid = @userInEvent.Id;
            return RedirectToAction("ThankYou", new { id = userEventid });

        }

        // GET: Events/ThankYou/5
        public async Task<IActionResult> ThankYou(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.UserInEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(events.EventBarcode,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] bytes = (byte[])TypeDescriptor.GetConverter(qrCodeImage)
            .ConvertTo(qrCodeImage, typeof(byte[]));
            string base64String = Convert.ToBase64String(bytes);
            ViewBag.QrCode = base64String;

            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["CategoryId"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = Convert.ToString(c.Id)
            });
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events @events)
        {

            if (ModelState.IsValid)
            {
                var fileName = String.Empty;

                string Picture = String.Empty;
                string Photo = String.Empty;

                // Save Event Picture to uploads folder
                if (@events.Picture != null)
                {
                    fileName = Guid.NewGuid().ToString() + "_" + @events.Picture.FileName;
                    var path = "/Content/upload/eventPicture";
                    var uploadDir = Path.Combine(webHostEnvironment.WebRootPath,@".." + path);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(imageUrl, FileMode.Create))
                    {
                        @events.Picture.CopyTo(fileStream);
                    }
                    Picture = path + '/' + fileName;
                }

                // Save Event Photo to upload folder
                if (@events.Photo != null)
                {
                    fileName = Guid.NewGuid().ToString() + "_" + @events.Photo.FileName;
                    var path = "/Content/upload/eventPhoto";
                    var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, @".." + path);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(imageUrl, FileMode.Create))
                    {
                        @events.Photo.CopyTo(fileStream);
                    }
                    Photo = path + '/' + fileName;
                }

                //Model data for saving
                var @event = new Event
                {
                    Name = @events.Name,
                    Detail = @events.Detail,
                    Picture = Picture,
                    Photo = Photo,
                    Requirement = @events.Requirement,
                    Time = @events.Time,
                    Location = @events.Location,
                    ParticipationIcon = @events.ParticipationIcon,
                    ISsetlimited = @events.ISsetlimited,
                    Setlimited = @events.Setlimited,
                    IsNeedApproval = @events.IsNeedApproval,
                    StartDate = @events.StartDate,
                    EndDate = @events.EndDate,
                    CategoryId = events.CategoryId
                };

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", @event.CategoryId);
            ViewData["CategoryId"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = Convert.ToString(c.Id)
            });
            return View(@events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            // Model data set to view
            var @events = new Events
            {
                Id = @event.Id,
                Name = @event.Name,
                Detail = @event.Detail,
                PicturePath = @event.Picture,
                PhotoPath = @event.Photo,
                Requirement = @event.Requirement,
                Time = @event.Time,
                Location = @event.Location,
                ParticipationIcon = @event.ParticipationIcon,
                ISsetlimited = @event.ISsetlimited,
                Setlimited = @event.Setlimited,
                IsNeedApproval = @event.IsNeedApproval,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                CategoryId = @event.CategoryId
            };

            //var Catid = new SelectList(_context.Categories, "Id", "Id", @event.CategoryId);
            ViewData["CategoryList"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = Convert.ToString(c.Id),
                Selected = (Convert.ToInt32(@event.CategoryId) == c.Id) ? true : false
            }); 

            return View(@events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Events @events)
        {
            if (id != @events.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var fileName = String.Empty;

                string Picture = String.Empty;
                string Photo = String.Empty;

                // Saving event picture in upload folder 
                if (@events.Picture != null)
                {
                    fileName = Guid.NewGuid().ToString() + "_" + @events.Picture.FileName;
                    var path = "/Content/upload/eventPicture";
                    var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, @".." + path);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(imageUrl, FileMode.Create))
                    {
                        @events.Picture.CopyTo(fileStream);
                    }
                    Picture = path + '/' + fileName;
                }

                // saving event photo in upload folder 
                if (@events.Photo != null)
                {
                    fileName = Guid.NewGuid().ToString() + "_" + @events.Photo.FileName;
                    var path = "/Content/upload/eventPhoto";
                    var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, @".." + path);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(imageUrl, FileMode.Create))
                    {
                        @events.Photo.CopyTo(fileStream);
                    }
                    Photo = path + '/' + fileName;
                }


                // Model data to update event
                var @event = new Event
                {
                    Id = @events.Id,
                    Name = @events.Name,
                    Detail = @events.Detail,
                    Picture = !String.IsNullOrEmpty(Picture) ? Picture : @events.PicturePath,
                    Photo = !String.IsNullOrEmpty(Photo) ? Photo : @events.PhotoPath,
                    Requirement = @events.Requirement,
                    Time = @events.Time,
                    Location = @events.Location,
                    ParticipationIcon = @events.ParticipationIcon,
                    ISsetlimited = @events.ISsetlimited,
                    Setlimited = @events.Setlimited,
                    IsNeedApproval = @events.IsNeedApproval,
                    StartDate = @events.StartDate,
                    EndDate = @events.EndDate,
                    CategoryId = @events.CategoryId
                };

                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", @events.CategoryId);
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", @events.CategoryId);
            ViewData["CategoryList"] = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = Convert.ToString(c.Id),
                Selected = (Convert.ToInt32(@events.CategoryId) == c.Id) ? true : false
            });
            return View(@events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
