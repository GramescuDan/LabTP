#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct Cuvant{

char text[16]; // max 15 litere+terminator
struct Cuvant *pred; // inlantuire la predecesor
struct Cuvant *urm; // inlantuire la urmator
}Cuvant;
// aloca un nou cuvant si ii seteaza campul text
// campurile pred si urm raman neinitializate
Cuvant *Cuvant_nou(const char *text)
{
Cuvant *c=(Cuvant*)malloc(sizeof(Cuvant));
if(!c){
printf("memorie insuficienta");
exit(EXIT_FAILURE);
}
strcpy(c->text,text);
return c;
}
typedef struct{
Cuvant *prim; // primul cuvant din lista
Cuvant *ultim; // ultimul cuvant din lista
}Propozitie;
// initializare propozitie noua
void Propozitie_init(Propozitie *p)
{
p->prim=p->ultim=NULL;
}
Cuvant *Cuvant_adauga(Propozitie *p, const char *cuv,const char *text)
{
Cuvant *c=(Cuvant*)malloc(sizeof(Cuvant));
Cuvant *s=(Cuvant*)malloc(sizeof(Cuvant));
    strcpy(s->text,cuv);
   for(c=p->prim;c;c=c->urm){
if(!strcmp(c->text,text))
{   s->pred=c->pred;
    s->urm=c;
    c->pred=s;
}
if((c->urm==NULL)&&(strcmp(c->text,text)==0))
    {
       s->pred=c;
       s->urm=NULL;
       c->urm=s;
    }
}
return p;
}
// adauga un cuvant la sfarsitul propozitiei
void Propozitie_adauga(Propozitie *p,Cuvant *c)
{
c->pred=p->ultim; // predecesorul cuvantului este ultimul cuvant din lista
if(p->ultim){ // daca mai sunt si alte cuvinte in lista
p->ultim->urm=c; // ultimul cuvant din lista va pointa catre noul cuvant
}else{ // altfel, daca c este primul cuvant din lista
p->prim=c; // seteaza si inceputul listei la el
}
p->ultim=c; // seteaza sfarsitul listei pe noul cuvant
c->urm=NULL; // dupa cuvantul introdus nu mai urmeaza niciun cuvant
}
// cauta un text in propozitie si daca il gaseste returneaza un pointer la cuvantul respectiv
// daca nu-l gaseste, returneaza NULL
Cuvant *Propozitie_cauta(Propozitie *p,const char *text)
{
Cuvant *c;
for(c=p->prim;c;c=c->urm){
if(!strcmp(c->text,text))return c;
}
return NULL;
}
// sterge un cuvant din propozitie
void Propozitie_sterge(Propozitie *p,Cuvant *c)
{
if(c->pred){ // cuvantul nu este primul in propozitie
c->pred->urm=c->urm; // campul urm al predecesorului lui c va pointa la cuvantul de dupa c
}else{ // cuvantul este primul in propozitie
p->prim=c->urm; // seteaza inceputul listei pe urmatorul cuvant de dupa c
}
if(c->urm){ // cuvantul nu este ultimul din propozitie
c->urm->pred=c->pred; // campul pred al cuvantului de dupa c va pointa la cuvantul de dinainte de c
}else{ // cuvantul este ultimul din propozitie
p->ultim=c->pred; // seteaza sfarsitul listei pe predecesorul lui c
}
free(c);
}
// elibereaza cuvintele din memorie si reinitializeaza propozitia ca fiind vida
void Propozitie_elibereaza(Propozitie *p)
{
Cuvant *c,*urm;
for(c=p->prim;c;c=urm){
urm=c->urm;
free(c);
}
Propozitie_init(p);
}
int main()
{
Propozitie p;
int op; // optiune
char text[16];
Cuvant *c;
Propozitie_init(&p); // initializare propozitie vida
do{
printf("1 - propozitie noua\n");
printf("2 - afisare\n");
printf("3 - stergere cuvant\n");
printf("4 - iesire\n");
printf("5 - Adaugare_cuv\n");
printf("optiune: ");scanf("%d",&op);
switch(op){
case 1:
Propozitie_elibereaza(&p); // elibereaza posibila propozitie anterioara
for(;;){
scanf("%s",text);
// intre ultimul cuvant si punct trebuie sa existe un spatiu, pentru ca punctul sa fie considerat separat
if(!strcmp(text,"."))break; // atentie: "." este un sir de caractere, nu o litera (char)
Cuvant *c=Cuvant_nou(text);
Propozitie_adauga(&p,c);
}
break;
case 2:
for(c=p.prim;c;c=c->urm)printf("%s ",c->text);
printf(".\n");
break;
case 3:
printf("cuvant de sters:");scanf("%s",text);
c=Propozitie_cauta(&p,text);
if(c){
Propozitie_sterge(&p,c);
}else{
printf("cuvantul \"%s\" nu se gaseste in propozitie\n",text);
}
break;
case 4:break;
default:printf("optiune invalida");
}
}while(op!=4);
return 0;
}
