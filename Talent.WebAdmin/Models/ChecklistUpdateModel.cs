﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class ChecklistUpdateModel
    {
        public TaskModel Task { get; set; }

        public string Question { get; set; }

        public int CompetencyTypeId { get; set; }

        public List<ChecklistChoiceModel> Choices { get; set; }

        public FileContent FileContent { get; set; }
    }
}
