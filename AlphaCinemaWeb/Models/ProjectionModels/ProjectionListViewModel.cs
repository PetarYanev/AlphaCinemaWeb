﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlphaCinemaWeb.Models.ProjectionModels
{
    public class ProjectionListViewModel
    {
        public ProjectionListViewModel()
        {

        }

        public ProjectionListViewModel(int currentPage, int maxPages, string titleSort, string hourSort,
            string sortOrder, int cityId, string userId, DayOfWeek day, IEnumerable<ProjectionViewModel> projections)
        {
            this.Day = day;
            this.UserId = userId;
            this.CityId = cityId;
            this.HourSort = hourSort;
            this.SortOrder = sortOrder;
            this.MaxPages = maxPages;
            this.TitleSort = titleSort;
            this.CurrentPage = currentPage;
            this.Projections = projections;
        }
        public int ProjectionId { get; set; }

        public string Image { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public string SortOrder { get; set; }

        public string UserId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CityId { get; set; }

        public string TitleSort { get; set; }

        public string HourSort { get; set; }

        public int? CurrentPage { get; set; }

        public int MaxPages { get; set; }

        public IEnumerable<ProjectionViewModel> Projections { get; set; }
    }
}
