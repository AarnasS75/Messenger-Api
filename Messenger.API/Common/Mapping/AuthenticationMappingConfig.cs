using Mapster;
using Messenger.Application.Services.Authentication.Common;
using Messenger.Contracts.Authentication;

namespace Messenger.API.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(des => des.Token,src => src.Token)
            .Map(des => des,src => src.User);
    }
}