#get base sdk image from ms
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env 
WORKDIR /app

#copy the csproj file and restore any dependices via nuget
COPY *.csproj ./
RUN dotnet restore

#copy the project files and build out relase
COPY . ./ 
RUN dotnet publish -c Release -o out

# generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "dockerapi.dll"]
