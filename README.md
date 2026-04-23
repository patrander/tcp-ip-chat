# 🚀 TCP Chat Application (.NET)

Ez a projekt egy modern, aszinkron, eseményvezérelt TCP kliens-szerver csevegőalkalmazás, amely C# és .NET (Generic Host) alapokon nyugszik. A projekt fő célja a magas szintű szoftverarchitekturális minták (SOLID, DI, Clean Architecture) gyakorlati alkalmazásának bemutatása volt egy alacsony szintű hálózati (TCP/IP) környezetben.

## ✨ Főbb funkciók

- **Aszinkron TCP Hálózat:** Stabil, nem blokkoló hálózati kommunikáció `NetworkStream` és `Task` alapokon.
- **Enterprise Architektúra:** Beépített Függőséginjektálás (Dependency Injection), konfigurációkezelés (`appsettings.json`) és naplózás a `.NET Generic Host` segítségével.
- **Háttérszolgáltatások (Worker Services):** A hálózati figyelés és a felhasználói bevitel külön `BackgroundService` szálakon fut.
- **Bővíthető Parancsrendszer (Command Pattern):** A szerver okosbotként is funkcionál, a parancsok (`/time`, `/roll`, stb.) futtatása interfész-alapú és dinamikusan injektált, így a végtelenségig bővíthető a meglévő kód módosítása nélkül (Open/Closed Principle).
- **Thread-Safe UI:** Színkódolt, időbélyeggel ellátott konzolos felhasználói felület, amely elkülönül a hálózati (Core) logikától.

## 🏗️ Mappastruktúra és Architektúra

A kódbázis mind a Kliens, mind a Szerver oldalon szigorú logikai szétválasztást követ:

├── Configuration/   # Konfigurációs modellek (appsettings.json kötés)
├── Core/            # A hálózati motor (IChatClient / IChatServer)
├── Commands/        # (Szerver) Parancsközpont és IChatCommand implementációk
├── Workers/         # Háttérben futó hosztolt szolgáltatások
└── UI/              # Eseményvezérelt vizuális megjelenítés


