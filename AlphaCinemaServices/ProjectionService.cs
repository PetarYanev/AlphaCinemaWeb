﻿using AlphaCinemaData.Context;
using AlphaCinemaData.Models.Associative;
using AlphaCinemaServices.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaCinemaServices
{
    public class ProjectionService : IProjectionService
    {
        private readonly AlphaCinemaContext context;

        public ProjectionService(AlphaCinemaContext context)
        {
            this.context = context;
        }

        public IEnumerable<Projection> GetByTownId(int townId)
        {
            return this.context.Projections
                .Where(p => p.CityId == townId)
                .Include(p => p.OpenHour)
                .Where(p => p.OpenHour.Hours > DateTime.Now.Hour || (p.OpenHour.Hours == DateTime.Now.Hour && p.OpenHour.Minutes >= DateTime.Now.Minute))
                //Тук казваме че или отворения час е след настоящия или е равен на него, но минутите са след настоящите
                .Include(p => p.Movie)
                    .ThenInclude(m => m.MovieGenres)
                        .ThenInclude(mg => mg.Genre)
                .ToList();
        }

        public IEnumerable<Projection> GetTopProjections(int count)
        {
            return this.context.Projections
                .Include(p => p.OpenHour)
                .Include(p => p.Movie)
                    .ThenInclude(m => m.MovieGenres)
                        .ThenInclude(mg => mg.Genre)
                .GroupBy(p => p.Movie)
                .Select(group => group.First())
                .Take(count)
                .ToList();
        }
    }
}
