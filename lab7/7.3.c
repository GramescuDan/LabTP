// program cu erori (prg.c)
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#if defined(DEBUG) || defined(_DEBUG)
double bucla(double *v,int n)
{   int i=0;
for(i=0;i<n;i++)
    printf("%lg ",v[i]);
}

#endif

double vmax(double *v,int n)
{
bucla(v,n);
int i;
double m=v[0];
for(i=0;i<n;i++){
if(m<v[i])m=v[i];
}
return m;
}
int main()
{
double *v;
int i,n;
printf("n=");scanf("%d",&n);
if((v=(double*)malloc(n*sizeof(double)))==NULL){
printf("memorie insuficienta");
exit(EXIT_FAILURE);
}
for(i=0;i<n;i++){
printf("v[%d]=",i);scanf("%lg",&v[i]);
}
printf("maximul este: %lg\n",vmax(v,n));
free(v);
return 0;
}
