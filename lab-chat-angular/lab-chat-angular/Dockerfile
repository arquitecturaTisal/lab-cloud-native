FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_11.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup
WORKDIR /app
EXPOSE 44353

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY lab-chat-angular/lab-chat-angular.csproj lab-chat-angular/
RUN dotnet restore lab-chat-angular/lab-chat-angular.csproj
COPY . .
WORKDIR /src/lab-chat-angular
RUN dotnet build lab-chat-angular.csproj -c Release -o /app


FROM build AS publish
RUN dotnet publish lab-chat-angular.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "lab-chat-angular.dll"]
