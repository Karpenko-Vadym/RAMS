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
            Mapper.CreateMap<RegisterAgentViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<RegisterClientViewModel, Client>();
            Mapper.CreateMap<RegisterAdminViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));

            Mapper.CreateMap<EditAgentViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<EditClientViewModel, Client>().ForMember(a => a.Role, map => map.MapFrom(vm => Role.Employee));
            Mapper.CreateMap<EditAdminViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));
        }
    }
}