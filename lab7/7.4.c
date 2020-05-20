// program cu erori (prg.c)
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#ifdef DEBUG
double test(double *v,int n,double m)
{
    for(int i=0;i<n;i++)
    {
        if (v[i]>m)
            return 0;
        else
            return 1;
    }
}
#endif
#include <assert.h>
double vmax(double *v,int n)
{
int i;
double m=v[0];
for(i=1;i<n;i++){
if(m<v[i])m=v[i];
}

return m;
}
int main()
{
double *v;
double m;
int i,n;
printf("n=");scanf("%d",&n);
if((v=(double*)malloc(n*sizeof(double)))==NULL){
printf("memorie insuficienta");
exit(EXIT_FAILURE);
}
for(i=0;i<n;i++){
printf("v[%d]=",i);scanf("%lg",&v[i]);
}
m=vmax(v,n);
assert(test(v,n,m)!=0);
printf("maximul este %lg",m);
free(v);
return 0;
}
