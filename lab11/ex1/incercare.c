#include <stdio.h>

/* Numarul maxim de orase. */
#define N_MAX 100

/* Constanta care se foloseste ca valoare de initializare
   la cautarea minimului. */
#define MINIM 10000

/* Numarul de orase. */
int n;

/* Matricea distantelor dintre orase. */
int d[N_MAX][N_MAX];

/* Drumul comis voiajorului. Contine indicii oraselor in
   ordinea in care sunt ele parcurse. */
int drum[N_MAX];

/* Vector care memoreaza care orase au fost vizitate.
   vizitat[k] va fi 1 daca orasul k a fost vizitat, 0
   altfel. */
int vizitat[N_MAX];

/* Functie care alege urmatorul element care sa fie
   prelucrat din multimea oraselor. Primeste ca parametru
   ultimul oras care a fost vizitat, si returneaza
   urmatorul oras care sa fie vizitat precum si lungimea
   drumului catre acesta. */
void alege(int ultimul, int *min, int *j_min)
{
    int j;

    /* Cautam drumul minim de la ultimul oras pana la
       orasele neparcurse inca. */
    *min = MINIM;
    *j_min = -1;
    for (j = 0; j < n; j++)
        if (!vizitat[j]) {
            if (d[ultimul][j] < *min) {
                *min = d[ultimul][j];
                *j_min = j;
            }
        }
}

int main(void)
{
    FILE *fin;
    int i, j;
    int count, cost, min, j_min;

    /* Deschidem fisierul pentru citire in mod text. */
    fin = fopen("comis.in", "rt");
    if (!fin) {
        printf("Eroare: nu pot deschide fisierul.\n");
        return -1;
    }

    /* Citim datele din fisier. */
    fscanf(fin, "%d", &n);
    for (i = 0; i < n; i++)
        for (j = 0; j < n; j++)
            fscanf(fin, "%d", &(d[i][j]));

    /* Afisam pe ecran datele preluate din fisier. */
    printf("Avem %d orase.\n", n);
    printf("Distantele dintre orase sunt:\n");
    for (i = 0; i < n; i++) {
        for (j = 0; j < n; j++)
            printf("%d ", d[i][j]);
        printf("\n");
    }
    printf("\n");

    /* Initial nici un oras nu este vizitat. */
    for (i = 0; i < n; i++)
        vizitat[i] = 0;

    /* Primul oras vizitat este cel cu numarul "0". Costul
       total este zero deocamdata. */
    drum[0] = 0;
    vizitat[0] = 1;
    count = 1;
    cost = 0;

    /* Parcurgem restul de n-1 orase. */
    for (i = 0; i < n - 1; i++) {
        /* Alegem urmatorul oras care sa fie vizitat. */
        alege(drum[count - 1], &min, &j_min);

        /* Parcurgem drumul minim gasit si vizitam un nou
           oras. */
        printf("Am ales drumul (%d, %d) de cost %d.\n",
               drum[count - 1], j_min, min);
        drum[count] = j_min;
        vizitat[j_min] = 1;
        count++;
        cost += min;
    }

    /* Parcurgem drumul de la ultimul oras vizitat catre
       primul oras si actualizam costul total. */
    cost += d[drum[n - 1]][0];

    /* Afisam drumul parcurs. */
    printf("\nDrumul are costul %d si este:\n", cost);
    for (i = 0; i < n; i++)
        printf("%d ", drum[i]);
    printf("0\n");
    return 0;
}

