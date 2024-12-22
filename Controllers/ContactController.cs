using AutoMapper;
using Microsoft.AspNetCore.Mvc;


public class ContactController:Controller
{
   
    public IActionResult Index()
    {        
        return View();
    }

    [HttpPost]
    public IActionResult Index(ContactVM model)
    {  
        if (!ModelState.IsValid)
    {
        return View(model);
    }
    return RedirectToAction("Index","Contact");

    }   

}


