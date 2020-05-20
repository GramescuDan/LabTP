#include <stdio.h>
#include <stdlib.h>
#include <string.h>
typedef struct jucatori{
    char nume[20];
    int val;
    int aloc;
}jucatori;

int main()
{
    int n,m,max=0,k;
    printf("Numarul de jucatori:");
    scanf("%d",&n);
    printf("Numarul de echipe:");
    scanf("%d",&m);
    jucatori juc[n];
    int valoare[n];
    for(int i=0;i<n;i++)
    {
        printf("Nume:");
        scanf("%s",juc[i].nume);
        printf("Valoare:");
        scanf("%d",&juc[i].val);
        juc[i].aloc=0;
    }
    for(int i=0;i<n;i++)
    {
        for(int j=0;j<n;j++)
        {
            if((max<juc[j].val)&&(juc[j].aloc==0))
            {

                max=juc[j].val;
                k=j;
            }
        }
        max=0;
        valoare[i]=k;
        juc[k].aloc=1;
    }
    for(int i=0;i<n;i++)
    {
printf("%d\n",valoare[i]);
    }
k=0;
int i=0;
for(k=0;k<m;k++)
{
    printf("Echipa %d:\n",k);
    for(i=0;i<n/m;i++)
    {
        printf("%s %d\n",juc[valoare[(n/m)*i+k]].nume,juc[valoare[(n/m)*i+k]].val);
    }
}
//juc[valoare[(n/m)*i+k]].nume
//(n/m)*i+k
}
