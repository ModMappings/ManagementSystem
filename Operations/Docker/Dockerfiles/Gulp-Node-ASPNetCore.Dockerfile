FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

#Setup node and gulp
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install -g gulp-cli
RUN npm install -g gulp

WORKDIR /solution

# Setup multi service building from the same solution and docker file.
ARG SERVICE_NAME
ARG SERVICE_TYPE

# Copy service type directory.
COPY ./Development/$SERVICE_TYPE/ ./
WORKDIR /solution/$SERVICE_NAME
RUN npm install
RUN gulp build
RUN dotnet publish -c Release -o /build/out

WORKDIR /build/out
RUN echo "dotnet $SERVICE_NAME.dll" >> entrypoint.sh
RUN chmod +x entrypoint.sh

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /app
COPY --from=build /build/out ./

CMD /app/entrypoint.sh
