FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o arquivo .sln
COPY Contato.Cadastrar.Web.sln . 

# Copia os projetos necessários
COPY Contato.Cadastrar.Web/Contato.Cadastrar.Web.csproj Contato.Cadastrar.Web/
COPY Contato.Cadastrar.Application/Contato.Cadastrar.Application.csproj Contato.Cadastrar.Application/
COPY Contato.Cadastrar.Domain/Contato.Cadastrar.Domain.csproj Contato.Cadastrar.Domain/
COPY Contato.Cadastrar.Infra/Contato.Cadastrar.Infra.csproj Contato.Cadastrar.Infra/

# Restaura os pacotes
RUN dotnet restore Contato.Cadastrar.Web.sln

# Copia o restante dos arquivos
COPY . .

# Compila o projeto
WORKDIR /src/Contato.Cadastrar.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contato.Cadastrar.Web.dll"]
