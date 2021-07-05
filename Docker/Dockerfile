#Наследуемся от образа с mono.
FROM mono:latest as backend

#Копируем билд бэкенда.
COPY ODataBackend /backend

#Теперь переходим к самому web-серверу.
FROM flexberry/alt.p8-apache2-mono:5.18.0
 
MAINTAINER mail@flexberry.net 

#Копируем заготовленные файлы backend в папку веб сервера. /var/www/vhosts является для apache путем по умолчанию.
COPY --from=backend /backend /var/www/vhosts/backend

#Копируем конфиг для apach. 
COPY DockerConfig/vhosts.conf /conf

#Копируем скрипт установки переменных окружения.
COPY DockerConfig/prepareConf.sh /opt

#Делаем Web.Docker.config основным конфигом.
RUN mv /var/www/vhosts/backend/Web.Docker.config /var/www/vhosts/backend/Web.config 

#При старте контейнера запускаются подготовительные скрипты настройки
CMD bash -x /opt/prepareConf.sh