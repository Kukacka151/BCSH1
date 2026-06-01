#include "linka.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

static tZastavka *najdiZastavku(tZastavka *linka, const char *nazevZastavky)
{
    while (linka != NULL) {
        if (strcmp(linka->nazevZastavky, nazevZastavky) == 0) {
            return linka;
        }

        linka = linka->dalsiZastavku;
    }

    return NULL;
}

static int jeNaLince(tZastavka *linka, tZastavka *zastavka)
{
    while (linka != NULL) {
        if (linka == zastavka) {
            return 1;
        }

        linka = linka->dalsiZastavku;
    }

    return 0;
}

static int casMeziZastavkami(tZastavka *start, tZastavka *cil)
{
    int celkovyCas = 0;
    tZastavka *aktualni = start;

    while (aktualni != NULL && aktualni != cil) {
        celkovyCas += aktualni->casNaDalsiZastavku;
        aktualni = aktualni->dalsiZastavku;
    }

    if (aktualni == cil) {
        return celkovyCas;
    }

    return -1;
}

tZastavka *vytvorZastavku(char *nazevZastavky, int cas)
{
    tZastavka *novaZastavka = (tZastavka *)malloc(sizeof(tZastavka));

    if (novaZastavka == NULL) {
        fprintf(stderr, "Chyba: nepodarilo se alokovat pamet pro zastavku.\n");
        return NULL;
    }

    strncpy(novaZastavka->nazevZastavky, nazevZastavky, MAX_NAZEV_ZASTAVKY - 1);
    novaZastavka->nazevZastavky[MAX_NAZEV_ZASTAVKY - 1] = '\0';
    novaZastavka->casNaDalsiZastavku = cas;
    novaZastavka->casNaPrestup = 0;
    novaZastavka->dalsiZastavku = NULL;
    novaZastavka->prestupZastavku = NULL;

    return novaZastavka;
}

tZastavka *nactiZastavkyDoLinky(char *jmSouboru)
{
    FILE *soubor = fopen(jmSouboru, "r");
    char radek[128];
    tZastavka *zacatek = NULL;
    tZastavka *konec = NULL;

    if (soubor == NULL) {
        fprintf(stderr, "Chyba: nepodarilo se otevrit soubor %s.\n", jmSouboru);
        return NULL;
    }

    while (fgets(radek, sizeof(radek), soubor) != NULL) {
        char nazevZastavky[MAX_NAZEV_ZASTAVKY];
        int casNaDalsiZastavku;
        tZastavka *novaZastavka;

        if (radek[0] == '\n' || radek[0] == '#') {
            continue;
        }

        if (sscanf(radek, "%19s %d", nazevZastavky, &casNaDalsiZastavku) != 2) {
            fprintf(stderr, "Varovani: preskakuji neplatny radek: %s", radek);
            continue;
        }

        novaZastavka = vytvorZastavku(nazevZastavky, casNaDalsiZastavku);
        if (novaZastavka == NULL) {
            uvolniLinku(zacatek);
            fclose(soubor);
            return NULL;
        }

        if (zacatek == NULL) {
            zacatek = novaZastavka;
        } else {
            konec->dalsiZastavku = novaZastavka;
        }

        konec = novaZastavka;
    }

    fclose(soubor);
    return zacatek;
}

void vlozPrestup(tZastavka *linka1, char *z1, tZastavka *linka2, char *z2, int casNaPrestup)
{
    tZastavka *zastavka1 = najdiZastavku(linka1, z1);
    tZastavka *zastavka2 = najdiZastavku(linka2, z2);

    if (zastavka1 == NULL || zastavka2 == NULL) {
        fprintf(stderr, "Chyba: prestup %s <-> %s nelze vytvorit, zastavka neexistuje.\n", z1, z2);
        return;
    }

    zastavka1->prestupZastavku = zastavka2;
    zastavka1->casNaPrestup = casNaPrestup;
    zastavka2->prestupZastavku = zastavka1;
    zastavka2->casNaPrestup = casNaPrestup;
}

void cestuj(tZastavka *linka1, char *z1, tZastavka *linka2, char *z2)
{
    tZastavka *start = najdiZastavku(linka1, z1);
    tZastavka *cil = najdiZastavku(linka2, z2);

    if (start == NULL) {
        printf("Startovni zastavka %s nebyla nalezena.\n", z1);
        return;
    }

    if (cil == NULL) {
        printf("Cilova zastavka %s nebyla nalezena.\n", z2);
        return;
    }

    if (linka1 == linka2) {
        int cas = casMeziZastavkami(start, cil);

        if (cas >= 0) {
            printf("Cesta %s -> %s trva %d minut.\n", z1, z2, cas);
        } else {
            printf("Cesta %s -> %s neni v danem smeru linky mozna.\n", z1, z2);
        }

        return;
    }

    {
        tZastavka *aktualni = start;
        tZastavka *nejlepsiPrestup = NULL;
        tZastavka *nejlepsiCilPrestupu = NULL;
        int casKePrestupu = 0;
        int nejlepsiCas = -1;

        while (aktualni != NULL) {
            if (aktualni->prestupZastavku != NULL && jeNaLince(linka2, aktualni->prestupZastavku)) {
                int casPoPrestupu = casMeziZastavkami(aktualni->prestupZastavku, cil);

                if (casPoPrestupu >= 0) {
                    int celkovyCas = casKePrestupu + aktualni->casNaPrestup + casPoPrestupu;

                    if (nejlepsiCas < 0 || celkovyCas < nejlepsiCas) {
                        nejlepsiCas = celkovyCas;
                        nejlepsiPrestup = aktualni;
                        nejlepsiCilPrestupu = aktualni->prestupZastavku;
                    }
                }
            }

            casKePrestupu += aktualni->casNaDalsiZastavku;
            aktualni = aktualni->dalsiZastavku;
        }

        if (nejlepsiCas >= 0) {
            printf("Cesta %s -> %s trva %d minut.\n", z1, z2, nejlepsiCas);
            printf("Prestup: %s -> %s (%d minut).\n",
                   nejlepsiPrestup->nazevZastavky,
                   nejlepsiCilPrestupu->nazevZastavky,
                   nejlepsiPrestup->casNaPrestup);
        } else {
            printf("Cesta %s -> %s neni v danem smeru linek mozna.\n", z1, z2);
        }
    }
}

void uvolniLinku(tZastavka *linka)
{
    while (linka != NULL) {
        tZastavka *dalsi = linka->dalsiZastavku;
        free(linka);
        linka = dalsi;
    }
}
