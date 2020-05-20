// util.c - functii utilitare de uz general
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>
void err(const char *fmt,...)
{
va_list va;
va_start(va,fmt);
fprintf(stderr,"eroare: ");
vfprintf(stderr,fmt,va);
fputc('\n',stderr);
va_end(va);
exit(EXIT_FAILURE);
}
void citesteText(const char *text,char *dst,int max)
{
printf("%s: ",text);
fgets(dst,max,stdin);
dst[strcspn(dst,"\r\n")]='\0'; // elimina \n de la sfarsitul liniei
}
float citesteFloat(const char *text)
{
float f;
printf("%s: ",text);
scanf("%g",&f);
return f;
}
