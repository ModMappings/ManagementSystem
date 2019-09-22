FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

WORKDIR /solution

# Setup multi service building from the same solution and docker file.
ARG SERVICE_NAME
ARG SERVICE_TYPE

# Copy service type directory.
COPY ./Development/$SERVICE_TYPE/ ./
WORKDIR /solution/$SERVICE_NAME
RUN dotnet publish -c Release -o /build/out

WORKDIR /build/out
RUN echo "dotnet $SERVICE_NAME.dll" >> entrypoint.sh
RUN chmod +x entrypoint.sh

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /app
COPY --from=build /build/out ./

CMD /app/entrypoint.sh
