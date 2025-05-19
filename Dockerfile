# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Exponha apenas a porta principal usada pela API
EXPOSE 8080

# Imagem para build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solução
COPY Contato.Cadastrar.Web.sln . 

# Copia os projetos
COPY Contato.Cadastrar.Web/Contato.Cadastrar.Web.csproj Contato.Cadastrar.Web/
COPY Contato.Cadastrar.Application/Contato.Cadastrar.Application.csproj Contato.Cadastrar.Application/
COPY Contato.Cadastrar.Domain/Contato.Cadastrar.Domain.csproj Contato.Cadastrar.Domain/
COPY Contato.Cadastrar.Infra/Contato.Cadastrar.Infra.csproj Contato.Cadastrar.Infra/

# Restaura dependências
RUN dotnet restore Contato.Cadastrar.Web.sln

# Copia o restante do código
COPY . .

# Build do projeto
WORKDIR /src/Contato.Cadastrar.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publica a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final: imagem de runtime
FROM base AS final
WORKDIR /app

# Garante que a aplicação escute na porta correta
ENV ASPNETCORE_URLS=http://+:8080

# Copia os arquivos publicados
COPY --from=publish /app/publish .

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "Contato.Cadastrar.Web.dll"]