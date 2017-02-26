using Artifacts.Data;
using Artifacts.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artifacts.Services
{
    public class ImageService
    {
        private ApplicationDbContext _db;

        public ImageService(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}