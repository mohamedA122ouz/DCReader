using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DCReader.Models;

namespace DCReader.Controllers;

public class HomeController : Controller
{   
    Document document=new Document(new TextDoc());
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
         
        return View();
        
    }
    [HttpPost]
    public IActionResult Index(IFormFile file,string FileType,string UserInput)
    {
        if(file==null){
           ViewData["IsError"]="true";
           ViewData["Messeg"]="Cant Upload Empty Data"; 
           return View();
        }
        Trie trie=new Trie();
        if(FileType=="video"){
            IDoc doc=new VedioDoc();
            document.ChangeState(doc);
            document.read(file);
        }
        else if(FileType=="audio"){
            IDoc doc=new AudioDoc();
            document.ChangeState(doc);
            document.read(file);
        }
        else if(FileType=="text"){
            IDoc doc=new TextDoc();
            document.ChangeState(doc);
            document.read(file);
        }
        else if(FileType=="image"){
            IDoc doc=new ImageDoc();
            document.ChangeState(doc);
            document.read(file);
        }
        var data=document.Search("dollars");
        ViewData["IsError"]="false";
        ViewData["Messeg"]=data; 

        return View();
        
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
}
