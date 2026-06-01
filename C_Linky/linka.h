#ifndef LINKA_H
#define LINKA_H

#define MAX_NAZEV_ZASTAVKY 20

typedef struct zastavka {
    char nazevZastavky[MAX_NAZEV_ZASTAVKY];
    int casNaDalsiZastavku;
    int casNaPrestup;
    struct zastavka *dalsiZastavku;
    struct zastavka *prestupZastavku;
} tZastavka;

tZastavka *vytvorZastavku(char *nazevZastavky, int cas);
tZastavka *nactiZastavkyDoLinky(char *jmSouboru);
void vlozPrestup(tZastavka *linka1, char *z1, tZastavka *linka2, char *z2, int casNaPrestup);
void cestuj(tZastavka *linka1, char *z1, tZastavka *linka2, char *z2);
void uvolniLinku(tZastavka *linka);

#endif
