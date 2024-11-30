using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Diagnostics;
using Presentation.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Presentation.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPresentation _presentation;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUser user;

        public HomeController(IPresentation _presentation, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment,IUser user)
        {
            this._presentation = _presentation;
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.user = user;
        }

        public async Task<IActionResult>  Index()
        {
            var presentations = await _presentation.GetAll();
            return View(presentations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> CreatePresentation()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreatePresentation(CreatePresentationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;


                if (model.Photo != null)
                {
                    try
                    {
                        string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");


                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Photo.FileName);


                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Photo.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "File upload failed: " + ex.Message);
                        return View(model);
                    }
                }


                var presentation = new PresentationModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    PresentationName = model.PresentationName,
                    PresentationDetails = model.PresentationDetails,
                    Photo = uniqueFileName
                };


                var result = await _presentation.InsertByAsync(presentation);

                if (result)
                {
                    Console.WriteLine("Posted");
                }
                else
                {
                    Console.WriteLine("Failled");
                }
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Presentation(int Id)
        {
            var Presentation = await _presentation.FindByIdAsync(Id);
            var singlePresentation = new SinglePresentationViewModel
            {
                presentationModel = Presentation,
            };
            return View(singlePresentation);
        }


        [HttpGet]
        public async Task<IActionResult> EditPresentation(int Id)
        {
            var presentation = await _presentation.FindByIdAsync(Id);
            var model = new EditViewmodel
            {
                Id = presentation.Id,
                Name = presentation.Name,
                PresentationName = presentation.PresentationName,
                PresentationDetails = presentation.PresentationDetails,
                
            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditPresentation(EditViewmodel model)
        {
            PresentationModel Model = await _presentation.FindByIdAsync(model.Id);
            if (Model == null) 
            {
                Model.PresentationName = model.PresentationName;
                Model.PresentationDetails = model.PresentationDetails;

            };
            var result = await _presentation.UpdateByAsync(Model);
           
            if (result)
            {
                return RedirectToAction( "Index");

            }
            
            return RedirectToAction("Index");
        }
    }

}

    

