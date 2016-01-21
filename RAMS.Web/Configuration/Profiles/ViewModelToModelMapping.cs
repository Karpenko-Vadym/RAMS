using AutoMapper;
using RAMS.Enums;
using RAMS.Models;
using RAMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Web.Configuration
{
    /// <summary>
    /// ViewModelToModelMapping profile allows to configure view model to domain model mapping
    /// </summary>
    public class ViewModelToModelMapping : Profile
    {
        public ViewModelToModelMapping() : base("ViewModelToModelMapping") { }

        /// <summary>
        /// Configure method contains detailed configuration of each mapping 
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<AgentAddViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<ClientAddViewModel, Client>();
            Mapper.CreateMap<AdminAddViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));

            Mapper.CreateMap<AgentEditViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<ClientEditViewModel, Client>().ForMember(a => a.Role, map => map.MapFrom(vm => Role.Employee));
            Mapper.CreateMap<AdminEditViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));

            Mapper.CreateMap<UserEditProfileViewModel, Agent>();
            Mapper.CreateMap<UserEditProfileViewModel, Client>();
            Mapper.CreateMap<UserEditProfileViewModel, Admin>();

            Mapper.CreateMap<DepartmentAddViewModel, Department>();
            Mapper.CreateMap<DepartmentEditViewModel, Department>();

            Mapper.CreateMap<PositionAddViewModel, Position>();
        }
    }
}