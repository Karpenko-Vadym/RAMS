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
            Mapper.CreateMap<Agent, AgentAddViewModel>();
            Mapper.CreateMap<Client, ClientAddViewModel>();
            Mapper.CreateMap<Admin, AdminAddViewModel>();

            Mapper.CreateMap<Agent, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Client, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Admin, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));

            Mapper.CreateMap<Agent, AgentEditViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Client, ClientEditViewModel>().ForMember(c => c.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Admin, AdminEditViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));

            Mapper.CreateMap<Agent, UserConfirmationViewModel>();
            Mapper.CreateMap<Client, UserConfirmationViewModel>();
            Mapper.CreateMap<Admin, UserConfirmationViewModel>();

            Mapper.CreateMap<Agent, EditUserConfirmationViewModel>();
            Mapper.CreateMap<Client, EditUserConfirmationViewModel>();
            Mapper.CreateMap<Admin, EditUserConfirmationViewModel>();

            Mapper.CreateMap<Agent, AgentAddEditConfirmationViewModel>().ForMember(a => a.DepartmentName, map => map.MapFrom(vm => vm.Department.Name));
            Mapper.CreateMap<Client, ClientAddEditConfirmationViewModel>();
            Mapper.CreateMap<Admin, AdminAddEditConfirmationViewModel>();

            Mapper.CreateMap<Agent, UserEditProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(vm => vm.AgentId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentEmail, map=> map.MapFrom(model => model.Email)); 
            Mapper.CreateMap<Client, UserEditProfileViewModel>().ForMember(c => c.UserId, map => map.MapFrom(vm => vm.ClientId)).ForMember(c => c.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(c => c.CurrentEmail, map => map.MapFrom(model => model.Email));
            Mapper.CreateMap<Admin, UserEditProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(vm => vm.AdminId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email));

            Mapper.CreateMap<Department, DepartmentListViewModel>().ForMember(d => d.NumOfAgents, map => map.MapFrom(vm => vm.Agents.Count()));
            Mapper.CreateMap<Department, DepartmentAddViewModel>();
            Mapper.CreateMap<Department, DepartmentEditViewModel>();

            Mapper.CreateMap<Agent, AgentProfileDetailsViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Client, ClientProfileDetailsViewModel>().ForMember(c => c.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Admin, AdminProfileDetailsViewModel>().ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));

            Mapper.CreateMap<Notification, NotificationListViewModel>();

            Mapper.CreateMap<Position, PositionListViewModel>().ForMember(p => p.PositionIdForDisplay, map => map.MapFrom(model => model.PositionId.ToString("00000"))).ForMember(p => p.CategoryName, map => map.MapFrom(model => model.Category.Name)).ForMember(p => p.AssignedTo, map => map.MapFrom(model => model.Agent.FirstName + " " + model.Agent.LastName));
            Mapper.CreateMap<Position, PositionAddViewModel>();

            Mapper.CreateMap<Position, PositionConfirmationViewModel>().ForMember(p => p.Client, map => map.MapFrom(model => model.Client.FirstName + " " + model.Client.LastName)).ForMember(p => p.Category, map => map.MapFrom(model => model.Category.Name));
            Mapper.CreateMap<Position, PositionDetailsViewModel>().ForMember(p => p.Client, map => map.MapFrom(model => model.Client.FirstName + " " + model.Client.LastName)).ForMember(p => p.Category, map => map.MapFrom(model => model.Category.Name)).ForMember(p => p.AssignedTo, map => map.MapFrom(model => model.Agent.FirstName + " " + model.Agent.LastName)).ForMember(p => p.AgentId, map => map.MapFrom(model => model.Agent.AgentId));

        }
    }
}