using AutoMapper;

namespace Clinic.Core.Automapper
{
    public class CoreMapperWrapper : ICoreMapper
    {
        protected readonly IMapper _mapper;

        public CoreMapperWrapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IMapper Mapper { get { return _mapper; } }
    }
}
