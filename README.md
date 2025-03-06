# Сервис для выгрузки в БД данных о курсах валют от ЦБ РФ

### Запуск:
docker compose down currencyratemonitor.db

docker compose build currencyratemonitor

docker compose run currencyratemonitor

### Функционал:
В config.json настраивается подключение к БД (при корректном развертывании приложения, эти параметры менять не надо), а также конфигурация Cron, которая указывающая, в какое время суток будут собираться данные фоновым сервисом (используются форматы ЧЧ:ММ:СС или  ЧЧ:ММ, если данные будут некорректны, будет использоваться 12:00)

При запуске приложения, стартует фоновый процесс для ежедневной выгрузки данных, а также пользователю дается возможность выгрузить курсы валют за последний месяц.
