#include <stdio.h>
#include <stdlib.h>

#define NMAX 100

int n, v[NMAX], vol;

int tm, vm[NMAX], vn;

void citire()
{
    printf("n : ");
    scanf("%d", &n);

    for(int i=0; i<n; i++)
    {
        printf("Obiectul %d: ", i);
        scanf("%d", v+i);
    }

    printf("volumul de ocupat: ");
    scanf("%d", &vol);
}

void afisare(int a[], int n)
{
    for(int i = 0; i < n; i++)
    {
        printf("%d ", a[i]);
    }
    printf("\n");

}

int comp(const void *pa, const void *pb)
{
    int *a = (int *)pa;
    int *b = (int *)pb;

    return *a > *b;
}

void solutiemax(int a[], int an, int at)
{
    for(int i=0; i<n; i++)
    {
        vm[i]=a[i];
    }
    vn=an;
    tm=at;
}

void solutie(int s[], int t[], int sn, int tn, int sum, int p)
{
    if(sum > tm)
    {
        solutiemax(t,tn,sum);
    }

    if( vol == sum )
    {
        afisare(t, tn);

        if( p + 1 < sn && sum - s[p] + s[p+1] <= vol )
        {
            solutie(s, t, sn, tn-1, sum - s[p], p + 1);
        }
        return;
    }
    else
    {
        if( p < sn && sum + s[p] <= vol )
        {
            for( int i = p; i < sn; i++ )
            {
                t[tn] = s[i];

                if( sum + s[i] <= vol )
                {
                    solutie(s, t, sn, tn + 1, sum + s[i], i + 1);
                }
            }
        }
    }
}

void cauta()
{
    int *a=(int*)malloc(n*sizeof(int));
    if(!a)
    {
        printf("\nMemorie insuficienta!\n");
        exit(EXIT_FAILURE);
    }

    qsort(v, n, sizeof(int), &comp);

    if(v[0] <= vol)
    {
        solutie(v, a, n, 0, 0, 0);
    }
    free(a);
}

int main()
{
    vol
    citire();
    cauta();
    return 0;
}
