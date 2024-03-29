﻿using AutoMapper;
using DaisyStudy.Application.Catalog.Classes;
using DaisyStudy.Application.Catalog.Homeworks;
using DaisyStudy.Models.Catalog.Homeworks;
using Microsoft.AspNetCore.Mvc;

namespace DaisyStudy.Controllers;

public class HomeworkController : BaseController
{
    private readonly IHomeworkService _homeworkService;
    private readonly IClassService _classService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public HomeworkController(IHomeworkService homeworkService,
                              IClassService classService,
                              IConfiguration configuration,
                              IMapper mapper)
    {
        _homeworkService = homeworkService;
        _classService = classService;
        _configuration = configuration;
        _mapper = mapper;
    }

    [Route("giao-vien/bai-tap")]
    public async Task<IActionResult> Index(int ClassID, string keyword, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageHomeworkPagingRequest()
        {
            UserId = HttpContext.Session.GetString("UserId"),
            ClassID = ClassID,
            Keyword = keyword,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        var data = await _homeworkService.GetAllMyAdminHomeworkPaging(request);
        ViewBag.Keyword = keyword;
        ViewBag.ClassID = ClassID;

        var list = await _classService.GetAllMyAdminClass(HttpContext.Session.GetString("UserId"));
        ViewBag.listCl = list;

        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }

        return View(data);
    }

    [Route("bai-tap")]
    public async Task<IActionResult> BaiTap(string keyword, int pageIndex = 1, int pageSize = 10)
    {
        var request = new GetManageHomeworkPagingRequest()
        {
            UserId = HttpContext.Session.GetString("UserId"),
            Keyword = keyword,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        var data = await _homeworkService.GetAllMyHomeworkPaging(request);
        ViewBag.Keyword = keyword;
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] HomeworkCreateRequest request)
    {
        if (request.SubmissionDateTime == new DateTime(0001, 01, 01, 00, 00, 00))
            request.SubmissionDateTime = DateTime.Now;

        if (request.HomeworkName == null || request.Deadline == null || request.Description == null)
            return View(request);
        var result = await _homeworkService.Create(request);
        if (result != null)
        {
            TempData["result"] = "Thêm mới bài tập thành công";
            return RedirectToAction("Details", "Class", new { id = request.ClassID });
        }

        ModelState.AddModelError("", "Thêm bài tập thất bại");
        return View(request);
    }

    [HttpGet("chi-tiet-bai-tap")]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _homeworkService.GetById(id);
        if (result == null) NotFound();
        if (TempData["result"] != null)
        {
            ViewBag.SuccessMsg = TempData["result"];
        }
        return View(result);
    }

    [HttpPost("gui-mail-bai-tap")]
    public async Task<IActionResult> SendMail(int ClassID, int HomeworkID)
    {
        var students = await _classService.GetAllStudentByClassIDD(ClassID);
        foreach (var item in students)
        {
            if (string.IsNullOrEmpty(item.Email)) return RedirectToAction("Details", "Homework", new { id = HomeworkID });
            DaisyStudy.Utilities.Helpers.SendMail.SendEmail(item.Email,
                                                            "Thông báo từ DaisyStudy",
                                                            "<p>\n    <img class=\"image_resized\" style=\"width:24.92%;\" src=\"https://localhost:5000/img/logo.png\" alt=\"logo-white.svg\">\n</p>\n<p>\n    <span style=\"font-family:'Trebuchet MS', Helvetica, sans-serif;font-size:20px;\"><mark class=\"marker-yellow\"><strong>Thông báo từ DaisyStudy</strong></mark></span>\n</p>\n<p>\n    <span style=\"font-family:'Trebuchet MS', Helvetica, sans-serif;\"><mark class=\"marker-yellow\">Lớp học của bạn vừa cập nhật một bài tập mới</mark></span>\n</p>\n<p>\n    <span style=\"font-family:'Trebuchet MS', Helvetica, sans-serif;\"><mark class=\"marker-yellow\">Nhấn vào &nbsp;</mark></span><a href=\"https://localhost:5000/\"><span style=\"font-family:'Trebuchet MS', Helvetica, sans-serif;\"><mark class=\"marker-yellow\">https://localhost:5000/</mark></span></a><span style=\"font-family:'Trebuchet MS', Helvetica, sans-serif;\"><mark class=\"marker-yellow\"> để kiểm tra.</mark></span>\n</p>\n",
                                                            "");
        }
        TempData["result"] = "Đã gửi mail thành công";
        return RedirectToAction("Details", "Homework", new { id = HomeworkID });
    }

    [HttpGet("chinh-sua-bai-tap")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _homeworkService.GetById(id);
        if (result == null) return NotFound();
        var homeworkViewModel = _mapper.Map<HomeworkViewModel, HomeworkUpdateRequest>(result);
        return View(homeworkViewModel);
    }

    [HttpPost("chinh-sua-bai-tap")]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] HomeworkUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _homeworkService.Update(request);
        TempData["result"] = "Cập nhật bài tập thành công";
        var homework = await _homeworkService.GetById(request.HomeworkID);
        return RedirectToAction("Details", "Homework", new { id = homework.HomeworkID });
    }

    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View();
        var homework = await _homeworkService.GetById(id);
        var result = await _homeworkService.Delete(id);
        if (result != 0)
        {
            TempData["result"] = "Xoá bài tập thành công";
            return RedirectToAction("Details", "Class", new { id = homework.ID });
        }

        ModelState.AddModelError("", "Xoá bài tập thất bại");
        return Redirect(returnUrl);
    }
}