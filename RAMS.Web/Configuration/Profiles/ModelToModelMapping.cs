using AutoMapper;
using RAMS.Models;
using RAMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Web.Configuration
{
    /// <summary>
    /// ModelToModelMapping profile allows to configure domain model to model mapping
    /// </summary>
    public class ModelToModelMapping : Profile
    {
        public ModelToModelMapping() : base("ModelToModelMapping") { }

        /// <summary>
        /// Configure method contains detailed configuration of each mapping 
        /// </summary>
        protected override void Configure()
        {         
            /***** ARCHIVE MAPPING *****/

            Mapper.CreateMap<Position, Archive>().ForMember(a => a.ArchiveId, map => map.MapFrom(model => model.PositionId)).ForMember(a => a.CategoryName, map => map.MapFrom(model => model.Category.Name)).ForMember(a => a.ClientName, map => map.MapFrom(model => String.Format("{0} {1}", model.Client.FirstName, model.Client.LastName))).ForMember(a => a.AgentName, map => map.MapFrom(model => String.Format("{0} {1}", model.Agent.FirstName, model.Agent.LastName))).ForMember(a => a.InterviewedApplicants, map => map.MapFrom(model => model.Candidates.Where(c => c.Status == Enums.CandidateStatus.Interviewed).Count())).ForMember(a => a.TotalApplicants, map => map.MapFrom(model => model.Candidates.Count()));
            
            /***** END OF ARCHIVE MAPPING *****/
        }
    }
}