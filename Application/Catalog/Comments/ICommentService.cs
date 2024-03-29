﻿using DaisyStudy.Models.Catalog.Comments;
using DaisyStudy.Models.Common;

namespace DaisyStudy.Application.Catalog.Comments;
public interface ICommentService
{
    Task<int> Create(CommentCreateRequest request);
    Task<int> Update(CommentUpdateRequest request);
    Task<int> Delete(int CommentID);
    Task<CommentViewModel> GetById(int NotificationID);
    Task<PagedResult<CommentViewModel>> GetAllPaging(GetManageCommentPagingRequest request);
}