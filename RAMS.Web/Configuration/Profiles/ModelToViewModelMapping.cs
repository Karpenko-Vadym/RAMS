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
            Mapper.CreateMap<Agent, RegisterAgentViewModel>();
            Mapper.CreateMap<Client, RegisterClientViewModel>();
            Mapper.CreateMap<Admin, RegisterAdminViewModel>();

            Mapper.CreateMap<Agent, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Client, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));
            Mapper.CreateMap<Admin, UserListViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.FullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName));

            Mapper.CreateMap<Agent, EditAgentViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AgentId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Client, EditClientViewModel>().ForMember(c => c.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.ClientId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));
            Mapper.CreateMap<Admin, EditAdminViewModel>().ForMember(a => a.UserId, map => map.MapFrom(model => model.UserType.ToString().Substring(0, 2).ToUpper() + model.AdminId.ToString("00000"))).ForMember(a => a.CurrentUserName, map => map.MapFrom(model => model.UserName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentRole, map => map.MapFrom(model => model.Role.ToString()));

            Mapper.CreateMap<Agent, ConfirmationViewModel>();
            Mapper.CreateMap<Client, ConfirmationViewModel>();
            Mapper.CreateMap<Admin, ConfirmationViewModel>();

            Mapper.CreateMap<Agent, EditUserProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(vm => vm.AgentId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentEmail, map=> map.MapFrom(model => model.Email)); 
            Mapper.CreateMap<Client, EditUserProfileViewModel>().ForMember(c => c.UserId, map => map.MapFrom(vm => vm.ClientId)).ForMember(c => c.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(c => c.CurrentEmail, map => map.MapFrom(model => model.Email));
            Mapper.CreateMap<Admin, EditUserProfileViewModel>().ForMember(a => a.UserId, map => map.MapFrom(vm => vm.AdminId)).ForMember(a => a.CurrentFullName, map => map.MapFrom(model => model.FirstName + " " + model.LastName)).ForMember(a => a.CurrentEmail, map => map.MapFrom(model => model.Email));
        }
    }
}