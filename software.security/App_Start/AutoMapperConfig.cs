using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Software.Security.App_Start
{
    public class AutoMapperConfig
    {
        private readonly Container _container;

        public AutoMapperConfig(Container container)
        {
            _container = container;
        }

        public IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(_container.GetInstance);
            
            /// Mapping models
            mce.CreateMap<Database.Models.User, Software.Security.Models.Authorization.UserViewModel>();


            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            IMapper m = new Mapper(mc, t => _container.GetInstance(t));

            return m;
        }
        public class SomeProfile : Profile
        {
            //public SomeProfile()
            //{
            //    var map = CreateMap<MySourceType, MyDestinationType>();
            //    map.ForMember(d => d.PropertyThatDependsOnIoc, opt => opt.ResolveUsing<PropertyThatDependsOnIocValueResolver>());
            //}
        }
    }
   
}