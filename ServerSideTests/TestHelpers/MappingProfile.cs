using AutoMapper;
using DataConductor.Server;

namespace Support.DataConductor.ServerTests.TestHelpers
{
    public static class MappingFactory
    {
        public static IMapper GetFlowMapper() => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new FlowMappingProfile())));


    }
}
