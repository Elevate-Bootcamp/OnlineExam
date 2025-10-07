﻿using MediatR;
using OnlineExam.Features.UserAnswers.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.UserAnswers.Queries
{
    public record GetUserAnswerHistoryQuery : IRequest<ServiceResponse<PagedUserAnswerHistoryDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SortBy { get; init; }
    }
}