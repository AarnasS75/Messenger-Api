# Base Image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ["Messenger.API/Messenger.API.csproj", "Messenger.API/"]
COPY ["Messenger.Infrastructure/Messenger.Infrastructure.csproj", "Messenger.Infrastructure/"]
COPY ["Messenger.Domain/Messenger.Domain.csproj", "Messenger.Domain/"]
COPY ["Messenger.Application/Messenger.Application.csproj", "Messenger.Application/"]
COPY ["Messenger.Contracts/Messenger.Contracts.csproj", "Messenger.Contracts/"]
COPY ["Message.Handlers.Messages/Message.Handlers.Messages.csproj", "Message.Handlers.Messages/"]
COPY ["Message.Handlers/Message.Handlers.csproj", "Message.Handlers/"]

RUN dotnet restore "Messenger.API/Messenger.API.csproj"

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0 
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Messenger.API.dll"]