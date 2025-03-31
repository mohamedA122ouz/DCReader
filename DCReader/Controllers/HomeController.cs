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
    public IActionResult Index(IFormFile file,string UserInput)
    {
        if(file==null){
           ViewData["IsError"]="true";
           ViewData["Messeg"]="Cant Upload Empty Data"; 
           return View();
        }
        string Extension=Path.GetExtension(file.FileName).ToLower();
        if(Extension==".mp3"||Extension=="wav"){
          IDoc AudioDoc=new AudioDoc();
          document.ChangeState(AudioDoc);
          document.read(file);
        } 
        else if(Extension==".mp4"){
          IDoc VedioDoc=new VedioDoc();
          document.ChangeState(VedioDoc);
          document.read(file);
        }
        else if(Extension==".text"||Extension==""){
          IDoc TextDoc=new TextDoc();
          document.ChangeState(TextDoc);
          document.read(file);

        }
        else if(Extension==".jpg"||Extension==".png"||Extension==".jpeg"){
            IDoc ImageDoc=new ImageDoc();
            document.ChangeState(ImageDoc);
            document.read(file);
        }
        else{ 
           ViewData["IsError"]="true";
           ViewData["Messeg"]="Not Supported  Type"; 
           return View();
        }
        List<string> lines=UserInput.Split("\n").ToList();
        string Type;
        foreach(string line in lines){
          string str=line.Trim();
          var ls=str.Split("-");
          str=ls[0].Trim();
          Type=ls[1].Trim();
        }

        var data=document.Search("Test");
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
