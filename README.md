# APBD1 - Wypożyczalnia sprzętu

## Opis projektu
Projekt przedstawia prostą aplikację konsolową do obsługi wypożyczalni sprzętu.
System pozwala dodawać użytkowników i sprzęt, obsługiwać wypożyczenia i zwroty,
blokować niedostępny sprzęt, liczyć kary za opóźnienia oraz generować raport końcowy.

Kod jest podzielony na model domenowy (`Users`, `Equipment`, `Rentals`), logikę aplikacyjną
(`Services`) oraz narzędzia pomocnicze (`Utils`).

## Struktura projektu
- `APBD1/Users/`
  - `User.cs` - abstrakcyjna klasa bazowa użytkownika
  - `Student.cs`, `Employee.cs` - konkretne typy użytkowników z limitami wypożyczeń
- `APBD1/Equipment/`
  - `Equipment.cs` - abstrakcyjna klasa bazowa sprzętu
  - `Laptop.cs`, `Projector.cs`, `Camera.cs` - konkretne typy sprzętu
  - `EquipmentStatus.cs` - statusy sprzętu (`Available`, `Rented`, `Unavailable`)
- `APBD1/Rentals/`
  - `Rental.cs` - encja wypożyczenia (daty, powiązania, kara)
- `APBD1/Services/`
  - `UserService.cs` - obsługa użytkowników
  - `EquipmentService.cs` - obsługa sprzętu i statusów
  - `RentalService.cs` - logika wypożyczania, zwrotów, walidacji i raportu
- `APBD1/Utils/`
  - `IdGenerator.cs` - generowanie identyfikatorów
  - `LateFeeCalculator.cs` - obliczanie kary za opóźnienie
- `APBD1/main.cs`
  - scenariusz testowy (kroki 11-17)

## Krótkie uzasadnienie decyzji projektowych
Wybrałam podział na katalogi domenowe + serwisy, bo:
- modele (`User`, `Equipment`, `Rental`) trzymają dane i podstawowe zachowania encji,
- serwisy trzymają operacje biznesowe na kolekcjach obiektów,
- utils zawierają logikę wspólną, która nie powinna być przypisana do jednej encji.

Dzięki temu łatwiej rozwijać projekt:
- nowy typ użytkownika lub sprzętu dodaje się w odpowiednim katalogu domenowym,
- reguły wypożyczeń modyfikuje się głównie w `RentalService`,
- zmiana sposobu naliczania kary wymaga zmian tylko w `LateFeeCalculator`.

## Gdzie widać kohezję, coupling i odpowiedzialności klas
### Kohezja (spójność odpowiedzialności)
- `UserService` odpowiada tylko za operacje na użytkownikach (`APBD1/Services/UserService.cs`).
- `EquipmentService` odpowiada tylko za operacje na sprzęcie i statusach (`APBD1/Services/EquipmentService.cs`).
- `RentalService` odpowiada za proces wypożyczenia/zwrotu i reguły limitów (`APBD1/Services/RentalService.cs`).
- `LateFeeCalculator` ma jedno zadanie: policzyć karę (`APBD1/Utils/LateFeeCalculator.cs`).

### Coupling (sprzężenie)
- `RentalService` zależy od `UserService` i `EquipmentService`, ale przez ich publiczne metody,
  a nie przez bezpośrednią manipulację ich stanem wewnętrznym.
- `EquipmentService.GetAllEquipmentData()` zwraca kopię listy (`ToList()`), co ogranicza
  ryzyko przypadkowej modyfikacji stanu przez inne klasy.
- `User` i `Equipment` są klasami abstrakcyjnymi, więc logika w serwisach działa na wspólnym
  kontrakcie zamiast na konkretnych klasach.

### Odpowiedzialności klas
- `User` określa `MaxActiveRentals` i `UserType` (`APBD1/Users/User.cs`),
  a `Student` i `Employee` definiują konkretne limity.
- `Equipment` przechowuje dane wspólne i stan sprzętu (`APBD1/Equipment/Equipment.cs`).
- `Rental` trzyma dane pojedynczego wypożyczenia i informacje o zwrocie (`APBD1/Rentals/Rental.cs`).

## Instrukcja uruchomienia
### Wymagania
- .NET SDK 10.0
- JetBrains Rider (opcjonalnie, jeśli uruchamiasz projekt z IDE)

### Opcja 1: klonowanie i uruchomienie w terminalu

```powershell
git clone https://github.com/s30535/APBD1.git
cd APBD1
dotnet restore .\APBD1.sln
dotnet run --project .\APBD1\APBD1.csproj
```

### Opcja 2: klonowanie i uruchomienie w JetBrains Rider
1. Otwórz Rider i wybierz Git -> Clone... 
2. Wklej adres repozytorium, np. `https://github.com/s30535/APBD1.git`
3. Kliknij `Clone`
4. Po otwarciu projektu uruchom plik `main.cs`


