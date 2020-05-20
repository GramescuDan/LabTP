// bd.h - interfata cu baza de date
#pragma once
#define MAX_NUME 31
typedef struct Persoana{
char nume[MAX_NUME];
char sex;
float salariu;
struct Persoana *urm;
}Persoana;
extern Persoana *bd;
// adauga o noua persoana la baza de date
// adaugarea se face in asa fel incat baza de date este mereu sortata dupa nume
void adauga(const char *nume,char *sex,float salariu);
// sterge o persoana din baza de date
// returneaza 1 daca persoana a fost gasita, altfel 0
int sterge(const char *nume);
// elibereaza memoria ocupata de baza de date
void elibereaza();
// salveaza baza de date in fisier
void salveaza();
// incarca baza de date din fisier
void incarca();
//sorteaza baza de date
void sortare();
