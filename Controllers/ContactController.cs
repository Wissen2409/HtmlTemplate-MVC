using AutoMapper;
using Microsoft.AspNetCore.Mvc;


public class ContactController : Controller
{
    private readonly SendMail _sendMail;
    public ContactController(SendMail sendMail)
    {
        _sendMail = sendMail;
    }
    public IActionResult Index()
    {
        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData, ("Home", "/"), ("Contact", null));
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactVM model)
    {
        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData, ("Home", "/"), ("Contact", null));
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            await _sendMail.Execute(model);
        }
        catch (Exception ex)
        {

            TempData["FailMessage"] = "Error occured ->" + ex.Message;
            return View(model);
        }
        TempData["SuccessMessage"] = "Mail Sent Succesfully";
        return RedirectToAction("Index", "Contact");
    }
}


