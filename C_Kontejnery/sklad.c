#include <stdio.h>
#include <stdlib.h>

typedef struct kontejner {
    char kod;
    struct kontejner *dalsi;
} tKontejner;

typedef struct sklad {
    tKontejner *vrchol;
} tSklad;

tSklad *initSklady(int pocet)
{
    tSklad *sklady = malloc((size_t)pocet * sizeof(tSklad));

    for (int i = 0; i < pocet; i++) {
        sklady[i].vrchol = NULL;
    }

    return sklady;
}

tKontejner *vytvorKontejner(char kod)
{
    tKontejner *kontejner = malloc(sizeof(tKontejner));

    kontejner->kod = kod;
    kontejner->dalsi = NULL;

    return kontejner;
}

void pridejKontejner(tSklad *sklady, int index, tKontejner *kontejner)
{
    kontejner->dalsi = sklady[index].vrchol;
    sklady[index].vrchol = kontejner;
}

tKontejner *odeberKontejner(tSklad *sklady, int index)
{
    tKontejner *kontejner = sklady[index].vrchol;

    if (kontejner != NULL) {
        sklady[index].vrchol = kontejner->dalsi;
        kontejner->dalsi = NULL;
    }

    return kontejner;
}

void presunKontejner(tSklad *sklady, int odkud, int kam)
{
    tKontejner *kontejner = odeberKontejner(sklady, odkud);

    if (kontejner != NULL) {
        pridejKontejner(sklady, kam, kontejner);
    }
}

void vypisSklady(tSklad *sklady, int pocet)
{
    for (int i = 0; i < pocet; i++) {
        tKontejner *kontejner = sklady[i].vrchol;

        printf("Sklad %d:", i);

        while (kontejner != NULL) {
            printf(" %c", kontejner->kod);
            kontejner = kontejner->dalsi;
        }

        printf("\n");
    }
}
