# Test tool
Skrypt ściąga z bazy danych liczbę gotówki na każdym z kont, następnie tworzy NUMBER_OF_QUERIES (zmienna na górze pliku) losowych HTTP requestów typu przelew na API zapamiętując wykonane zapytania. Następnie prosi o wciśnięcie enter po tym jak wszystkie żądania zostaną wykonane (przestaną się pojawiać logi na konsoli docker-compose'a). Następnie samemu oblicza jaki stan konta powinien być na każdym z kont i porównuje czy się zgadza z aktualnym w bazie danych.


## skypt do jmeter'a ---> test.py
poprostu uruchomic podac liczbe wierszy w pliku do testowania
wykonac test plan z przelewy_plan i podac tyle zapytan co wierszy w pliku testujacym
nacisnac enter w test.py i zobaczymy czy testy sie udaly


### Jak uruchomić
W celu ściągnięcia wymaganych bibliotek:

    pip/pip3 install -r requirements.txt

Uruchomienie samego skryptu:

    python main.py
   
**Do działania programu potrzebne jest żeby w tle działał uruchomiony program** 

## benchmark.py

Uruchomienie 

python performance-test.py [liczba requestow]

zmienna 'number_of_concurrent_tasks = 100' ustawia mozliwosc generowana 100 zapytan rownoczesnie


