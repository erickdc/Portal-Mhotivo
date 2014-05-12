﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Logic;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IMeisterRepository _meisterRepository;
        private readonly AcademicYearLogic _academicYearLogic;

        public AcademicYearController(IAcademicYearRepository academicYearRepository, IMeisterRepository meisterRepository, AcademicYearLogic academicYearLogic)
        {
            _academicYearRepository = academicYearRepository;
            _meisterRepository = meisterRepository;
            _academicYearLogic = academicYearLogic;
        }

        public ActionResult Management()
        {
            var elements = new AcademicYearViewManagement
            {
                Elements = _academicYearRepository.Filter(x => x.IsActive).ToList().Select(x => new AcademicYearViewData
                {
                    Approved = x.Approved ? "Active" : "Inactive",
                    Course = x.Course.Name,
                    Grade = x.Grade.Name,
                    Id = x.Id,
                    EndDate = (x.TeacherEndDate == null ? "Sin Maestro Asignado" : x.TeacherEndDate.Value.ToShortDateString()),
                    Limit = x.StudentsLimit,
                    Meister = x.Teacher == null ? "Sin Maestro Asignado" : x.Teacher.FullName,
                    Room = x.Room.IsEmpty() ? "Sin Aula Asignada" : x.Room,
                    Schedule = x.Schedule == null ? "Sin Maestro Asignado" : x.Schedule.Value.ToShortTimeString(),
                    Section = x.Section,
                    StartDate = x.TeacherStartDate == null ? "Sin Maestro Asignado" : x.TeacherStartDate.Value.ToShortDateString(),
                    Year = x.Year.Year
                }),
                CurrentYear = DateTime.Now.Year,
                CanGenerate = true
            };

            return View(elements);

        }

        [HttpGet]
        public ActionResult SelectNewTeacher(long id)
        {
            var meisters = _meisterRepository.Query(x => x);
            ViewBag.AcademicYearId = id;
            return View(meisters);
        }

        [HttpGet]
        public ActionResult ChangeTeacher(long id,long teacherId)
        {
            var academicYear = _academicYearRepository.GetById(id);
            var meister = _meisterRepository.GetById(teacherId);
            academicYear.Teacher = meister;
            _academicYearRepository.Update(academicYear,false,false,false);
            _academicYearRepository.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ManagementPost()
        {
            _academicYearLogic.GenerateSectionForGrades();

            return RedirectToAction("Management");
        }
    }
}
