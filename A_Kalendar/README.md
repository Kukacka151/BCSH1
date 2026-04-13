# Kalendář (WinForms) - semestrální práce BCSH1

## Co aplikace umí (implementováno)
- GUI WinForms aplikace
- Dnový pohled: vybereš datum a uvidíš
  - Události (název, popis, začátek/konec)
  - Poznámky 
  - Upomínky (datum+čas, text, hotovo/nesplněno)
- CRUD pro všechny 3 typy entit (přidat / upravit / smazat)
- Ukládání a načítání dat přes JSON soubory (bez externích knihoven)

## Data
- Soubor se vytvoří automaticky při prvním spuštění
- Umístění: složka `data/` v repozitáři (aplikace ji najde i po buildnutí)
- Formáty:
  - `data/events.json`
  - `data/notes.json`
  - `data/reminders.json`


## Zamýšlený progres 
- Vyhledávání (text v událostech/poznámkách)
- Export/import (např. JSON)
- "Přehled týdne" a jednoduché notifikace (hotovo/urgent)

