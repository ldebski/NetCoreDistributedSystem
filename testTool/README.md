# Test tool
Skrypt ściąga z bazy danych liczbę gotówki na każdym z kont, następnie tworzy NUMBER_OF_QUERIES (zmienna na górze pliku) losowych HTTP requestów typu przelew na API zapamiętując wykonane zapytania. Następnie prosi o wciśnięcie enter po tym jak wszystkie żądania zostaną wykonane (przestaną się pojawiać logi na konsoli docker-compose'a). Następnie samemu oblicza jaki stan konta powinien być na każdym z kont i porównuje czy się zgadza z aktualnym w bazie danych.

### Jak uruchomić
W celu ściągnięcia wymaganych bibliotek:

    pip/pip3 install -r requirements.txt

Uruchomienie samego skryptu:

    python main.py
   
**Do działania programu potrzebne jest żeby w tle działał uruchomiony program** 

##performance-test.py

Uruchomienie 

python performance-test.py [liczba request]


