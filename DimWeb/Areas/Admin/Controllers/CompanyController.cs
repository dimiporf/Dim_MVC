using Dim.Utility;
using DimWeb.DataAccess.Data;
using DimWeb.DataAccess.Repository.IRepository;
using DimWeb.Models;
using DimWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Metrics;

namespace DimWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

               public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

                   }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id) /* Update or Insert */
        {
            
             
            if (id == null || id == 0)
            {
                //create
                return View(new Company ());
            }
            else 
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u=> u.Id == id);
                return View(companyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
           if (ModelState.IsValid)
            {
                
                if(companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Company Saved Successfully!";
                return RedirectToAction("Index");
            }
           else
            {               
            return View(companyObj);
            }
        }

        
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null) { return NotFound(); };
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully!";
            return RedirectToAction("Index");
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete (int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u=>u.Id == id);
            if (companyToBeDeleted == null) 
            {
                return Json(new { success= false , message = "Error while deleting"});
            }

            
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
}
