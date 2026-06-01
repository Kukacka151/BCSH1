# Projekt C - Linky a zastavky

Reseni odpovida zadani z fotky:

- `linka.h` obsahuje strukturu `tZastavka` a deklarace funkci.
- `linka.c` obsahuje implementaci funkci pro vytvareni zastavek, nacitani linek,
  vlozeni prestupu a vypocet cesty.
- `main.c` nacte dve linky ze souboru, vlozi prestup a zavola funkci `cestuj`.
- `data/linka1.txt` a `data/linka2.txt` jsou ukazkove textove soubory s daty.

## Format textovych souboru

Kazdy radek obsahuje nazev zastavky a cas na dalsi zastavku:

```txt
zastavka1 4
zastavka2 3
```

Nazev zastavky je bez mezer a muze mit maximalne 19 znaku.

## Kompilace a spusteni

Ve slozce `C_Linky`:

```sh
make
./linky
```

Bez Makefile lze kompilovat i primo:

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
