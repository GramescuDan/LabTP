// bd.c - functii de operare cu baza de date
#include "bd.h"
#include "util.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
Persoana *bd;
// creaza un nou element de tip Persoana
static Persoana *nou(const char *nume,float salariu, Persoana *urm)
{
    Persoana *p=(Persoana*)malloc(sizeof(Persoana));
    if(!p)
        err("memorie insuficienta");
    strcpy(p->nume,nume);
    p->salariu=salariu;
    p->urm=urm;
    return p;
}
void adauga(const char *nume,float salariu)
{
    Persoana *pred=NULL; // predecesorul nodului curent
    Persoana *crt; // nodul curent
// itereaza toate numele din bd
    for(crt=bd; crt; pred=crt,crt=crt->urm)
    {
// daca s-a ajuns la un nume identic cu cel de inserat sau care trebuie sa fie dupa acesta,
// atunci termina iterarea
        if(strcmp(crt->nume,nume)>=0)
            break;
    }
    if(pred)  // inserare in interiorul sau la sfarsitul listei
    {
        pred->urm=nou(nume,salariu,crt);
    }
    else   // inserare la inceputul listei
    {
        bd=nou(nume,salariu,crt);
    }
}
int sterge(const char *nume)
{
    Persoana *pred=NULL; // predecesorul nodului curent
    Persoana *crt; // nodul curent
    for(crt=bd; crt; pred=crt,crt=crt->urm)
    {
        if(!strcmp(crt->nume,nume))
        {
            if(pred)  // stergere din interiorul sau de la sfarsitul listei
            {
                pred->urm=crt->urm;
            }
            else   // stergere de la inceputul listei
            {
                bd=crt->urm;
            }
            free(crt);
            return 1;
        }
    }
    return 0;
}
void elibereaza()
{
    Persoana *crt,*urm;
    for(crt=bd; crt; crt=urm)
    {
        urm=crt->urm;
        free(crt);
    }
    bd=NULL; // pentru a se putea refolosi
}

void swap(struct Persoana *a,struct Persoana *b)
{
    float val;
    char nume[MAX_NUME];
    val=a->salariu;
    strcpy(nume,a->nume);
    a->salariu=b->salariu;
    strcpy(a->nume,b->nume);
    strcpy(b->nume,nume);
    b->salariu=val;
}
void sortare()
{
    Persoana *crt=bd;
    Persoana *pred=NULL;
    if(crt==NULL)
    {
        printf("nu avem ce sorta");
    }
    int swp=0;
    do
    {
        swp=0;
        crt=bd;
    while(crt->urm!=pred)
        {if(crt->urm->salariu<crt->salariu)
        {
            swp=1;
            swap(crt,crt->urm);

        }
        crt=crt->urm;
        }
        pred=crt;

    }
    while(swp);
}
