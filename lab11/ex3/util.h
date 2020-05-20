// util.h - functii utilitare de uz general
#pragma once
// afiseaza un text de eroare formatat si iese din program
void err(const char *fmt,...);
// citeste in "dst" o linie de text de maxim "max" caractere
// inainte de a se citi linia, se afiseaza "text"
void citesteText(const char *text,char *dst,int max);
// citeste si returneaza o valoare de tip float
// inainte de a se citi valoarea, se afiseaza "text"
float citesteFloat(const char *text);
