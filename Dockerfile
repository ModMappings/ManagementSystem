FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /solution

# Setup multi target building from the same solution and docker file.
ARG TARGET
RUN echo "Target: " $TARGET

# copy csproj and restore as distinct layers
COPY *.sln .
COPY API/*.csproj ./API/
COPY Auth/*.csproj ./Auth/
COPY Data.Core/*.csproj ./Data.Core/
COPY Data.EFCore/*.csproj ./Data.EFCore/
COPY Data.MCPImport/*.csproj ./Data.MCPImport/
RUN dotnet restore

# copy everything else and build app
COPY ./ ./
WORKDIR /solution/$TARGET
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime

# Setup multi target building from the same solution and docker file.
ARG TARGET
RUN echo "Target: " $TARGET

WORKDIR /app
COPY --from=build /solution/$TARGET/out ./
ENTRYPOINT ["dotnet", "$TARGET.dll"]
