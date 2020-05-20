#include <stdio.h>
#include <stdlib.h>
#include <math.h>
//Se foloseste metoda Greedy.Se adauga puncte de la tastatura si apoi se face matricea distantelor dintre ele.la final se alege minimul distanelor de pe fiecare linie
//si se afiseaza punctele
typedef struct{
int gaura;
float x;
float y;
}pct;


double distanta(double xa,double ya,double xb,double yb)
{
double x=pow(xb-xa,2);
double y=pow(yb-ya,2);
return sqrt(x+y);
}

int main()
{
int n,k,nr=0;
double mindist=2000;
printf("numarul de puncte este:");
scanf("%d",&n);
double x,y;
pct puncte[n+1];
double dist[n+1][n+1];
for (int i=1;i<n+1;i++)
{puncte[i].gaura=0;
 printf("Punctul %d:\n",i);
 printf("x:");
 scanf("%g",&puncte[i].x);
 printf("y:");
 scanf("%g",&puncte[i].y);
}
puncte[0].x=0;
puncte[0].y=0;
puncte[0].gaura=0;

for (int i=0;i<n+1;i++)
{
    for (int j=0;j<n+1;j++)
    {
        if(i!=j)
        {
            dist[i][j]=distanta(puncte[i].x,puncte[i].y,puncte[j].x,puncte[j].y);
        }
    }
}
for (int i=0;i<n+1;i++)
{
    for (int j=0;j<n+1;j++)
        {
        if((i!=j)&&(puncte[j].gaura==0))
        {
        if(mindist>dist[i][j])
        {
            mindist=dist[i][j];
            k=j;
        }
        }
        }
        puncte[k].gaura=1;
        puncte[0].gaura=1;
        printf("(%g,%g)\n",puncte[k].x,puncte[k].y);
        mindist=2000;
        nr++;
        if(nr>=n)
            break;
    }
}
