# Spusteni projektu ve Visual Studiu

Nejjednodussi postup:

1. Zkopiruj celou slozku `C_Linky` kamkoliv do pocitace.
2. Otevri Visual Studio.
3. Zvol `Open a project or solution`.
4. Vyber soubor `Linky.sln` ve slozce `C_Linky`.
5. Nahore zvol konfiguraci `Debug` a platformu `x64` nebo `x86`.
6. Spust program pomoci `F5` nebo `Ctrl+F5`.

Projekt je nastaveny jako konzolova aplikace v jazyce C. Pracovni slozka je
nastavena na slozku projektu, takze program najde textove soubory:

```txt
data/linka1.txt
data/linka2.txt
```

Slozka muze byt umistena kdekoliv. Dulezite je jen nepresouvat soubory
`linka1.txt` a `linka2.txt` mimo slozku `data`.

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
