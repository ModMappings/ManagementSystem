FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /solution

# copy csproj and restore as distinct layers
COPY *.sln .
COPY API/*.csproj ./API/
COPY Data.Core/*.csproj ./Data.Core/
COPY Data.EFCore/*.csproj ./Data.EFCore/
RUN dotnet restore

# copy everything else and build app
COPY ./ ./
WORKDIR /solution/API
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /app
COPY --from=build /solution/API/out ./
ENTRYPOINT ["dotnet", "API.dll"]
