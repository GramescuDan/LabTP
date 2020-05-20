/*
Preconditii:
-v1!=NULL
-v2!=NULL
-n1>0
-n1>n2
-elementele lui v2 sa fie incluse in v1
*/
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>

void stergere(int *v1,int *n1, int * v2,int *n2)
{
    int i=0,j=0;
    int k=0;
    for(i=0;i<*n1;i++)
    {
        for(j=0;j<*n2;j++)
        {
            if(v1[i]==v2[j])
            {
                for(k=i+1;k<*n1;k++)
                {
                    v1[k-1]=v1[k];
                }
                *n1=*n1-1;
            }

        }
    }
}

void citire (int *v,int n,int k)
{
    int i;
for(i=0;i<n;i++)
    {printf("v%d[%d]=",k,i);
    scanf("%d",&v[i]);
    }
}

int main()
{


   int n1,n2;
   printf("Dimenisunea primului vector este:");
   scanf("%d",&n1);
   assert(n1>0);
   int v1[n1];
   assert(v1!=NULL);
   citire(v1,n1,1);
   printf("Dimenisunea celui de al 2-lea vector este:");
   assert(n2<n1);
   assert(n2>0);
   scanf("%d",&n2);
   int v2[n2];
    assert(v2!=NULL);
   citire(v2,n2,2);
   stergere(v1,&n1,v2,&n2);
    for(int i=0;i<n1;i++)
        printf("%d ",v1[i]);
   printf("\nDimensiunea este %d",n1);
}
