using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo( "LMSControllerTests" )]
namespace LMS_CustomIdentity.Controllers
{
[Authorize(Roles = "Professor")]
public class ProfessorController : Controller
{

    private readonly LMSContext db;

    public ProfessorController(LMSContext _db)
    {
        db = _db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Students(string subject, string num, string season, string year)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        return View();
    }

    public IActionResult Class(string subject, string num, string season, string year)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        return View();
    }

    public IActionResult Categories(string subject, string num, string season, string year)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        return View();
    }

    public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        ViewData["cat"] = cat;
        return View();
    }

    public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        ViewData["cat"] = cat;
        ViewData["aname"] = aname;
        return View();
    }

    public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        ViewData["cat"] = cat;
        ViewData["aname"] = aname;
        return View();
    }

    public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
    {
        ViewData["subject"] = subject;
        ViewData["num"] = num;
        ViewData["season"] = season;
        ViewData["year"] = year;
        ViewData["cat"] = cat;
        ViewData["aname"] = aname;
        ViewData["uid"] = uid;
        return View();
    }

    /*******Begin code to modify********/


    /// <summary>
    /// Returns a JSON array of all the students in a class.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "dob" - date of birth
    /// "grade" - the student's grade in this class
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
    {
      var curClass = GetClass(subject, num, season, year);
      if (curClass == null)
        return Json(new { success = false });

      var enrolled = from e  in curClass.Enrolleds
                     select new
                     {
                       fname  = e.Student.FName,
                       lname  = e.Student.LName,
                       uid    = e.StudentId,
                       dob    = e.Student.Dob,
                       grade  = e.Grade
                     };

      return Json(enrolled.ToArray());
    }


    /// <summary>
    /// Returns a JSON array with all the assignments in an assignment category for a class.
    /// If the "category" parameter is null, return all assignments in the class.
    /// Each object in the array should have the following fields:
    /// "aname" - The assignment name
    /// "cname" - The assignment category name.
    /// "due" - The due DateTime
    /// "submissions" - The number of submissions to the assignment
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class, 
    /// or null to return assignments from all categories</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
    {
      var curClass = GetClass(subject, num, season, year);
      if (curClass == null)
        return Json(new { success = false });

      var assignments = from ac in curClass.AssignmentCategories
                        from a in ac.Assignments
                        select new
                        {
                          aname = a.Name,
                          cname = ac.Name,
                          due = a.Due,
                          submissions = a.Submissions.Count(),
                        };

      if (category != null)
        assignments = assignments.Where(x => x.cname == category);

      return Json(assignments.ToArray());
    }


    /// <summary>
    /// Returns a JSON array of the assignment categories for a certain class.
    /// Each object in the array should have the folling fields:
    /// "name" - The category name
    /// "weight" - The category weight
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
    {
      var cl = GetClass(subject, num, season, year);
      if (cl == null)
        return Json(new { success = false });

      var categories = from ac in cl.AssignmentCategories
                       select new
                       {
                         name = ac.Name,
                         weight = ac.Weight
                       };
 
      return Json(categories.ToArray());
    }


    /// <summary>
    /// Creates a new assignment category for the specified class.
    /// If a category of the given class with the given name already exists, return success = false.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The new category name</param>
    /// <param name="catweight">The new category weight</param>
    /// <returns>A JSON object containing {success = true/false} </returns>
    public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
    {
      var currClass = GetClass(subject, num, season, year);
      if (currClass == null)
          return Json( new {success = false} );

      if (currClass.AssignmentCategories.Any( ac => ac.Name == category))
          return Json( new {success = false} );

      db.AssignmentCategories.Add(new AssignmentCategory {
          Name      = category,
          Weight    = (uint)catweight,
          ClassId   = currClass.ClassId
      });
      db.SaveChanges();
      return Json( new {success = true} );
    }


    /// <summary>
    /// Creates a new assignment for the given class and category.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The new assignment name</param>
    /// <param name="asgpoints">The max point value for the new assignment</param>
    /// <param name="asgdue">The due DateTime for the new assignment</param>
    /// <param name="asgcontents">The contents of the new assignment</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
    {
      var cat = (from co in db.Courses
                where co.Subject == subject && co.Num == num
                from cl in co.Classes
                where cl.Season == season && cl.Year == year
                from ac in cl.AssignmentCategories
                where ac.Name == category
                select ac).FirstOrDefault();

      if (cat == null || cat.Assignments.Any(a => a.Name == asgname))
        return Json(new { success = false });

      db.Assignments.Add(new Assignment
      {
        Name = asgname,
        Points = (uint)asgpoints,
        Due = DateOnly.FromDateTime(asgdue),
        Contents = asgcontents,
        AcId = cat.AcId
      });
      db.SaveChanges();
      return Json(new { success = true });

    }


    /// <summary>
    /// Gets a JSON array of all the submissions to a certain assignment.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "time" - DateTime of the submission
    /// "score" - The score given to the submission
    /// 
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
    {
      var assignment = GetAssignment(subject, num, season, year, category, asgname);
      if (assignment == null)
        return Json(new { success = false });

      var submissions = from s in assignment.Submissions
                        select new
                        {
                          fname = s.Student.FName,
                          lname = s.Student.LName,
                          uid = s.StudentId,
                          time = s.Time,
                          score = s.Score
                        };
      return Json(submissions.ToArray());

    }


    /// <summary>
    /// Set the score of an assignment submission
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <param name="uid">The uid of the student who's submission is being graded</param>
    /// <param name="score">The new score for the submission</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
    {
      var assignment = GetAssignment(subject, num, season, year, category, asgname);
      var submission = assignment?.Submissions.FirstOrDefault(s => s.StudentId == uid);

      if (submission == null)
        return Json(new { success = false });

      submission.Score = (uint)score;
      db.SaveChanges();
      return Json(new { success = true });
    }


    /// <summary>
    /// Returns a JSON array of the classes taught by the specified professor
    /// Each object in the array should have the following fields:
    /// "subject" - The subject abbreviation of the class (such as "CS")
    /// "number" - The course number (such as 5530)
    /// "name" - The course name  
    /// "season" - The season part of the semester in which the class is taught
    /// "year" - The year part of the semester in which the class is taught
    /// </summary>
    /// <param name="uid">The professor's uid</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetMyClasses(string uid)
    {
      var query = from cl in db.Classes
                  where cl.PuId == uid
                  select new
                  {
                    subject = cl.Course.Subject,
                    number = cl.Course.Num,
                    name = cl.Course.Name,
                    season = cl.Season,
                    year = cl.Year
                  };

      return Json(query.ToArray());
    }


    // helpers

    /// <summary>
    /// Retrieves a class from the DB.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <returns>A class object if it exists--otherwise null.</returns>
    public Class? GetClass(string subject, int num, string season, int year)
    {
      var foundClass = (
          from co in db.Courses
          where co.Subject == subject && co.Num == num
          from cl in co.Classes
          where cl.Year == year && cl.Season == season
          select cl).FirstOrDefault();

      return foundClass;
    }

    /// <summary>
    /// Gets an assignment in the DB.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The category of the assignment</param>
    /// <param name="asgname">The assignment name.</param>
    /// <returns>An assignment object or null.</returns>
    public Assignment? GetAssignment(string subject, int num, string season, int year, string category, string asgname)
    {
      var assignment = (from co in db.Courses
              where co.Subject == subject && co.Num == num
              from cl in co.Classes
              where cl.Season == season && cl.Year == year
              from ac in cl.AssignmentCategories
              where ac.Name == category
              from a in ac.Assignments
              where a.Name == asgname
              select a).FirstOrDefault();
      return assignment;
    }


    /*******End code to modify********/
  }
}

