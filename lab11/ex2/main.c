// main.c - meniu utilizator si comportament global
#include "util.h"
#include "bd.h"
#include <stdio.h>
int main()
{
char nume[MAX_NUME];
float salariu;
Persoana *p;
incarca();
for(;;){
printf("1. adaugare\n");
printf("2. stergere\n");
printf("3. listare\n");
printf("4. Modifica\n");
printf("5. iesire\n");
int op; // optiunea
printf("optiune: ");scanf("%d",&op);
switch(op){
case 1:
getchar();
citesteText("nume",nume,MAX_NUME);
salariu=citesteFloat("salariu");
adauga(nume,salariu);
break;
case 2:
getchar();
citesteText("nume",nume,MAX_NUME);
if(sterge(nume)){
printf("%s a fost sters din baza de date\n",nume);
}else{
printf("%s nu exista in baza de date\n",nume);
}
break;
case 3:
for(p=bd;p;p=p->urm){
printf("%s\t%g\n",p->nume,p->salariu);
}
break;
case 4:
getchar();
citesteText("nume",nume,MAX_NUME);
modifica(nume);
break;
case 5:
salveaza();
elibereaza();
return 0;
default:printf("optiune invalida\n");
}
}
}
