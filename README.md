# Лабораторная работа №1 (Docker)

Приложение, созданное на основе тестового DockerApi, которое предоставляет .NET:

```sh
dotnet new  webapi -n DockerAPI
```

К нему добавлен контроллер со счетчиком.

## Запуск одного приложения

```sh
docker build -t sergejoz/dockerapi .
docker run -p 8080:80 sergejoz/dockerapi
```

## Автоматизированный запуск нескольких приложений

```sh
docker-compose up
```

Запускает три приложения на портах 8181, 8282, 8383 и балансировщик NGINX на :81. За это отвечает [docker-compise.yml](https://github.com/sergejoz/dockerapp/blob/master/docker-compose.yml).

## Пояснение по [Dockerfile](https://github.com/sergejoz/dockerapp/blob/master/Dockerfile)

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
## Запуск утилиты ab

```sh
ab -c 100 -n 10000 http://localhost:81/
```
Результат:
```s
Concurrency Level:      100
Time taken for tests:   1.833 seconds
Complete requests:      10000
Failed requests:        8003
   (Connect: 0, Receive: 0, Length: 8003, Exceptions: 0)
Total transferred:      1478003 bytes
HTML transferred:       38003 bytes
Requests per second:    5456.84 [#/sec] (mean)
Time per request:       18.326 [ms] (mean)
Time per request:       0.183 [ms] (mean, across all concurrent requests)
Transfer rate:          787.62 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.3      0      12
Processing:     0   18  14.6     15     123
Waiting:        0   18  14.6     14     123
Total:          1   18  14.7     15     124

Percentage of the requests served within a certain time (ms)
  50%     15
  66%     19
  75%     23
  80%     25
  90%     34
  95%     45
  98%     59
  99%     86
 100%    124 (longest request)
```

Запустим с большим количеством:
```sh
ab -c 100 -n 100000 http://localhost:81/
```
Результат:
```s
oncurrency Level:      100
Time taken for tests:   126.565 seconds
Complete requests:      100000
Failed requests:        81001
   (Connect: 0, Receive: 0, Length: 81001, Exceptions: 0)
Total transferred:      14881001 bytes
HTML transferred:       481001 bytes
Requests per second:    790.11 [#/sec] (mean)
Time per request:       126.565 [ms] (mean)
Time per request:       1.266 [ms] (mean, across all concurrent requests)
Transfer rate:          114.82 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0   29 646.5      0   64118
Processing:     0   95 733.2      7   60044
Waiting:        0   95 733.2      7   60044
Total:          0  124 975.4      7   64121

Percentage of the requests served within a certain time (ms)
  50%      7
  66%     11
  75%     13
  80%     15
  90%     26
  95%   1024
  98%   1037
  99%   3023
 100%  64121 (longest request)
```
# Реализация запуска приложения в Kubernetes
[Гайд по minikube](https://minikube.sigs.k8s.io/docs/start/)

Для начала установлен Minikube. VM уже был установлен на машине.
Запускаем minikube:
```sh
minikube start
```

Видим, что он корректно запустился:
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/start.png)

Для деплоя был взят пример hello-minikube.
```sh
kubectl create deployment hello-minikube --image=k8s.gcr.io/echoserver:1.4
kubectl expose deployment hello-minikube --type=NodePort --port=8080
kubectl get services hello-minikube
minikube service hello-minikube
```
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/service%20hello-minikube.png)

Успешно запущен.
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/browser%20answer.png)
Попробуем перекинуть на другой порт:
```sh
kubectl port-forward service/hello-minikube 7080:8080
```
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/mapping.png)
## Dashboard
Запуск dashboard:
```sh
minikube dashboard
```
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/dashboard%20cmd.png)

Вид dashboard:
![Screenshot](https://github.com/sergejoz/dockerapp/blob/master/Minikube/dashboard%20view.png)

## Управление Minikube
Пауза: 
```sh
minikube pause
```
Остановка:
```sh
minikube stop
```

Удаление всех кластеров:
```sh
minikube delete --all
```

Update: добавлен yml файл ([Dockerfile](https://github.com/sergejoz/dockerapp/blob/master/Minikube/minikube.yml)).
