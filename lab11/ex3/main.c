// main.c - meniu utilizator si comportament global
#include "util.h"
#include "bd.h"
#include <stdio.h>
#include <string.h>
int main()
{
char nume[MAX_NUME];
char sex;
float salariu;
Persoana *p;
for(;;){
printf("1. adaugare\n");
printf("2. stergere\n");
printf("3. listare\n");
printf("4. iesire\n");
int op; // optiunea
printf("optiune: ");scanf("%d",&op);
switch(op){
case 1:
getchar();
citesteText("nume",nume,MAX_NUME);
printf("sex:");
scanf("%c",&sex);
if(strchr("mf",&sex)==NULL)
    err("Nu ati ales un sex");
salariu=citesteFloat("salariu");
adauga(nume,&sex,salariu);
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
printf("%s",p->nume);
printf("\t\%c\t%g\n",p->sex,p->salariu);
}
break;
case 4:
elibereaza();
return 0;
default:printf("optiune invalida\n");
}
}
}
