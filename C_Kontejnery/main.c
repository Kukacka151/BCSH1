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

int main(void)
{
    tSklad *sklady = initSklady(3);

    pridejKontejner(sklady, 0, vytvorKontejner('A'));
    pridejKontejner(sklady, 0, vytvorKontejner('B'));
    pridejKontejner(sklady, 1, vytvorKontejner('C'));
    pridejKontejner(sklady, 1, vytvorKontejner('D'));
    pridejKontejner(sklady, 2, vytvorKontejner('E'));

    vypisSklady(sklady, 3);

    presunKontejner(sklady, 0, 1);
    presunKontejner(sklady, 2, 0);
    presunKontejner(sklady, 2, 1);

    vypisSklady(sklady, 3);

    return 0;
}
