FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoList/TodoList.csproj", "TodoList/"]
COPY ["TodoList.Domain/TodoList.Domain.csproj", "TodoList.Domain/"]
COPY ["TodoList.Infra.Data/TodoList.Infra.Data.csproj", "TodoList.Infra.Data/"]
RUN dotnet restore "TodoList/TodoList.csproj"
COPY . .
WORKDIR "/src/TodoList"
RUN dotnet build "./TodoList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./TodoList.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoList.dll"]