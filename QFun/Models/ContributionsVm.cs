﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models
{
    public class ContributionsVm
    {
        public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
        public int ContributionsId { get; set; }
        public int ChallengeId { get; set; }
        public string Path { get; set; }
        public DateTime TimeOfUpload { get; set; }
        [Required(ErrorMessage = "Du måste ha en beskrivning för att ladda upp ett bidrag")]
        [MinLength(5, ErrorMessage = "Beskrivningen måste vara minst 5 bokstäver lång")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Challenge Challenge { get; set; }
        public List<Vote> Votes { get; set; } = new List<Vote>();

        [Required(ErrorMessage = "Du måste ladda upp en bild för att ladda upp ett bidrag")]
        public IFormFile Image { get; set; }
        public bool ShowImageError { get; internal set; } = true;
    }
}
