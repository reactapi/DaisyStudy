﻿using DaisyStudy.Models.Catalog.Homeworks;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.Homeworks;

public interface IHomeworkService
{
    Task<int> Create(HomeworkCreateRequest request);
    Task<int> Update(HomeworkUpdateRequest request);
    Task<int> Delete(int ID);
    Task<HomeworkViewModel> GetById(int ID);
    Task<PagedResult<HomeworkViewModel>> GetAllPaging(GetManageHomeworkPagingRequest request);
    Task<PagedResult<HomeworkViewModel>> GetAllMyHomeworkPaging(GetManageHomeworkPagingRequest request);
    Task<PagedResult<HomeworkViewModel>> GetAllMyAdminHomeworkPaging(GetManageHomeworkPagingRequest request);
    Task<List<HomeworkViewModel>> GetAllMyAdminHomework(string? UserID);
    Task<List<HomeworkViewModel>> GetAll();
}