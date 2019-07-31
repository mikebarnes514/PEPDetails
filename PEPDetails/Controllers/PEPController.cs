using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PEPDetails.Models;
using PEPDetails.Models.Data;

namespace PEPDetails.Controllers
{
    public class PEPController : Controller
    {
        private PEP_Test2Context _context;

        public PEPController(PEP_Test2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Project.Include(p => p.Cmim).Include(p => p.BillingAtty).Include(p => p.ResponsibleAtty).ToList());
        }
                
        public async Task<IActionResult> ProjectDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.BillingAtty)
                .Include(p => p.Cmim)
                .Include(p => p.ResponsibleAtty)
                .Include(p => p.ProjectType)
                .Include(p => p.Billing)
                .Include(p => p.BillingHistory)
                .Include(p => p.ProjectTask)
                
                .SingleOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            foreach (ProjectTask pt in project.ProjectTask)
            {
                _context.Task.Where(t => t.Id == pt.TaskId).Load();
                _context.Stage.Where(s => s.Id == pt.Task.StageId).Load();
            }

            var viewmodel = new ProjectDetailViewModel(project);
            viewmodel.ProjectTypes = _context.ProjectType.ToList();
            viewmodel.ActivityTypes = _context.ActivityType.ToList();

            return View(viewmodel);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjectDetails(int id, string quote, int typeId, DateTime start, DateTime meeting, DateTime deadline, DateTime signing, DateTime mailing, string notes)
        {
            DateTime lastTaskDate = DateTime.MinValue;
            int lastTaskId = -1;

            if (!_context.Project.Any(p => p.Id == id))
                return NotFound();

            var project = _context.Project.Include(p=>p.Billing).Include(p=>p.ProjectTask).Single(p => p.Id == id);

            decimal.TryParse(quote, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out decimal dQuote);
            project.ProjectTypeId = typeId;
            project.Notes = notes;
            project.Billing.Quote = dQuote;
            project.ProjectStartDate = start == DateTime.MinValue ? (DateTime?)null : start.Date;
            project.InitialMeetingDate = meeting == DateTime.MinValue ? (DateTime?)null : meeting.Date;
            project.SigningDeadline = deadline == DateTime.MinValue ? (DateTime?)null : deadline.Date;
            project.SigningDate = signing == DateTime.MinValue ? (DateTime?)null : signing.Date;
            project.MailingDate = mailing == DateTime.MinValue ? (DateTime?)null : mailing.Date;

            /* update all project task information */
            foreach(ProjectTask t in project.ProjectTask)
            {
                var tasknotes = Request.Form[String.Format("task-{0}-notes", t.Id)].FirstOrDefault();

                if (this.Request.Form.Keys.Contains(String.Format("task-{0}-complete", t.Id)))
                {
                    var completeDate = Request.Form[String.Format("task-{0}-date", t.Id)].FirstOrDefault();
                
                    t.IsComplete = true;
                    t.CompletionDate = completeDate == null ? DateTime.Today : DateTime.Parse(completeDate);
                    if(t.Id > lastTaskId)
                    {
                        lastTaskId = t.Id;
                        lastTaskDate = t.CompletionDate.Value;
                    }
                }
                else
                {
                    t.IsComplete = false;
                    t.CompletionDate = null;
                }

                t.Notes = tasknotes == null ? "" : tasknotes;
            }

            if(lastTaskId > -1)
            {
                project.CurrentTaskId = lastTaskId + 1;
                project.LastTaskDate = lastTaskDate;
            }

            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProjectDetails", new { id });
        }

        public async Task<IActionResult> ProjectView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.BillingAtty)
                .Include(p => p.Cmim)
                .Include(p => p.ResponsibleAtty)
                .Include(p => p.ProjectType)
                .Include(p => p.Billing)
                .Include(p => p.BillingHistory)
                .Include(p => p.ProjectTask)

                .SingleOrDefaultAsync(m => m.Id == id);

            if ((project == null) || !project.IsComplete)
            {
                return NotFound();
            }

            foreach (ProjectTask pt in project.ProjectTask)
            {
                _context.Task.Where(t => t.Id == pt.TaskId).Load();
                _context.Stage.Where(s => s.Id == pt.Task.StageId).Load();
            }

            var viewmodel = new ProjectDetailViewModel(project);
            viewmodel.ProjectTypes = _context.ProjectType.ToList();
            viewmodel.ActivityTypes = _context.ActivityType.ToList();

            return View(viewmodel);
        }

        public ActionResult Close()
        {
            return View();
        }

        public IActionResult LoadActivities()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var activityId = Request.Form["activityId"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int id = Convert.ToInt32(activityId);
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //var customerData = (from tempcustomer in _context.CustomerTB select tempcustomer);
                var activities = (from pa in _context.ProjectActivity where pa.ProjectId == id select new { pa.Id, Date = pa.ActivityDate.ToShortDateString(), pa.ActivityType.Code, pa.Notes });

                //if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection)))
                //    activities = activities.OrderBy(sortColumn + " " + sortColumnDirection);

                if (!string.IsNullOrEmpty(searchValue))
                    activities = activities.Where(m => m.Notes.Contains(searchValue));

                recordsTotal = activities.Count();

                //var data = activities.Skip(skip).Take(pageSize).ToList();
                var data = activities;
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult UpdateQuote(int id, string amount)
        {
            decimal remaining = -1;
            decimal overrun = -1;

            if(decimal.TryParse(amount, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out decimal newQuote))
            {
                if(_context.Project.Any(p=>p.Id == id))
                {
                    var project = _context.Project.Include(p => p.Billing).Include(p => p.BillingHistory).Single(p => p.Id == id);
                    project.Billing.Quote = newQuote;
                    _context.Update(project);

                    var model = new ProjectDetailViewModel(project);
                    remaining = model.RemainingBudget;
                    overrun = model.BudgetOverrun;
                }
            }

            return Json(new { budget = remaining, overrun });
        }

        [HttpPost]
        public IActionResult UpdateStartDate(int id, DateTime start)
        {
            decimal billed = -1;
            decimal unbilled = -1;
            decimal budget = -1;
            decimal overrun = -1;

            if (_context.Project.Any(p => p.Id == id))
            {
                var project = _context.Project.Include(p => p.Billing).Include(p => p.BillingHistory).Single(p => p.Id == id);
                project.ProjectStartDate = start;
                _context.Update(project);

                var model = new ProjectDetailViewModel(project);
                billed = model.BilledAmount;
                unbilled = model.ProjectDetail.Billing.UnbilledTotal.HasValue ? model.ProjectDetail.Billing.UnbilledTotal.Value : 0;
                budget = model.RemainingBudget;
                overrun = model.BudgetOverrun;
            }

            return Json(new { billed, unbilled, budget, overrun });
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(int projectId, int activityTypeId, DateTime activityDate, string notes)
        {
            int success = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    ProjectActivity activity = new ProjectActivity();

                    activity.ProjectId = projectId;
                    activity.ActivityTypeId = activityTypeId;
                    activity.ActivityDate = activityDate;
                    activity.Notes = notes;
                    _context.Add(activity);
                    await _context.SaveChangesAsync();
                    success = 1;
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to add activity. Try again and if the problem persists see your system administrator.");
            }

            return Json(success);
        }     
        
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int projectId, int projectTaskId, DateTime completed, string notes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectTask = _context.ProjectTask.Include(t=>t.Task).Single(t => t.Id == projectTaskId);
                    var project = _context.Project.Single(p => p.Id == projectId);

                    projectTask.IsComplete = true;
                    projectTask.CompletionDate = completed;
                    projectTask.Notes = notes;
                    _context.Update(projectTask);

                    project.LastTaskDate = completed;
                    project.CurrentTaskId = FindNextTaskId(projectId, projectTaskId);
                    await _context.SaveChangesAsync();
                    return Json(new { taskId = projectTaskId, stageId = projectTask.Task.StageId, completeDate = completed, notes });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to add activity. Try again and if the problem persists see your system administrator.");
            }

            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProject(int id)
        {
            if (!_context.Project.Any(p => p.Id == id))
                return NotFound();

            var project = _context.Project.Include(p => p.Billing).Single(p => p.Id == id);

            project.IsComplete = true;
            project.CompletionDate = DateTime.Today;
            _context.Update(project);
            await _context.SaveChangesAsync();

            return Redirect("/PEP/PEP/ProjectView/" + id);
        }

        private int FindNextTaskId(int projectId, int projectTaskId)
        {
            int nextId = -1;
            List<int> orderedTasks = new List<int>();

            foreach (ProjectTask t in (from pt in _context.ProjectTask where pt.ProjectId == projectId && pt.IsComplete == false orderby pt.Task.Stage.SortOrder, pt.Task.StageOrder select pt))
                orderedTasks.Add(t.Id);

            if (orderedTasks.Contains(projectTaskId) && (orderedTasks.IndexOf(projectTaskId) < (orderedTasks.Count - 1)))
                 nextId = orderedTasks.ElementAt(orderedTasks.IndexOf(projectTaskId) + 1);

            return nextId;
        }        
    }
}