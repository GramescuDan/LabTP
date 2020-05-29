// cautarea tuturor solutiilor posibile
#include <stdio.h>
#include <stdlib.h>
// definirea labirintului
#define LMAX 100 // dimensiunea maxima pe o coordonata
// o celula are urmatoarele semnificatii:
// '.' - locatie libera
// '#' - zid
// alte caractere - locatii de pe traseu
char lab[LMAX][LMAX]; // labirintul propriu-zis; matrice patratica
int nLab; // latimea si lungimea labirintului
int startI,startJ; // punctul de pornire
// citeste un labirint
void citire(const char *numeFisier)
{
    FILE *fis=fopen(numeFisier,"rt");
    if(!fis)
    {
        printf("fisierul %s nu poate fi deschis\n",numeFisier);
        exit(EXIT_FAILURE);
    }
    fscanf(fis,"%d",&nLab);
    fscanf(fis,"%d %d",&startI,&startJ);
    int i,j;
    char linie[LMAX+1];
    fgets(linie,LMAX,fis); // consuma \n de dupa startY
    for(i=0; i<nLab; i++)
    {
        fgets(linie,LMAX,fis);
        for(j=0; j<nLab; j++)
            lab[i][j]=linie[j];
    }
    fclose(fis);
}
// afiseaza un labirint, inclusiv cu un posibil traseu
void afisare()
{
    int i,j;
    for(i=0; i<nLab; i++)
    {
        for(j=0; j<nLab; j++)
        {
            putchar(lab[i][j]);
        }
        putchar('\n');
    }
}
#define TMAX 1000 // lungimea maxima a unui traseu
int traseuI[TMAX],traseuJ[TMAX]; // coordonatele (i,j) ale punctelor de pe traseu
int nTraseu; // lungimea traseului
int nSolutii; // numarul de solutii
// delta i si j - cantitatea de adaugat la pozitia curenta (i,j) pentru a se obtine noua pozitie
int di[8]= {1,-1,0,0,-1,1,1,-1};
int dj[8]= {0,0,1,-1,-1,1,-1,1};
void solutie()
{
    int k;
    nSolutii++;
    printf("Solutia %d, lungime %d:",nSolutii,nTraseu);
    for(k=0; k<nTraseu; k++)
        printf(" (%d,%d)",traseuI[k],traseuJ[k]);
    putchar('\n');
    afisare();
    putchar('\n');
}
// cauta toate solutiile
void cauta(int i,int j) // (i,j) - pozitia curenta
{
// revenire daca pozitia curenta nu este valida:
    if(i<0||i>=nLab||j<0||j>=nLab)
        return; // daca este in afara labirintului
    if(lab[i][j]!='.')
        return; // sau daca nu este libera
// adauga pozitia curenta la traseu
    traseuI[nTraseu]=i;
    traseuJ[nTraseu]=j;
    lab[i][j]='a'+nTraseu%26; // setare pozitie ocupata cu sugerarea vizuala a traseului
    nTraseu++;
    if(i==0||i==nLab-1||j==0||j==nLab-1)  // testare daca este solutie
    {
        solutie();
    }
    else   // daca nu este solutie, incearca toate variantele posibile
    {
        int k;
        for(k=0; k<8; k++)
        {
            cauta(i+di[k],j+dj[k]);
        }
    }
// sterge pozitia curenta din traseu
    nTraseu--;
    lab[i][j]='.';
}
int main()
{
    citire("lab.txt");
    cauta(startI,startJ);
    printf("numar solutii: %d\n",nSolutii);
    return 0;
}
