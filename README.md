# NetCoreDistributedSystem
opis
# Uruchamianie
Uruchamianie projektu:

- docker-compose build
- docker-compose up --scale receiverprzelew=X --scale receiverget=Y

Polecenia te utworzą:
- API którego zadaniem jest pobrać żądanie i włożyc je do odpowiedniej kolejki
- RabbitMQ czyli kolejke
- Bazę danych SQL
- X procesów zajmujących się przelewami, Y procesów zajmujących się get'ami które biorą żądanie z kolejki i wykonują odpowiednie query na bazie danych


# SQL
W bazie danych tworzony jest database BankDataBase z tabelą do której umieszczane jest 1000 wartości (AccountID 0-999 i losowe CashAmount)

```
Account {
  AccountID int NOT NULL UNIQUE,
  CashAmount int
}
```
# API
API dostępne jest pod:
- http://localhost:8000/send/get/x - żądanie robi "SELECT CashAmount FROM Account WHERE AccountID = x":
- http://localhost:8000/przelew/x/y/amount - żądanie przelewa "amount" gotówki z konta o id x na konto o id y pod warunkiem że konto x ma na to środki
### Skrypt testujący
W folderze testTool jest skrypt testujący napisany w pythonie, zamieszczam tam oddzielne readme
