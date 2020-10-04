# Лабораторная работа №1 (Docker)

Приложение, созданное на основе тестового DockerApi, которое предоставляет .NET:

```sh
dotnet new  webapi -n DockerAPI
```

К нему добавлен контроллер со счетчиком.

# Запуск одного контейнера

```sh
docker build -t sergejoz/dockerapi .
docker run -p 8080:80 sergejoz/dockerapi
```

# Автоматизированный запуск нескольких контейнеров

```sh
docker-compose up
```

Запускает три приложения на портах 8181, 8282, 8383 и балансировщик [NGINX] на :81.

# Пояснение по Dockerfile

Получаем .NET SDK от Microsoft:
```sh
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env 
WORKDIR /app
```
Копирует .csproj и [восстанавливает зависимости](https://docs.microsoft.com/ru-ru/dotnet/core/tools/dotnet-restore#:~:text=%D0%9A%D0%BE%D0%BC%D0%B0%D0%BD%D0%B4%D0%B0%20dotnet%20restore%20%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D1%83%D0%B5%D1%82%20NuGet,specified%20in%20the%20project%20file.):
```sh
COPY *.csproj ./
RUN dotnet restore
```

Копирует файлы проекта и билдит релиз версию:
```sh
COPY . ./ 
RUN dotnet publish -c Release -o out
```
Создаем image и вешаем его на 80 порт:
```sh
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "dockerapi.dll"]
```
