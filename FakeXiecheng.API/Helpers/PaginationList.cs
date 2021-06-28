using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.API.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; }
        public int TotalPages { get; set; }
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrev => CurrentPage > 1;

        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
        {
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
        }

        public static async Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> result)
        {
            var totalCount = await result.CountAsync();

            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);
            result = result.Take(pageSize);
            var items = await result.ToListAsync();

            return new PaginationList<T>(totalCount, currentPage, pageSize, items);
        }
    }
}