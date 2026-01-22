FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Disable Visual Studio fallback folders
ENV NUGET_FALLBACK_PACKAGES=""

# Copy the entire Hrms.EmployeeService folder
COPY . .

# Move into API project folder
WORKDIR /src/Hrms.EmployeeService.Api

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Hrms.EmployeeService.Api.dll"]