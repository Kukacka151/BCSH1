#include <stdio.h>
#include <stdlib.h>

typedef struct kontejner {
    char kod;
    struct kontejner *dalsi;
} tKontejner;

typedef struct sklad {
    tKontejner *vrchol;
} tSklad;

tSklad *initSklady(int pocet);
tKontejner *vytvorKontejner(char kod);
void pridejKontejner(tSklad *sklady, int index, tKontejner *kontejner);
tKontejner *odeberKontejner(tSklad *sklady, int index);
void presunKontejner(tSklad *sklady, int odkud, int kam);
void vypisSklady(tSklad *sklady, int pocet);
void uvolniSklady(tSklad *sklady, int pocet);

tSklad *initSklady(int pocet)
{
    tSklad *sklady = malloc((size_t)pocet * sizeof(tSklad));

    if (sklady == NULL) {
        printf("Chyba: nepodarilo se alokovat sklady.\n");
        exit(1);
    }

    for (int i = 0; i < pocet; i++) {
        sklady[i].vrchol = NULL;
    }

    return sklady;
}

tKontejner *vytvorKontejner(char kod)
{
    tKontejner *kontejner = malloc(sizeof(tKontejner));

    if (kontejner == NULL) {
        printf("Chyba: nepodarilo se alokovat kontejner.\n");
        exit(1);
    }

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
    tKontejner *odebrany = sklady[index].vrchol;

    if (odebrany != NULL) {
        sklady[index].vrchol = odebrany->dalsi;
        odebrany->dalsi = NULL;
    }

    return odebrany;
}

void presunKontejner(tSklad *sklady, int odkud, int kam)
{
    tKontejner *kontejner = odeberKontejner(sklady, odkud);

    if (kontejner == NULL) {
        printf("Varovani: sklad %d je prazdny, kontejner nelze presunout.\n", odkud);
        return;
    }

    pridejKontejner(sklady, kam, kontejner);
}

void vypisSklady(tSklad *sklady, int pocet)
{
    for (int i = 0; i < pocet; i++) {
        tKontejner *aktualni = sklady[i].vrchol;

        printf("Sklad %d:", i);

        if (aktualni == NULL) {
            printf(" prazdny");
        }

        while (aktualni != NULL) {
            printf(" %c", aktualni->kod);
            aktualni = aktualni->dalsi;
        }

        printf("\n");
    }
}

void uvolniSklady(tSklad *sklady, int pocet)
{
    for (int i = 0; i < pocet; i++) {
        tKontejner *aktualni = sklady[i].vrchol;

        while (aktualni != NULL) {
            tKontejner *dalsi = aktualni->dalsi;
            free(aktualni);
            aktualni = dalsi;
        }
    }

    free(sklady);
}

int main(void)
{
    const int pocetSkladu = 3;
    tSklad *sklady = initSklady(pocetSkladu);

    pridejKontejner(sklady, 0, vytvorKontejner('A'));
    pridejKontejner(sklady, 0, vytvorKontejner('B'));
    pridejKontejner(sklady, 1, vytvorKontejner('C'));
    pridejKontejner(sklady, 1, vytvorKontejner('D'));
    pridejKontejner(sklady, 2, vytvorKontejner('E'));

    printf("Pocatecni stav skladu:\n");
    vypisSklady(sklady, pocetSkladu);

    printf("\nPresuny kontejneru:\n");
    presunKontejner(sklady, 0, 1);
    presunKontejner(sklady, 2, 0);
    presunKontejner(sklady, 2, 1);

    printf("\nKonecny stav skladu:\n");
    vypisSklady(sklady, pocetSkladu);

    uvolniSklady(sklady, pocetSkladu);

    return 0;
}
