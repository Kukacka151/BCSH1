# Spusteni projektu ve Visual Studiu

Nejjednodussi postup:

1. Otevri Visual Studio.
2. Zvol `Open a project or solution`.
3. Vyber soubor `C_Linky/Linky.sln`.
4. Nahoře zvol konfiguraci `Debug` a platformu `x64` nebo `x86`.
5. Spust program pomoci `F5` nebo `Ctrl+F5`.

Projekt je nastaveny jako konzolova aplikace v jazyce C. Pracovni slozka je
nastavena na slozku projektu, takze program najde textove soubory:

```txt
data/linka1.txt
data/linka2.txt
```

## Kdyz si projekt vytvaris ve Visual Studiu rucne

Vytvor `Empty Project` a pridej do nej tyto soubory:

- `main.c`
- `linka.c`
- `linka.h`
- slozku `data` se soubory `linka1.txt` a `linka2.txt`

Potom zkontroluj:

```txt
Configuration Properties -> Debugging -> Working Directory
```

Nastav tam slozku, kde mas `main.c` a slozku `data`.

Pokud Visual Studio hlasi varovani ke `scanf`, pridej do nastaveni projektu:

```txt
Configuration Properties -> C/C++ -> Preprocessor -> Preprocessor Definitions
```

hodnotu:

```txt
_CRT_SECURE_NO_WARNINGS
```

V pripravenem souboru `Linky.vcxproj` uz je toto nastaveni hotove.
