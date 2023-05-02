FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy all .csproj files in subfolders of the src directory
COPY src/**/*.csproj ./

# Restore dependencies for each project
RUN for file in $(ls *.csproj); do \
    dotnet restore "$file"; \
done

# Copy the rest of the application code and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ETeamProjectApplication.dll"]