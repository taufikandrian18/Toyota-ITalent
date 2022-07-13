using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Teams
    {
        public Teams()
        {
            CoachEmployeeMappings = new HashSet<CoachEmployeeMappings>();
            RotateTeamMembersNextTeam = new HashSet<RotateTeamMembers>();
            RotateTeamMembersPreviousTeam = new HashSet<RotateTeamMembers>();
            TeamDetails = new HashSet<TeamDetails>();
            TeamMemberRequests = new HashSet<TeamMemberRequests>();
        }

        [Key]
        public int TeamId { get; set; }
        [Required]
        [StringLength(64)]
        public string TeamLeaderId { get; set; }

        [ForeignKey("TeamLeaderId")]
        [InverseProperty("Teams")]
        public virtual Employees TeamLeader { get; set; }
        [InverseProperty("Team")]
        public virtual ICollection<CoachEmployeeMappings> CoachEmployeeMappings { get; set; }
        [InverseProperty("NextTeam")]
        public virtual ICollection<RotateTeamMembers> RotateTeamMembersNextTeam { get; set; }
        [InverseProperty("PreviousTeam")]
        public virtual ICollection<RotateTeamMembers> RotateTeamMembersPreviousTeam { get; set; }
        [InverseProperty("Team")]
        public virtual ICollection<TeamDetails> TeamDetails { get; set; }
        [InverseProperty("Team")]
        public virtual ICollection<TeamMemberRequests> TeamMemberRequests { get; set; }
    }
}
