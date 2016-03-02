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
    /// ModelToViewModelMapping profile allows to configure domain model to view model mapping
    /// </summary>
    public class ModelToViewModelMapping : Profile
    {
        public ModelToViewModelMapping() : base("ModelToViewModelMapping") { }

        /// <summary>
        /// Configure method contains detailed configuration of each mapping 
        /// </summary>
        protected override void Configure()
        {
            /***** USER MAPPING *****/

            Mapper.CreateMap<Agent, AgentAddViewModel>();
            Mapper.CreateMap<Client, ClientAddViewModel>();
            Mapper.CreateMap<Admin, AdminAddViewModel>();

            Mapper.CreateMap<Agent, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Client, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Admin, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));

            Mapper.CreateMap<Agent, AgentEditViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Client, ClientEditViewModel>().ForMember(c => c.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Admin, AdminEditViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));

            Mapper.CreateMap<Agent, UserConfirmationViewModel>();
            Mapper.CreateMap<Client, UserConfirmationViewModel>();
            Mapper.CreateMap<Admin, UserConfirmationViewModel>();

            Mapper.CreateMap<Agent, EditUserConfirmationViewModel>();
            Mapper.CreateMap<Client, EditUserConfirmationViewModel>();
            Mapper.CreateMap<Admin, EditUserConfirmationViewModel>();

            Mapper.CreateMap<Agent, AgentAddEditConfirmationViewModel>().ForMember(a => a.DepartmentName, map => map.MapFrom(model => model.Department.Name));
            Mapper.CreateMap<Client, ClientAddEditConfirmationViewModel>();
            Mapper.CreateMap<Admin, AdminAddEditConfirmationViewModel>();

            Mapper.CreateMap<Agent, UserEditProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.AgentId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email));
            Mapper.CreateMap<Client, UserEditProfileViewModel>().ForMember(c => c.UserId, map => map.MapFrom(model => model.ClientId)).ForMember(c => c.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(c => c.CurrentEmail, map => map.MapFrom(model => model.Email));
            Mapper.CreateMap<Admin, UserEditProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.AdminId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email));

            Mapper.CreateMap<Agent, AgentProfileDetailsViewModel>().ForMember(a => a.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName)));
            Mapper.CreateMap<Client, ClientProfileDetailsViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName)));
            Mapper.CreateMap<Admin, AdminProfileDetailsViewModel>().ForMember(a => a.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName)));

            Mapper.CreateMap<Agent, AgentAssignPositionViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.AgentIdForDisplay, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.Positions, map => map.MapFrom(model => model.Positions.Where(p => p.Status != Enums.PositionStatus.Closed).Count()));
            Mapper.CreateMap<Agent, AgentListViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.AgentIdForDisplay, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.Positions, map => map.MapFrom(model => model.Positions.Where(p => p.Status != Enums.PositionStatus.Closed).Count())).ForMember(a => a.Department, map => map.MapFrom(model => model.Department.Name));
            Mapper.CreateMap<Agent, AgentDetailsViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(a => a.PositionsCurrent, map => map.MapFrom(model => model.Positions.Where(p => p.Status != Enums.PositionStatus.Closed).Count())).ForMember(a => a.PositionsTotal, map => map.MapFrom(model => model.Positions.Count())).ForMember(a => a.Department, map => map.MapFrom(model => model.Department.Name));

            Mapper.CreateMap<Client, ClientListViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(c => c.ClientIdForDisplay, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(c => c.Positions, map => map.MapFrom(model => model.Positions.Count()));
            Mapper.CreateMap<Client, ClientDetailsViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName))).ForMember(c => c.Positions, map => map.MapFrom(model => model.Positions.Count()));

            /***** END OF USER MAPPING *****/
            /***** DEPARTMENT MAPPING *****/

            Mapper.CreateMap<Department, DepartmentListViewModel>().ForMember(d => d.NumOfAgents, map => map.MapFrom(model => model.Agents.Count())).ForMember(d => d.DepartmentIdForDisplay, map => map.MapFrom(model => model.DepartmentId.ToString("00000"))); ;
            Mapper.CreateMap<Department, DepartmentAddViewModel>();
            Mapper.CreateMap<Department, DepartmentEditViewModel>();
            Mapper.CreateMap<Department, DepartmentAddEditConfirmationViewModel>();

            /***** END OF DEPARTMENT MAPPING *****/
            /***** NOTIFICATION MAPPING *****/

            Mapper.CreateMap<Notification, NotificationListViewModel>();

            /***** END OF NOTIFICATION MAPPING *****/
            /***** POSITION MAPPING *****/

            Mapper.CreateMap<Position, PositionListViewModel>().ForMember(p => p.PositionIdForDisplay, map => map.MapFrom(model => model.PositionId.ToString("00000"))).ForMember(p => p.CategoryName, map => map.MapFrom(model => model.Category.Name)).ForMember(p => p.AssignedTo, map => map.MapFrom(model => String.Format("{0} {1}", model.Agent.FirstName, model.Agent.LastName)));
            Mapper.CreateMap<Position, PositionAddViewModel>();

            Mapper.CreateMap<Position, PositionConfirmationViewModel>().ForMember(p => p.Client, map => map.MapFrom(model => model.Client.FirstName + " " + model.Client.LastName)).ForMember(p => p.Category, map => map.MapFrom(model => model.Category.Name));
            Mapper.CreateMap<Position, PositionDetailsViewModel>().ForMember(p => p.Client, map => map.MapFrom(model => model.Client.FirstName + " " + model.Client.LastName)).ForMember(p => p.Category, map => map.MapFrom(model => model.Category.Name)).ForMember(p => p.AssignedTo, map => map.MapFrom(model => String.Format("{0} {1}",model.Agent.FirstName, model.Agent.LastName)));

            Mapper.CreateMap<Position, PositionEditViewModel>().ForMember(p => p.Candidates, map => map.MapFrom(model => Mapper.Map<List<Candidate>, List<CandidateListViewModel>>(model.Candidates)));

            Mapper.CreateMap<Position, PositionListForReportViewModel>().ForMember(p => p.PositionIdForDisplay, map => map.MapFrom(model => model.PositionId.ToString("00000"))).ForMember(p => p.CategoryName, map => map.MapFrom(model => model.Category.Name)).ForMember(p => p.AssignedTo, map => map.MapFrom(model => String.Format("{0} {1}", model.Agent.FirstName, model.Agent.LastName))).ForMember(p => p.ReportType, map => map.MapFrom(model => (model.Status == Enums.PositionStatus.Closed) ? "Final Report" : "Status Report" ));

            Mapper.CreateMap<Position, PositionReportDetailsViewModel>().ForMember(p => p.TotalCandidates, map => map.MapFrom(model => model.Candidates.Count())).ForMember(p => p.PositionIdForDisplay, map => map.MapFrom(model => model.PositionId.ToString("00000")));
            Mapper.CreateMap<Position, PositionReportDetailsForPrintViewModel>().ForMember(p => p.TotalCandidates, map => map.MapFrom(model => model.Candidates.Count())).ForMember(p => p.PositionIdForDisplay, map => map.MapFrom(model => model.PositionId.ToString("00000"))).ForMember(p => p.Candidates, map => map.MapFrom(model => Mapper.Map<List<Candidate>, List<CandidateReportDetailsViewModel>>(model.Candidates)));

            /***** END OF POSITION MAPPING *****/
            /***** CANDIDATE MAPPING *****/

            Mapper.CreateMap<Candidate, CandidateListViewModel>().ForMember(c => c.CandidateIdDisplay, map => map.MapFrom(model => model.CandidateId.ToString("00000"))).ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}",model.FirstName, model.LastName))).ForMember(c => c.Selected, map => map.MapFrom(model => (model.Interviews.Count() > 0)));
            Mapper.CreateMap<Candidate, CandidateEditViewModel>().ForMember(c => c.CandidateIdDisplay, map => map.MapFrom(model => model.CandidateId.ToString("00000")));
            Mapper.CreateMap<Candidate, CandidateEditConfirmationViewModel>().ForMember(c => c.CandidateIdDisplay, map => map.MapFrom(model => model.CandidateId.ToString("00000")));
            Mapper.CreateMap<Candidate, CandidateReportDetailsViewModel>().ForMember(c => c.CandidateIdDisplay, map => map.MapFrom(model => model.CandidateId.ToString("00000"))).ForMember(c => c.Selected, map => map.MapFrom(model => (model.Interviews.Count() > 0)));
            Mapper.CreateMap<Candidate, CandidateScheduleViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => String.Format("{0} {1}", model.FirstName, model.LastName)));

            /***** END OF CANDIDATE MAPPING *****/
            /***** INTERVIEW MAPPING *****/

            Mapper.CreateMap<Interview, InterviewListViewModel>().ForMember(i => i.Candidate, map => map.MapFrom(model => Mapper.Map<Candidate, CandidateScheduleViewModel>(model.Candidate)));
            
            /***** END OF INTERVIEW MAPPING *****/
            /***** CATEGORY MAPPING *****/

            Mapper.CreateMap<Category, CategoryListViewModel>().ForMember(c => c.NumOfPositions, map => map.MapFrom(model => model.Positions.Count())).ForMember(c => c.CategoryIdForDisplay, map => map.MapFrom(model => model.CategoryId.ToString("00000")));
            Mapper.CreateMap<Category, CategoryAddViewModel>();
            Mapper.CreateMap<Category, CategoryEditViewModel>();
            Mapper.CreateMap<Category, CategoryAddEditConfirmationViewModel>();

            /***** END OF CATEGORY MAPPING *****/

        }
    }
}