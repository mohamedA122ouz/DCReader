using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DCReader.Models;
using System.Text.RegularExpressions;

namespace DCReader.Controllers;

public class HomeController : Controller
{
  Document document = new Document(new TextDoc());
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
  public IActionResult Index(IFormFile file, string UserInput)
  {
    Regex rg = new Regex("^\\w+ -[chp]|(cs)$", RegexOptions.Multiline);
    if (!rg.IsMatch(UserInput))
    {
      ViewData["IsError"] = "true";
      ViewData["Messeg"] = "Input is not in correct form";
      return View();
    }
    if (file == null)
    {
      ViewData["IsError"] = "true";
      ViewData["Messeg"] = "Cant Upload Empty Data";
      return View();
    }
    string Extension = Path.GetExtension(file.FileName).ToLower();
    if (Extension == ".mp3" || Extension == "wav")
    {
      IDoc AudioDoc = new AudioDoc();
      document.ChangeState(AudioDoc);
      document.read(file);
    }
    else if (Extension == ".mp4")
    {
      IDoc VedioDoc = new VedioDoc();
      document.ChangeState(VedioDoc);
      document.read(file);
    }
    else if (Extension == ".txt" || Extension == "")
    {
      IDoc TextDoc = new TextDoc();
      document.ChangeState(TextDoc);
      document.read(file);

    }
    else if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg")
    {
      IDoc ImageDoc = new ImageDoc();
      document.ChangeState(ImageDoc);
      document.read(file);
    }
    else
    {
      ViewData["IsError"] = "true";
      ViewData["Messeg"] = "Not Supported  Type";
      return View();
    }

    List<string> lines = UserInput.Split("\n").ToList();
    List<string> Type = new();
    List<string> Keys = new();
    Dictionary<string, string> CLevel = new()
    {
        { "c", "Critical" },
        { "h", "Highly secrete" },
        { "cs", "Contain Senstive Data" },
        { "p", "Private" }
    };

    foreach (string line in lines)
    {
      string str = line.Trim();
      var ls = str.Split("-");
      str = ls[0].Trim();
      Type.Add(ls[1].Trim());
      Keys.Add(str);
    }
    Dictionary<string, List<Position>> entries = document.SearchList(Keys);
    int ik = 0;
    int CrLevel = 0;//public = 0,Private = 1,...,Critical = 4
    string CrLevelChar = "";
    string message = "";
    foreach (string key in Keys)
    {
      try
      {
        entries.TryGetValue(key, out List<Position>? positions);
        if (positions.Count <= 0) continue;
        message += $"The word `{key}` which has Privacy level {CLevel[Type[ik]]} occure at Postions <br>(Row,Column)<br>";
        foreach (Position p in positions)
        {
          message += $"({p.Row},{p.Column})<br>";
        }
        if (Type[ik] == "p" && CrLevel < 1) { CrLevel = 1; CrLevelChar = "p"; }
        else if (Type[ik] == "cs" && CrLevel < 2) { CrLevel = 2; CrLevelChar = "cs"; }
        else if (Type[ik] == "h" && CrLevel < 3) { CrLevel = 3; CrLevelChar = "h"; }
        else if (Type[ik] == "c" && CrLevel < 4) { CrLevel = 4; CrLevelChar = "c"; }
        ik++;
      }
      catch { }
    }
    string finalMessage = "";
    if (message.Length != 0)
      finalMessage = $"This file Marked As ({CLevel.GetValueOrDefault(CrLevelChar)}):<br> {message}";
    else
      finalMessage = "This file Marked As (Public) No entries Found";

    ViewData["IsError"] = "false";
    ViewData["output"] = finalMessage;
    ViewData["crLevel"] = CrLevel;

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
