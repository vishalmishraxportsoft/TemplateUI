using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Template.Models;

namespace Template.Controllers
{
    public class DepartmentsController : Controller
    {
       
        private readonly IWebHostEnvironment webHostEnvironment;


       
        public DepartmentsController( IWebHostEnvironment hostEnvironment)
        {
           
            webHostEnvironment = hostEnvironment;
        }

        string Baseurl = "http://localhost:50635/";
        // GET: Departments with or without search
        public async Task<IActionResult> Index(string search)
        {

            SessionStore DepInfo = new SessionStore();
            List<Department> depList = new List<Department>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Departments");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var DepResponse = Res.Content.ReadAsStringAsync().Result;
                    //var myJObject = JObject.Parse(DepResponse).Last.ToList();

                    //Deserializing the response recieved from web api and storing into the Depaartment list  
                    DepInfo = JsonConvert.DeserializeObject<SessionStore>(DepResponse);

                    depList = DepInfo.data.ToList();
                    
                }
                //returning the dep list to view  
                return View(depList);
            }
           
        }

        // GET: Departments/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _context.Departments
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(department);
        //}

        //GET: Departments/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Departments departments)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var fileName = String.Empty;

        //        string departmentlogo = String.Empty;

        //        // Save department logo to uploads folder
        //        if (departments.Logo != null)
        //        {
        //            fileName = Guid.NewGuid().ToString() + "_" + departments.Logo.FileName;
        //            var path = "/Content/upload/departmentLogo";
        //            var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, @".." + path);
        //            var imageUrl = Path.Combine(uploadDir, fileName);
        //            using (var fileStream = new FileStream(imageUrl, FileMode.Create))
        //            {
        //                departments.Logo.CopyTo(fileStream);
        //            }
        //            departmentlogo = path + '/' + fileName;
        //        }

        //        var department = new Department
        //        {
        //            NameAR = departments.NameAR,
        //            NameEN = departments.NameEN,
        //            Logo = departmentlogo
        //        };

        //        _context.Add(department);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(departments);
        //}


        // GET: Department Details on partial view
        public async Task<IActionResult> partialDepartmentDetails(int id)
        {
            using (var client = new HttpClient())


            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(string.Format("api/GetDepartment/id={0}",id));
                //if (id == 0)
                //{
                //    id = _context.Departments.OrderBy(a => a.Id).First().Id;
                //}
                //var department = _context.Departments.FindAsync(id);
                //return PartialView(await department);
            }
            return PartialView();
        }

        // GET: Departments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _context.Departments.FindAsync(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(department);
        //}

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,NameAR,NameEN,Logo")] Department department)
        //{
        //    if (id != department.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(department);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DepartmentExists(department.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(department);
        //}

        // GET: Departments/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _context.Departments
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(department);
        //}

        // POST: Departments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var department = await _context.Departments.FindAsync(id);
        //    _context.Departments.Remove(department);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DepartmentExists(int id)
        //{
        //    return _context.Departments.Any(e => e.Id == id);
        //}
    }
}
