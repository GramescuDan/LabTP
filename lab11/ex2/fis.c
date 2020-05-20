// fis.c - functii de salvare si incarcare din fisier a bazei de date
#include "bd.h"
#include "util.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
void salveaza()
{
FILE *fis=fopen("bd.txt","wb");
if(!fis)err("nu se poate crea bd.txt");
Persoana *p;
for(p=bd;p;p=p->urm){
// fiecare persoana este salvata in fisier pe cate o linie,
// cu ';' ca separator intre nume si salariu
fprintf(fis,"%s;%g\n",p->nume,p->salariu);
}
fclose(fis);
}
void incarca()
{
elibereaza();
FILE *fis=fopen("bd.txt","rb");
// daca nu s-a putut deschide fisierul nu este eroare,
// deoarece este posibil sa fie prima rulare a programului
if(!fis)return;
char linie[MAX_NUME+32]; // o dimensiune acoperitoare pentru o linie din fisier (nume+';'+salariu+\n)
// citeste linie cu linie
while(fgets(linie,MAX_NUME+32,fis)){
char *sep=strchr(linie,';'); // cauta separatorul dintre nume si salariu
if(!sep)continue; // daca o linie nu contine separatorul, nu o ia in considerare
int nNume=sep-linie; // numarul de caractere din nume
if(nNume>=MAX_NUME)nNume=MAX_NUME-1; // ne asiguram ca numele citit nu este prea mare
char nume[MAX_NUME];
memcpy(nume,linie,nNume);
nume[nNume]='\0'; // adauga separator
float salariu=(float)atof(sep+1);
adauga(nume,salariu);
}
fclose(fis);
}
