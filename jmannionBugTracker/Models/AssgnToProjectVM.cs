using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jmannionBugTracker.Models
{
    public class AssignToProjectVM
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string ProjectName { get; set; }

        public MultiSelectList userList { get; set; }

        public string[] SelectedUsers { get; set;  }
    }
    public class ProjectDetails
    {

        public Project Project { get; set; }

        public AssignToProjectVM Assignusers { get; set; }
    }

}

