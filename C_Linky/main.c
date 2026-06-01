#include "linka.h"

#include <stdio.h>

static tZastavka *vyberLinku(int cisloLinky, tZastavka *linka1, tZastavka *linka2)
{
    if (cisloLinky == 1) {
        return linka1;
    }

    if (cisloLinky == 2) {
        return linka2;
    }

    return NULL;
}

int main(void)
{
    tZastavka *linka1 = nactiZastavkyDoLinky("data/linka1.txt");
    tZastavka *linka2 = nactiZastavkyDoLinky("data/linka2.txt");
    char start[MAX_NAZEV_ZASTAVKY];
    char cil[MAX_NAZEV_ZASTAVKY];
    int startovniLinka;
    int cilovaLinka;
    tZastavka *linkaStart;
    tZastavka *linkaCil;

    if (linka1 == NULL || linka2 == NULL) {
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    vlozPrestup(linka1, "zastavka3", linka2, "zastavka2", 4);

    printf("Zadej startovni linku (1 nebo 2): ");
    if (scanf("%d", &startovniLinka) != 1) {
        printf("Neplatne cislo linky.\n");
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    printf("Zadej startovni zastavku: ");
    if (scanf("%19s", start) != 1) {
        printf("Neplatny nazev zastavky.\n");
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    printf("Zadej cilovou linku (1 nebo 2): ");
    if (scanf("%d", &cilovaLinka) != 1) {
        printf("Neplatne cislo linky.\n");
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    printf("Zadej cilovou zastavku: ");
    if (scanf("%19s", cil) != 1) {
        printf("Neplatny nazev zastavky.\n");
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    linkaStart = vyberLinku(startovniLinka, linka1, linka2);
    linkaCil = vyberLinku(cilovaLinka, linka1, linka2);

    if (linkaStart == NULL || linkaCil == NULL) {
        printf("Linka musi byt 1 nebo 2.\n");
        uvolniLinku(linka1);
        uvolniLinku(linka2);
        return 1;
    }

    cestuj(linkaStart, start, linkaCil, cil);

    uvolniLinku(linka1);
    uvolniLinku(linka2);
    return 0;
}
