# Projekt C - Linky a zastavky

Reseni odpovida zadani z fotky:

- `linka.h` obsahuje strukturu `tZastavka` a deklarace funkci.
- `linka.c` obsahuje implementaci funkci pro vytvareni zastavek, nacitani linek,
  vlozeni prestupu a vypocet cesty.
- `main.c` nacte dve linky ze souboru, vlozi prestup a zavola funkci `cestuj`.
- `data/linka1.txt` a `data/linka2.txt` jsou ukazkove textove soubory s daty.
- `Linky.sln` a `Linky.vcxproj` slouzi pro otevreni projektu ve Visual Studiu.

## Prenos projektu

Projekt je pripraveny jako obycejna prenositelna slozka. Staci zkopirovat celou
slozku `C_Linky` na flash disk, do ZIP souboru nebo na jiny pocitac.

Ve slozce musi zustat tato struktura:

```txt
C_Linky/
  Linky.sln
  Linky.vcxproj
  Linky.vcxproj.filters
  main.c
  linka.c
  linka.h
  data/
    linka1.txt
    linka2.txt
```

Nejsou zde pouzite zadne absolutni cesty. Visual Studio ma pracovni slozku
nastavenou na slozku projektu, aby program nasel soubory v `data`.

## Format textovych souboru

Kazdy radek obsahuje nazev zastavky a cas na dalsi zastavku:

```txt
zastavka1 4
zastavka2 3
```

Nazev zastavky je bez mezer a muze mit maximalne 19 znaku.

## Spusteni ve Visual Studiu

Otevri soubor:

```txt
Linky.sln
```

Pak spust projekt klavesou `F5` nebo `Ctrl+F5`.

## Kompilace bez Visual Studia

Pokud nechces pouzit Visual Studio, lze program zkompilovat primo:

```sh
gcc -std=c11 -Wall -Wextra -pedantic main.c linka.c -o linky
./linky
```

## Ukazkove zadani vstupu

```txt
1
zastavka1
2
zastavka31
```

Program pak pocita cestu z linky 1 na linku 2 pres prestup
`zastavka3 -> zastavka2`.
