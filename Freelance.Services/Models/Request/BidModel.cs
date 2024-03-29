﻿using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class BidModel : PaginationModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public double Rate { get; set; }
        public int UserProfileId { get; set; }
        public int JobId { get; set; }
        public EntityStatus Status { get; set; }
    }
}
