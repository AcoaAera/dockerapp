# Лабораторная работа №1 (Docker)

Приложение, созданное на основе тестового DockerApi, которое предоставляет .NET.
К нему добавлен контроллер со счетчиком.

# Запуск

```sh
docker build -t sergejoz/dockerapi .
docker run -p 8080:80 sergejoz/dockerapi
```
