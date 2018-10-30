FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 56587
EXPOSE 44395

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ServicioValidaAD_OpenShift/ServicioValidaAD_OpenShift.csproj ServicioValidaAD_OpenShift/
RUN dotnet restore ServicioValidaAD_OpenShift/ServicioValidaAD_OpenShift.csproj
COPY . .
WORKDIR /src/ServicioValidaAD_OpenShift
RUN dotnet build ServicioValidaAD_OpenShift.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ServicioValidaAD_OpenShift.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ServicioValidaAD_OpenShift.dll"]


